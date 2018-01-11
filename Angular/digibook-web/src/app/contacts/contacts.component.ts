import { Component, OnInit } from '@angular/core';
import { ContactService } from './../services/contactService';
import { Contact } from './../domain/contact';
import { ContactClass } from './../domain/contactclass';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import {Message,SelectItem} from 'primeng/components/common/api';
import { MenuItem , ConfirmationService} from "primeng/primeng";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactService, ConfirmationService]
})
export class ContactsComponent implements OnInit {

  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;
  contactItems: MenuItem[];
  
  clonedSelectedContact: Contact;
  indexSelected: number;

  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;
  displayDialog: boolean;
  loading: boolean;
  contact: Contact = new ContactClass();

  regexEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
  numberFormat: string = '^\\d+$';

  btnSaveNew: boolean;
  btnDelete: boolean;
  btnSave: boolean;

  display: boolean = false;
  
      showDialog() {
          this.display = true;
      }
  constructor(private contactService: ContactService , private fb: FormBuilder, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.contactService.getContacts().then(contacts => this.contactList = contacts);
      this.loading = false;
    }, 1000);
    //this.selectedContact = this.ContactList[0]; 

    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberFormat)])),
      'streetAddress': new FormControl('', Validators.required),
      'cityAddress': new FormControl('', Validators.required),
      'zipCode': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberFormat)])),
      'country': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexEmailFormat)])),
      'isActive': new FormControl('', Validators.required)
  });
  this.contactItems = [
    {label:'Dashboard', routerLink:['/dashboard'] },
    {label:'Contacts', routerLink:['/contacts']}
  ]
}

  saveContacts() {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];  
    let tmpContactList = [...this.contactList];
    if(this.isNewContact){
      //this.contact.dateActivated = Date.now();
        this.contactService.addContacts(this.selectedContact).then(contacts => {this.contact = contacts;
        tmpContactList.push(this.contact);
        this.contactList = tmpContactList;
        });
    }else{
        this.contactService.saveContacts(this.selectedContact);
        tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
    }
    //this.ContactService.saveContacts(this.selectedContact);
    this.selectedContact = null;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
this.userform.markAsPristine();
  }

  saveNewContacts() {
    this.userform.enable();
   let tmpContactList = [...this.contactList];
    if(this.isNewContact){
      //this.contact.dateActivated = Date.now();
        this.contactService.addContacts(this.selectedContact).then(contacts => {this.contact = contacts;
        tmpContactList.push(this.contact);
        this.contactList = tmpContactList;
        });
    }else{
        this.contactService.saveContacts(this.selectedContact);
        tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
    }
    //this.ContactService.saveContacts(this.selectedContact);
    this.selectedContact = new ContactClass;
    this.userform.markAsPristine();
  }

  addContacts() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewContact = true;
     this.selectedContact = new ContactClass;
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Contact: Contact){
    this.userform.disable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedContact = Contact;
    this.cloneContact = this.cloneRecord(this.selectedContact);
  }

  deleteContacts(){
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
    this.selectedContact;
     let index = this.findSelectedContactIndex();
     this.contactList = this.contactList.filter((val,i) => i!=index);
    this.contactService.deleteContacts(this.selectedContact.contactId);
    this.selectedContact = null;
    this.displayDialog = false;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  editContacts(Contact: Contact){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewContact = false;
    this.selectedContact = Contact;
    this.cloneContact = this.cloneRecord(this.selectedContact);
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  // onRowSelect(event) {
  //         this.isNewContact = false;
  //         this.selectedContact;
  //         this.cloneContact = this.cloneRecord(this.selectedContact);
  //         this.displayDialog = true;
  // } 

  cloneRecord(r: Contact): Contact {
      let Contact = new ContactClass();
      for(let prop in r) {
          Contact[prop] = r[prop];
      }
      return Contact;
  }

  cancelContacts(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
    
    this.isNewContact = false;
    let tmpContactList = [...this.contactList];
    tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
    this.contactList = tmpContactList;
    this.selectedContact = this.cloneContact;
    this.selectedContact = null;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  findSelectedContactIndex(): number {
      return this.contactList.indexOf(this.selectedContact);
  }
}