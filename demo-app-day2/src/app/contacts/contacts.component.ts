import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ContactsService } from '../services/contacts.service';
import { IContacts } from '../domain/IContacts';
import { ContactsClass } from '../domain/contacts.class';


@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactsService]
})
export class ContactsComponent implements OnInit {

  contactList: IContacts[];
  selectedContact: IContacts;
  cloneContact: IContacts;
  isNewContact: boolean;
  globalIndex: number;

  constructor(private httpClient: HttpClient, private contactService: ContactsService) { }

  ngOnInit() {
    // retrieves contacts from db.
    this.getContacts();
  }

  getContacts(){
    this.contactService._getContacts()
      .then(contacts => 
      { 
          console.log("Contacts: "+ JSON.stringify(contacts));
          this.contactList = contacts; 
      });      
  }

  onRowSelect(){
    this.isNewContact = false;
    //this.cloneContact = this.cloneRecord(this.selectedContact);
    this.globalIndex = this.contactList.indexOf(this.selectedContact);
    this.selectedContact = Object.assign({}, this.selectedContact);
    this.cloneContact = Object.assign({}, this.selectedContact );
  }

  addContact(){
    this.isNewContact = true;
    this.selectedContact = new ContactsClass();
  }

  saveContact(){
    let tmpcContactList = [...this.contactList];
    if(this.isNewContact)
    {
      tmpcContactList.push(this.selectedContact);
      this.contactService._addContacts(this.selectedContact);
    }
    else
    {
      tmpcContactList[this.globalIndex] = this.selectedContact;
      this.contactService._saveContacts(this.selectedContact);
    }

    this.contactList = tmpcContactList;
    this.selectedContact = null;
    this.isNewContact = false;

    alert("------------ Retrieving Information ------------");
    this.getContacts();
  }

  cancelContact(){
    this.isNewContact = false;
    let tmpcontactList = [...this.contactList];
    tmpcontactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
    this.contactList = tmpcontactList;
    this.selectedContact =  Object.assign({}, this.cloneContact );
  }

  deleteContact(){
    let index = this.globalIndex;
    this.contactService._deleteContacts(this.selectedContact.contactId);
    this.contactList = this.contactList.filter((val,i) => i!=index);
    this.selectedContact = null;

    this.getContacts();
  }


}
