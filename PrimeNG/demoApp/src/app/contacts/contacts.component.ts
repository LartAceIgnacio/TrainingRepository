import { Component, OnInit } from '@angular/core';
import {ContactService} from '../services/ContactService';
import {Contact} from '../domain/Contact';
import {ContactClass} from '../domain/ContactClass';

import {HttpClient} from '@angular/common/http';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactService]
})
export class ContactsComponent implements OnInit {

  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;

  contactForm: FormGroup;

  constructor(private contactService: ContactService,
  private http:HttpClient,
  private fb:FormBuilder) { }

  ngOnInit() {
    this.contactService.getContacts().then(contacts => this.contactList=contacts);

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
  }

  addContact(){
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
  }

  saveContact(){
    let tmpContactList = [...this.contactList];
    if(this.isNewContact){
      tmpContactList.push(this.selectedContact);
      this.contactService.postContacts(this.selectedContact);
    }
    else{
      tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
      this.contactService.putContacts(this.selectedContact);
    }

    this.contactList=tmpContactList;
    this.selectedContact=null;
    this.isNewContact = false;
  }

  cancelContact(){
    let index = this.findSelectedContactIndex();
    if (this.isNewContact)
      this.selectedContact = null;
    else {
      let tmpContactList = [...this.contactList];
      tmpContactList[index] = this.cloneContact;
      this.contactList = tmpContactList;
      this.selectedContact = Object.assign({}, this.cloneContact);
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
    let index = this.findSelectedContactIndex();
    this.contactList = this.contactList.filter((val,i) => i!=index);
    this.contactService.deleteContacts(this.selectedContact.contactId);
    this.selectedContact = null;
  }

}
