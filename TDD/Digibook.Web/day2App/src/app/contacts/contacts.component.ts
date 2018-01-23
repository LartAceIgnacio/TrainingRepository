import { Component, OnInit } from '@angular/core';
import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';
import { ConfirmationService, MenuItem, DataTable } from 'primeng/primeng';
import { Message, SelectItem } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder} from '@angular/forms';
import { validateConfig } from '@angular/router/src/config';
import { PatternValidator } from '@angular/forms/src/directives/validators';
import { GenericService } from '../services/genericservice';

import { ViewChild } from '@angular/core';
import { Record } from '../domain/record';

import { REGEXEMAIL } from '../services/constants';
@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ConfirmationService, GenericService]
})

export class ContactsComponent implements OnInit {
  items : MenuItem[] = [];

  display : boolean = false;
  isEdit : boolean = false;
  contactList : Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;
  contact: Contact;
  userform : FormGroup;

  btnSave : boolean = true;
  btnSaveNew : boolean = true;
  btnDelete : boolean = true;

  //datatable components
  searchFilter: string = "";
  totalRecord: number = 0;
  searchButtonClickCtr: number = 0;
  retrieveRecordResult: Record<Contact>;

  msgs: Message[] = [];

  service: string = "Contacts";

  constructor(private confirmationService: ConfirmationService, private fb: FormBuilder, private genericService: GenericService
  ) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.items = [
      {label: 'Dashboard', icon: 'fa fa-book fa-5x', routerLink: ['/dashboard']},
      {label: 'Contacts', icon: 'fa fa-book fa-5x', routerLink: ['/contacts']}
    ]
    
    this.userform = this.fb.group({
      'firstName' : new FormControl('', Validators.required),
      'lastName' : new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern('^\\d+$')])),
      'streetAddress': new FormControl('', Validators.required),
      'cityAddress': new FormControl('', Validators.required),
      'zipCode' : new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(REGEXEMAIL)])),
    });
    
  }
  
  retrieveRecord(event){
    this.genericService.getPaginatedRecord<Record<Contact>>(this.service, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then( contactRecord => {
          this.retrieveRecordResult = contactRecord;
          this.contactList = this.retrieveRecordResult.result;
          this.totalRecord = this.retrieveRecordResult.totalRecord;
    });
  }

  addContact(){
    this.userform.enable();
    this.btnSave = true;
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.isEdit = false;
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
    this.display = true;
  }

  saveContact(isSaveNew: boolean){
    let tmpContactList = [...this.contactList];
    if(this.isNewContact)
    {
      this.genericService.insertRecord<Contact>(this.service, this.selectedContact).then(contact =>{
        this.contact = contact; 
        tmpContactList.push(this.contact);
        this.contactList = tmpContactList;  
        this.selectedContact = isSaveNew? new ContactClass(): null;
        this.dataTable.reset();
        }).then( () => this.msgs.push({severity:'success', summary:'Success Message', detail:'New Contact: '+ this.contact.lastName +' Added'}));;
    } 
    else{
      this.genericService.updateRecord(this.service, this.selectedContact.contactId, this.selectedContact).then(contact =>
        {this.contact = contact; 
        tmpContactList[this.contactList.indexOf(this.selectedContact)] = contact;
        this.contactList = tmpContactList;
        });
    }
    this.userform.markAsPristine();
  }

  cloneRecord(r: Contact): Contact{
    let contact = new ContactClass();
    for(let prop in r){
      contact[prop] = r[prop];
    }
    return contact;
  }

  deleteContact(contact: Contact){
      this.userform.markAsPristine();
      this.isEdit = false;
      this.userform.disable();
      this.cloneContact = this.cloneRecord(contact);
      this.selectedContact = contact;
      this.display = true;
      this.btnSave = false;
      this.btnSaveNew = false;     
      this.btnDelete = true;
  }

  deleteContactConfirmation(){
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let tmpContactList = [...this.contactList];
        this.genericService.deleteRecord(this.service, this.selectedContact.contactId);
        tmpContactList.splice(this.contactList.indexOf(this.selectedContact), 1);
    
        this.contactList = tmpContactList;
        this.selectedContact   = null;
        this.isNewContact = false;  
        this.msgs = [{severity:'info', summary:'Confirmed', detail:'Record deleted'}];
      },
      reject: () => {
          this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
      }
    });
  }

  cancelContact(){
    if(this.isNewContact){
      this.selectedContact = null;
    }
    else{
      if(this.isEdit)
      {
        if(this.userform.dirty){
          this.confirmationService.confirm({
            message: 'Are you sure you want to discard changes?',
            header: 'Discard Changes',
            icon: 'fa fa-pencil',
            accept: () => {
              let tmpContactList = [...this.contactList];
              tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
              this.contactList = tmpContactList;
              this.selectedContact = null;
            },
            reject: () => {

            }
          });
          this.userform.markAsPristine();
        }else{
          this.selectedContact = null;
        }
       }else{
        this.selectedContact = null;
       }
    }
    this.userform.markAsPristine();
  }

  editContact(contact: Contact){
    this.isEdit = true;
    this.userform.enable();
    this.btnSave = true;
    this.btnDelete = false;
    this.btnSaveNew = false;
    this.cloneContact =  Object.assign({}, contact);
    this.isNewContact = false;
    this.selectedContact = contact;
    this.display = true;
  }

  search(){
    this.dataTable.reset();
  }
}
