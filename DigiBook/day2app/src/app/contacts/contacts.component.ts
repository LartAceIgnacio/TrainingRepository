import { Component, OnInit, ViewChild } from '@angular/core';
import { ContactService } from '../services/contactservice';
import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';

import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';
import { AuthService } from "../services/auth.service";
import { GlobalService } from "../services/globalservice";
import { Pagination } from "../domain/pagination";
import { DataTable } from "primeng/components/datatable/datatable";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class ContactsComponent implements OnInit {

  contact: Contact = new ContactClass();
  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;

  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  description: string;

  items: MenuItem[];

  home: MenuItem;


  display: boolean = false;

  constructor(private globalService: GlobalService, private fb: FormBuilder, private conf: ConfirmationService
    , public auth: AuthService) { }

  newContact() {
    this.display = true;
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
  }

  editContact(contact: Contact) {
    this.display = true;
    this.selectedContact = contact
    this.isNewContact = false;
    this.cloneContact = this.cloneRecord(this.selectedContact);

  }

  deleteCont(contact: Contact) {
    this.selectedContact = contact;
    this.conf.confirm({
      message: 'Are you sure that you want to delete this data?',
      accept: () => {
        if (this.selectedContact.contactId == null)
          this.selectedContact = new ContactClass();
        else {
          this.globalService.deleteSomething("contacts", this.selectedContact.contactId)
          let index = this.contactList.indexOf(this.selectedContact);
          this.contactList = this.contactList.filter(
            (val, i) => i != index);
          this.contact = null;
        }
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Contact deleted!' });
        this.selectedContact = new ContactClass();
      }
    });

  }

  saveAndNewContact() {
    let tmpContactList = [...this.contactList];
    this.globalService.addSomething("contacts", this.selectedContact).then(emp => {
      tmpContactList.push(emp);
      this.contactList = tmpContactList;
      this.selectedContact = new ContactClass();
    });
    this.userform.markAsPristine();
    this.msgs = [];
    this.msgs.push({ severity: 'success', summary: 'Contact Saved!' });
  }

  ngOnInit() {
    // this.globalService.getSomething("contacts")
    //   .then(contacts => this.contactList = contacts);

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'phone': new FormControl('', Validators.required),
      'streetaddress': new FormControl('', Validators.required),
      'cityaddress': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.required)
    });

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Contact', routerLink: ['/contacts'] }
    ];
    this.home = { icon: 'fa fa-home' };

  }

  addContact() {
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
  }

  saveContact() {
    let tmpContactList = [...this.contactList];
    if (this.isNewContact) {
      this.globalService.addSomething("contacts", this.selectedContact).then(emp => {
        tmpContactList.push(emp);
        this.contactList = tmpContactList;
        this.selectedContact = null;
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Contact Added!' });
      });
    }

    else {
      this.globalService.updateSomething("contacts", this.selectedContact.contactId, this.selectedContact)
      tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
      this.msgs = [];
      this.msgs.push({ severity: 'success', summary: 'Contact Saved!' });
    }
    this.userform.markAsPristine();
    this.isNewContact = false;
    this.display = false;
  }

  onRowSelect() {
    this.isNewContact = false;
    this.cloneContact = this.cloneRecord(this.selectedContact);
  }

  cloneRecord(r: Contact): Contact {
    let contact = new ContactClass();
    for (let prop in r) {
      contact[prop] = r[prop];
    }
    return contact;
  }

  cancelContact() {
    this.isNewContact = false;
    let tmpContactList = [...this.contactList];
    tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
    this.contactList = tmpContactList;
    this.selectedContact = this.cloneContact;
    this.selectedContact = new ContactClass();
    this.display = false;
    this.userform.markAsPristine();
  }
  cancelCon() {
    if (this.isNewContact == true) {
      this.display = false;
    }
    else {
      if (this.userform.dirty) {
        this.conf.confirm({
          message: 'Do you want to discard changes?',
          accept: () => {
            this.cancelContact()
          }
        });
      }
      else {
        this.display = false;
      }
    }
    this.userform.markAsPristine();
  }
  deleteContact() {
    if (this.selectedContact.contactId == null)
      this.selectedContact = new ContactClass();
    else {
      this.globalService.deleteSomething("contacts", this.selectedContact.contactId)
      let index = this.contactList.indexOf(this.selectedContact);
      this.contactList = this.contactList.filter((val, i) => i != index);
      this.contact = null;
    }
    this.selectedContact = new ContactClass();
  }

  entity: string = "contacts";
  searchQuery: string = "";
  totalRecord: number;

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Contact>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.contactList = result.result;
        this.totalRecord = result.totalCount;
      });
  }

  @ViewChild('dt') public dataTable: DataTable;
  search() {
    this.dataTable.reset();
  }
}
