import { Component, OnInit } from '@angular/core';
import { ContactService } from '../services/contactservice';
import { Contact } from '../domain/contacts/contact';
import { ContactClass } from '../domain/contacts/contactclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { GlobalService } from '../services/globalservice';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';


@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [GlobalService, ConfirmationService, FormBuilder]
})
export class ContactsComponent implements OnInit {

  items: MenuItem[];
  home: MenuItem;
  contactList: Contact[];
  selectedContact: Contact;
  isNewContact: boolean;
  cloneContact: Contact;
  display: boolean;
  userform: FormGroup;
  isDelete: boolean;
  isEdit: boolean;
  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';
  paginationResult: PaginationResult<Contact>;
  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;


  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

    @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.items = [
      { label: 'Contacts' }
    ];
    this.home = { icon: 'fa fa-home', label: 'Home', routerLink: '/dashboard' };

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.compose([Validators.required, Validators.minLength(11)])),
      'streetaddress': new FormControl('', Validators.required),
      'cityaddress': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.compose([Validators.required, Validators.email]))
    });
  }

  showDialog() {
    this.isEdit = false;
    this.isDelete = false;
    this.userform.enable();
    this.userform.markAsPristine();
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
    this.display = true;
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Contact>>('Contacts', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.contactList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (let i = 0; i < this.contactList.length; i++) {
          this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null :
            new Date(this.contactList[i].dateActivated).toLocaleDateString();
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
  }

  editContact(contactToEdit: Contact) {
    this.isEdit = true;
    this.isDelete = false;
    this.userform.enable();
    this.isNewContact = false;
    this.selectedContact = this.cloneRecord(contactToEdit);
    this.cloneContact = contactToEdit;
    this.display = true;

  }

  deleteContact(contactToDelete: Contact) {
    this.isDelete = true;
    this.userform.disable();
    this.display = true;
    this.isNewContact = false;
    this.selectedContact = this.cloneRecord(contactToDelete);
    this.cloneContact = contactToDelete;
    this.userform.markAsPristine();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        this.globalService.deleteSomething<Contact>('Contacts', this.selectedContact.contactId);
        const index = this.contactList.indexOf(this.cloneContact);
        this.contactList = this.contactList.filter((val, i) => i !== index);
        this.selectedContact = null;
        this.isNewContact = false;
        this.display = false;
        this.setCurrentPage(1);
      }
    });
  }

  saveContact() {
    const tmpContactList = [...this.contactList];
    if (this.isNewContact) {
      this.globalService.postSomething<Contact>('Contacts', this.selectedContact)
        .then(contact => {
          tmpContactList.push(contact);
          this.contactList = tmpContactList;
          this.selectedContact = null;
          this.display = false;
          this.setCurrentPage(1);
        });
    } else {
      this.globalService.putSomething<Contact>('Contacts', this.selectedContact.contactId, this.selectedContact)
        .then(contact => {
          tmpContactList[this.contactList.indexOf(this.cloneContact)] = this.selectedContact;
          this.contactList = tmpContactList;
          this.selectedContact = null;
          this.display = false;
        });
    }
    this.isNewContact = false;
  }

  newSaveContact() {
    this.userform.markAsPristine();
    const tmpContactList = [...this.contactList];
    this.globalService.postSomething<Contact>('Contacts', this.selectedContact)
      .then(contact => {
        tmpContactList.push(contact);
        this.contactList = tmpContactList;
        this.selectedContact = new ContactClass;
        this.display = true;
      });
  }

  confirmCancel() {
    this.isNewContact = false;
    const tmpContactList = [...this.contactList];
    tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
    this.contactList = tmpContactList;
    this.selectedContact = Object.assign({}, this.cloneContact);
    this.selectedContact = new ContactClass();
    this.display = false;
    this.userform.markAsPristine();
  }

  cancelContact() {
    if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        accept: () => {
          this.confirmCancel();
        }
      });
    } else {
      this.display = false;
    }
  }

  onRowSelect() {
    this.isNewContact = false;
    this.cloneContact = this.cloneRecord(this.selectedContact);
  }

  cloneRecord(r: Contact): Contact {
    const contact = new ContactClass();
    // tslint:disable-next-line:forin
    for (const prop in r) {
      contact[prop] = r[prop];
    }
    return contact;
  }
}
