import { Component, OnInit } from '@angular/core';
import { Contact } from '../domain/contacts/contact';
import { ContactClass } from '../domain/contacts/contactclass';
import { ContactService } from '../services/contactservice';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class ContactsComponent implements OnInit {

  items: MenuItem[];

  home: MenuItem;

  displayDialog: boolean;

  contact: Contact = new ContactClass();

  selectedContact: Contact;

  newContact: boolean;

  contacts: Contact[];

  isDelete: boolean;

  isEdit: boolean;

  isNewContact: boolean;

  loading: boolean;

  userform: FormGroup;

  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';

  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  paginationResult: PaginationResult<Contact>;

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService, private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    // this.contactService.getContacts().then(contacts => this.contacts = contacts);
    // this.contactService.getContacts()
    // .then(contacts => this.contacts = contacts);

    this.loading = true;
    setTimeout(() => {
      // this.contactService.getContacts().then(contacts => this.contacts = contacts);
      this.loading = false;
    }, 1000);


    this.items = [
      { label: 'Home', routerLink: ['/dashboard'] },
      { label: 'Contact' }
    ];
    this.home = { icon: 'fa fa-home' };

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.required),
      'streetaddress': new FormControl('', Validators.required),
      'cityaddress': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.required)
    });

  }

  showDialogToAdd() {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = true;
    this.isDelete = false;
    this.newContact = true;
    this.selectedContact = null;
    this.contact = new ContactClass();
    this.displayDialog = true;
  }

  onRowSelect(clickContact: Contact) {
    this.newContact = false;
    this.contact = this.cloneContact(clickContact);
    this.selectedContact = clickContact;
    this.displayDialog = true;
  }

  cloneContact(c: Contact): Contact {
    const contact = new ContactClass();
    // tslint:disable-next-line:forin
    for (const prop in c) {
      contact[prop] = c[prop];
    }
    return contact;
  }

  save(number: boolean) {
    this.isNewContact = number;

    const contacts = [...this.contacts];
    if (this.newContact) {
      this.globalService.addSomething('Contacts', this.contact).then(
        data => {
          this.contact = data;
          contacts.push(this.contact);
          this.contacts = contacts;
          this.contact = new ContactClass();
          this.dataTable.reset();
          // this.userform.markAsPristine();
        }
      );

    } else {
      this.globalService.updateSomething('Contacts', this.contact.contactId, this.contact).then(
        data => {
          this.contact = data;
          contacts[this.findSelectedContactIndex()] = this.contact;
          this.contacts = contacts;
        }
      );
    }

    if (this.isNewContact) {
      this.userform.markAsPristine();
      // this.contacts = contacts;
      this.newContact = true;
      this.selectedContact = null;


    } else {
      // this.contacts = contacts;
      this.contact = null;
      this.displayDialog = false;
      // this.setCurrentPage(1);
    }

  }

  findSelectedContactIndex(): number {
    return this.contacts.indexOf(this.selectedContact);
  }

  delete(clickContact: Contact) {
    this.isDelete = true;
    this.userform.disable();
    this.onRowSelect(clickContact);

  }

  deleteContact() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        this.globalService.deleteSomething('Contacts', this.contact.contactId);
        const index = this.findSelectedContactIndex();
        this.contacts = this.contacts.filter((val, i) => i !== index);
        this.contact = null;
        this.displayDialog = false;
        this.dataTable.reset();
      }
    });
  }

  edit(clickContact: Contact) {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = false;
    this.isDelete = false;
    this.onRowSelect(clickContact);
  }

  cancel() {
    if (this.isDelete) {
      this.displayDialog = false;
    } else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to cancel?',
        accept: () => {
          this.contact = Object.assign({}, this.selectedContact);
        }
      });
    }
  }

  paginate(event) {
    // event.first = Index of the first record
    // event.rows = Number of rows to display in new page
    // event.page = Index of the new page
    // event.pageCount = Total number of pages
    this.globalService.getSomethingWithPagination<PaginationResult<Contact>>('Contacts', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.contacts = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (let i = 0; i < this.contacts.length; i++) {
          this.contacts[i].DateActivated = this.contacts[i].DateActivated == null ? null :
            new Date(this.contacts[i].DateActivated).toLocaleDateString();
        }
      });
  }

  searchContact() {
    if (this.searchFilter.length !== 1) {
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
}


class PaginationResultClass implements PaginationResult<Contact> {
  constructor (public results, public pageNo, public recordPage, public totalRecords) {

  }
}

