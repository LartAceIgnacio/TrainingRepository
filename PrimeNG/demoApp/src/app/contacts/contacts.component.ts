import { Component, OnInit, ViewChild } from '@angular/core';
import {ContactService} from '../services/ContactService';
import {Contact} from '../domain/Contact';
import {ContactClass} from '../domain/ContactClass';

import {BreadcrumbModule,MenuItem} from 'primeng/primeng';
import {DialogModule} from 'primeng/primeng';
import {ConfirmDialogModule,ConfirmationService} from 'primeng/primeng';

import {HttpClient} from '@angular/common/http';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import { PaginationResult } from "../domain/paginationresult";
import { DataTable } from "primeng/components/datatable/datatable";
import { GlobalService } from "../services/globalservice";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactService, ConfirmationService, GlobalService]
})
export class ContactsComponent implements OnInit {
  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;

  contactForm: FormGroup;

  brContact: MenuItem[];
  home: MenuItem;

  display: boolean;
  isDelete: boolean;

  searchFilter: string = "";
  paginationResult: PaginationResult<Contact>;
  totalRecords: number = 0;

  constructor(private contactService: ContactService,
  private http:HttpClient,
  private fb:FormBuilder,
  private confirmationService: ConfirmationService,
  private globalService: GlobalService) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    //this.contactService.getContacts().then(contacts => this.contactList=contacts);

    this.contactForm = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'streetaddress': new FormControl('', Validators.required),
      'cityaddress': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.required),
      'isactivated': new FormControl(''),
      'dateactivated': new FormControl('')
    });

    this.brContact=[
      {label: 'Contacts', url: '/contacts'}
    ]
    this.home = {icon: 'fa fa-home', routerLink: '/dashboard'};

    this.isDelete = false;
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Contact>>("Contacts", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.contactList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (var i = 0; i < this.contactList.length; i++) {
          this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null :
            new Date(this.contactList[i].dateActivated).toLocaleDateString();
        }
      });
  }

  searchContact() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
  }

  addContact(){
    this.contactForm.markAsPristine();
    this.display = true;
    this.isNewContact = true;
    this.isDelete = false;
    this.contactForm.enable();
    this.selectedContact = new ContactClass();
  }

  saveContact(){
    let tmpContactList = [...this.contactList];
    if(this.isNewContact){
      this.contactService.postContacts(this.selectedContact).then(contact =>{
        tmpContactList.push(contact);
        this.contactList=tmpContactList;
        this.selectedContact=null;
      });
    }
    else{
      this.contactService.putContacts(this.selectedContact).then(contact =>{
        tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
        this.contactList=tmpContactList;
        this.selectedContact=null;
      });
    }
    this.isNewContact = false;
  }

  saveAndNewContact(){
    let tmpContactList = [...this.contactList];

    this.contactForm.markAsPristine();

    this.contactService.postContacts(this.selectedContact).then(contact =>{
      tmpContactList.push(contact);
      this.contactList=tmpContactList;
    });

    this.isNewContact = true;
    this.selectedContact = new ContactClass();
  }

  cancelContact(){
    let index = this.findSelectedContactIndex();
    if (this.isNewContact)
      this.selectedContact = null;
    else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        header: 'Confirmation',
        icon: 'fa fa-question-circle',
        accept: () => {
            //Actual logic to perform a confirmation
            let tmpContactList = [...this.contactList];
            tmpContactList[index] = this.cloneContact;
            this.contactList = tmpContactList;
            this.selectedContact = Object.assign({}, this.cloneContact);
            this.display = false;
        }
    });
    }
  }

  onRowSelect(){
    this.isNewContact = false;
    this.cloneContact = this.cloneRecord(this.selectedContact);  
  }

  cloneRecord(r: Contact): Contact{
    let contact = new ContactClass();
    for(let prop in r){
      contact[prop] = r[prop];
    }
    return contact;
  }

  findSelectedContactIndex(): number{
    return this.contactList.indexOf(this.selectedContact);
  }

  deleteContact(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
          //Actual logic to perform a confirmation
          let index = this.findSelectedContactIndex();
          this.contactList = this.contactList.filter((val,i) => i!=index);
          this.contactService.deleteContacts(this.selectedContact.contactId);
          this.selectedContact = null;
      },
      reject: () => {
        this.display=false;
      }
    });
  }

  editContact(Contact: Contact){
    this.contactForm.markAsPristine();
    this.selectedContact=Contact;
    this.cloneContact = this.cloneRecord(this.selectedContact);
    this.isDelete = false;
    this.display=true;
    this.contactForm.enable();
    this.isNewContact = false;
  }

  confirmDelete(Contact: Contact){
    this.contactForm.markAsPristine();
    this.selectedContact=Contact;
    this.cloneContact = this.cloneRecord(this.selectedContact);
    this.isDelete = true;
    this.display=true;
    this.contactForm.disable();
    this.isNewContact = false;
  }

}
