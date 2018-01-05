import { Component, OnInit } from '@angular/core';
import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { Message } from '../domain/message';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [GlobalService, ConfirmationService]
})

export class ContactsComponent implements OnInit {
  contactList: Contact[];
  cloneContact: Contact;
  selectedContact: Contact;
  isNewContact: boolean;
  isDeleteContact: boolean = false;
  indexOfContact: number;
  totalRecords: number = 0;
  dateActivate: any;
  minDate: Date = new Date();
  isTrueFalse: object[];
  selectedActive: string;
  userform: FormGroup;
  msgs: Message[] = [];
  rexExpEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.isTrueFalse = [];
    this.isTrueFalse.push({ label: 'true', value: 'true' });
    this.isTrueFalse.push({ label: 'false', value: 'false' });

    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.rexExpEmailFormat)])),
      'streetAddress': new FormControl('', Validators.required),
      'cityAddress': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'zipCode': new FormControl('', Validators.compose([Validators.required, Validators.min(1)])),
      'active': new FormControl(''),
      'dateActivated': new FormControl(''),
    });

    // this.globalService.getSomething<Contact>("Contacts").then(contacts => 
    //   {
    //     this.contactList = contacts;
    //     for (var i = 0; i < this.contactList.length; i++) {
    //        this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null : 
    //         new Date(this.contactList[i].dateActivated).toLocaleDateString();
    //     }
    //   });
  }

  paginate(event) {
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
    this.globalService.getSomethingWithPagination<Contact>("Contacts", event.first, event.rows, "").then(contacts => {
      this.contactList = contacts;
      this.totalRecords = this.contactList.length;
      for (var i = 0; i < this.contactList.length; i++) {
        this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null :
          new Date(this.contactList[i].dateActivated).toLocaleDateString();
      }
    });
  }

  addContact() {
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
    this.selectedActive = "false";
    this.dateActivate = null;
    this.userform.clearValidators();
  }

  onRowSelect(contact) {
    this.userform.clearValidators();
    this.isNewContact = false;
    this.selectedContact = contact;
    this.indexOfContact = this.contactList.indexOf(this.selectedContact);
    this.selectedActive = this.selectedContact.isActive ? "true" : "false";
    this.dateActivate = this.selectedContact.dateActivated == null ? null :
      new Date(this.selectedContact.dateActivated).toLocaleDateString();
    this.cloneContact = Object.assign({}, this.selectedContact);
    this.selectedContact = Object.assign({}, this.selectedContact);
  }

  btnSave() {
    let tmpContactList = [...this.contactList];
    this.selectedContact.isActive = this.selectedActive == 'true' ? true : false;
    this.selectedContact.dateActivated = this.dateActivate == null || this.dateActivate == undefined ? null :
      this.dateActivate;
    if (this.isNewContact) {
      this.globalService.addSomething<Contact>("Contacts", this.selectedContact).then(contacts => {
        tmpContactList.push(contacts);
        this.contactList = tmpContactList;
        this.selectedContact = null;
        this.isNewContact = false;
      });
    }
    else {
      this.globalService.updateSomething<Contact>("Contacts", this.selectedContact.contactId, this.selectedContact).then(contacts => {
        tmpContactList[this.indexOfContact] = this.selectedContact;
        this.contactList = tmpContactList;
        this.selectedContact = null;
        this.isNewContact = false;
      });
    }
  }

  btnCancel() {
    if (this.isNewContact || this.isDeleteContact) {
      this.selectedContact = null;
      this.isDeleteContact = false;
      this.isNewContact = false
    }
    else {
      let tmpContactList = [...this.contactList];
      tmpContactList[this.indexOfContact] = this.cloneContact;
      this.contactList = tmpContactList;
      this.selectedContact = Object.assign({}, this.cloneContact);
    }
  }

  btnDelete(contact) {
    this.selectedContact = contact;
    this.indexOfContact = this.contactList.indexOf(contact);
    this.selectedActive = this.selectedContact.isActive ? "true" : "false";
    this.isDeleteContact = true;
  }

  btnOk() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];
        let tmpContactList = [...this.contactList];
        this.globalService.deleteSomething<Contact>("Contacts", this.selectedContact.contactId).then(contacts => {
          tmpContactList.splice(this.indexOfContact, 1);
          this.contactList = tmpContactList;
          this.selectedContact = null;
          this.isNewContact = false;
          this.isDeleteContact = false;
        });
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
        this.isDeleteContact = true;
      }
    });
  }
}
