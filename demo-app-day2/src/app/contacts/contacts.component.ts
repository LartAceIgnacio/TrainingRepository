import { Component, OnInit, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ContactsService } from '../services/contacts.service';
import { IContacts } from '../domain/IContacts';
import { ContactsClass } from '../domain/contacts.class';

import * as moment from 'moment';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactsService]
})
export class ContactsComponent implements OnInit, OnChanges/*, OnDestroy*/ {


  contactList: IContacts[];
  selectedContact: IContacts;
  cloneContact: IContacts;
  isNewContact: boolean;
  globalIndex: number;
  dateActivated: string;
  checked: boolean = true;

  constructor(private httpClient: HttpClient, private contactService: ContactsService) { }

  ngOnInit() {
    this.dateActivated = moment().format('L');
    // retrieves contacts from db.
    this.getContacts();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getContacts();
  }

  getContacts() {
    this.contactService._getContacts()
      .then(contacts => {
        console.log("Contacts: " + JSON.stringify(contacts));
        this.contactList = contacts;
      });
  }

  onRowSelect() {
    this.isNewContact = false;
    //this.cloneContact = this.cloneRecord(this.selectedContact);
    this.globalIndex = this.contactList.indexOf(this.selectedContact);
    this.selectedContact = Object.assign({}, this.selectedContact);
    this.cloneContact = Object.assign({}, this.selectedContact);

    console.log(this.selectedContact);
  }

  addContact() {
    this.isNewContact = true;
    this.selectedContact = new ContactsClass();
    this.selectedContact.dateActivated = this.dateActivated;
  }

  saveContact() {
    let tmpContactList = [...this.contactList];
    if (this.isNewContact) {
      this.contactService._addContacts(this.selectedContact)
        .then(contacts => {
          tmpContactList.push(contacts["result"]);
          this.contactList = tmpContactList;
          this.selectedContact = null;
        });
    }
    else {
      this.contactService._updateContacts(this.selectedContact)
        .then(contacts => {
          tmpContactList[this.globalIndex] = this.selectedContact;
          this.contactList = tmpContactList;
          this.selectedContact = null;
        });
    }
    this.isNewContact = false;

  }

  cancelContact() {
    if (this.isNewContact) {
      this.selectedContact = null;
    }
    else {
      this.isNewContact = false;
      let tmpContactList = [...this.contactList];
      tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
      this.contactList = tmpContactList;
      this.selectedContact = Object.assign({}, this.cloneContact); //null;
    }
  }

  deleteContact() {
    let index = this.globalIndex;
    this.contactService._deleteContacts(this.selectedContact.contactId);

    this.contactList = this.contactList.filter((val, i) => i != index);
    this.selectedContact = null;
  }

  ngOnDestroy() {
    this.getContacts();
  }
}
