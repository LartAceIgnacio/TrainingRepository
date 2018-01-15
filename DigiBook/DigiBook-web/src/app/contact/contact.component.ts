import { Component, OnInit } from '@angular/core';
import { GlobalService } from "../services/globalService";
import { Contact } from "../domain/contact";
import { ContactClass } from "../domain/contactClass";
import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { PaginationResult } from "../domain/paginationresult";
import { ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';
import { API_URL } from "../services/constants";

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css'],
  providers: [GlobalService]
})

export class ContactComponent implements OnInit {
  //contact variable
  contactList: Contact[];
  selectedContact:Contact;
  cloneContact:Contact;
  isNewContact:boolean;
  //regex  
  regexNumberFormat: string = "^[0-9]*$";
  regexEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
  //url 
  service: string = 'contacts';
  serviceUrl: string = `${API_URL}/${this.service}/`;
  //form
  contactForm: FormGroup;
  dialogTitle: String;
  //displayable
  displayInputBox: boolean = false;
  onEdit: boolean = false;
  onDelete: boolean = false;
  onAdd: boolean = false;
  //confirmation
  confirmEditCancel: boolean = false;
  confirmDeleteRecord: boolean = false;
  //pagination  
  totalRecords: number = 0;
  searchFilter: string = "";
  paginationResult: PaginationResult<Contact>;

  constructor(private globalService : GlobalService, private formbuilder: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {

    //this.globalService.retrieve(this.path).then(contacts => this.contactList = contacts);    
    this.contactForm = this.formbuilder.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexNumberFormat)])),
      'streetAddress': new FormControl('', Validators.required),
      'cityAddress': new FormControl('', Validators.required),
      'zipCode': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexNumberFormat)])),
      'country': new FormControl('', Validators.required),  
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexEmailFormat)])),
      'isActive': new FormControl(''),  
      'dateActivated': new FormControl(''),
      });
  }

  paginate(event) {
    this.globalService.retrieveWithPagination<PaginationResult<Contact>>(this.serviceUrl, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.contactList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
      });
  }

  searchContact() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    let paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }

  //--------------------CREATE-----------------------
  addContact(){
      this.isNewContact = true;
      this.selectedContact = new ContactClass();
      //form
      this.displayForm("New Contact", true);
      //displayable      
      this.onEdit= false;
      this.onDelete = false;
      this.onAdd = true;
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = false;
  }  
  //--------------------EDIT-----------------------
  editContact(contact: Contact){    
    this.isNewContact = false;
    this.selectedContact = contact;
    this.cloneContact = this.cloneRecord(this.selectedContact);
    //form
    this.displayForm("Edit Contact", true);
    //displayable      
    this.onEdit= true;
    this.onDelete = false;
    this.onAdd = false;
  }
  //--------------------DELETE-----------------------
  deleteContact(contact: Contact, rowSelect: boolean){
    //displayable      
    this.onEdit= false;
    this.onDelete = true;
    this.onAdd = false;

    if(rowSelect){
      this.selectedContact = contact;
      //form
      this.displayForm("Delete Contact", false);
    }
    else{
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = true;
    }
  }  
  //--------------------FORM-----------------------
  saveContact(withNew:boolean){
    let tempContactList = [...this.contactList];
    if(this.isNewContact){
      this.globalService.add(this.serviceUrl, this.selectedContact).then(contact => {
        tempContactList.push(contact);
        this.contactList = tempContactList;
        this.selectedContact = null;
        this.displayInputBox = false;
        if(withNew)
          this.addContact();
          this.setCurrentPage(1);
        })
    }
    else{      
      this.globalService.update(this.serviceUrl, this.selectedContact, this.selectedContact.contactId).then(contact => {
        tempContactList[this.contactList.indexOf(this.selectedContact)];
        this.contactList = tempContactList;
        this.selectedContact = null;
        this.displayInputBox = false;
        })
    }
    this.isNewContact = false;
  }  

  cancelContact(){
    if(this.onAdd){
      this.toCancel(true);
    }
    else if(this.onEdit){
      if(this.contactForm.dirty){  
        this.confirmEditCancel = true;
      }
      if(this.contactForm.pristine){
        this.displayInputBox = false;
      }
    }
    else if(this.onDelete){
      this.confirmDeleteRecord = false;
      this.displayInputBox = false;
    }    
    else{
      this.displayInputBox = false;
    }
  }
  //--------------------CONFIRMATION-----------------------
  toCancel(discard:boolean){  
    if(discard) {
      if(this.onEdit){
        this.isNewContact = false;
        let tempContactList = [...this.contactList];
        tempContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
        this.contactList = tempContactList;
        this.selectedContact = this.cloneContact;
        this.cloneContact = this.cloneRecord(this.selectedContact);
        this.contactForm.markAsPristine();
      }
      else{
        //displayable
        this.displayInputBox = false;
      }      
    }
    this.confirmEditCancel = false;
  }

  toDelete(toDelete: boolean){
    if(toDelete){    
      this.displayInputBox = true;
      this.globalService.delete(this.serviceUrl, this.selectedContact, this.selectedContact.contactId).then(contact => {
      let tempContactList = [...this.contactList];
      tempContactList.splice(this.contactList.indexOf(this.selectedContact), 1);   
      this.contactList = tempContactList;
      this.selectedContact = null;
      })
    }   
    this.displayInputBox = false; 
    this.confirmDeleteRecord = false;
  }   
  //--------------------MISC-----------------------
  cloneRecord(r: Contact): Contact {
    let contact = new ContactClass();
    for (let prop in r) {
      contact[prop] = r[prop];
    }
    return contact;
  }

  displayForm(title:string, editable: boolean){
    this.dialogTitle = title;
    this.contactForm.markAsPristine();
    if(editable){
      this.contactForm.enable();
    }
    else{
      this.contactForm.disable();
    }
    this.displayInputBox = true;
  }
}

class PaginationResultClass implements PaginationResult<Contact>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}