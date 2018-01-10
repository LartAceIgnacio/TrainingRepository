import { Component, OnInit } from '@angular/core';
import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { PaginationResult } from "../domain/paginationresult";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [GlobalService, ConfirmationService]
})

export class ContactsComponent implements OnInit {
  contactList: Contact[];
  contactListDefault: Contact[];
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
  searchFilter: string = "";
  paginationResult: PaginationResult<Contact>;
  rexExpEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

    
  @ViewChild('dt') public dataTable: DataTable;
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
  }

  paginate(event) {
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
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
    // let paging = {
    //     first: ((n - 1) * this.dataTable.rows),
    //     rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }

  addContact() {
    this.userform.enable();
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
    this.selectedActive = "false";
    this.dateActivate = null;
  }

  onRowSelect(contact) {
    this.userform.enable();
    this.isNewContact = false;
    this.selectedContact = contact;
    this.indexOfContact = this.contactList.indexOf(this.selectedContact);
    this.selectedActive = this.selectedContact.isActive ? "true" : "false";
    this.dateActivate = this.selectedContact.dateActivated == null ? null :
      new Date(this.selectedContact.dateActivated).toLocaleDateString();
    this.cloneContact = Object.assign({}, this.selectedContact);
    this.selectedContact = Object.assign({}, this.selectedContact);
  }

  btnSave(isSaveAndNew: boolean) {
    this.userform.markAsPristine();
    let tmpContactList = [...this.contactList];
    this.selectedContact.isActive = this.selectedActive == 'true' ? true : false;
    this.selectedContact.dateActivated = this.dateActivate == null || this.dateActivate == undefined ? null :
      this.dateActivate;
    if (this.isNewContact) {
      this.globalService.addSomething<Contact>("Contacts", this.selectedContact).then(contacts => {
        tmpContactList.push(contacts);
        this.contactList = tmpContactList;
        this.selectedContact = isSaveAndNew ? new ContactClass() : null;
        this.isNewContact = isSaveAndNew ? true : false;
        if(!isSaveAndNew) {
          this.setCurrentPage(1); 
        }
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
    if (this.isNewContact || this.isDeleteContact || !this.userform.dirty) {
      this.selectedContact = null;
      this.isDeleteContact = false;
      this.isNewContact = false
      this.userform.markAsPristine();
    }
    else if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to discard the changes?',
        header: 'Cancel Confirmation',
        icon: 'fa fa-ban',
        accept: () => {
          let tmpContactList = [...this.contactList];
          tmpContactList[this.indexOfContact] = this.cloneContact;
          this.contactList = tmpContactList;
          this.selectedContact = Object.assign({}, this.cloneContact);
          this.userform.markAsPristine();
        }
      });
    }
  }

  btnDelete(contact) {
    this.userform.disable();
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
        let tmpContactList = [...this.contactList];
        this.globalService.deleteSomething<Contact>("Contacts", this.selectedContact.contactId).then(contacts => {
          tmpContactList.splice(this.indexOfContact, 1);
          this.contactList = tmpContactList;
          this.selectedContact = null;
          this.isNewContact = false;
          this.isDeleteContact = false;
          this.setCurrentPage(1);
        });
      }
    });
  }
}

class PaginationResultClass implements PaginationResult<Contact>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}
