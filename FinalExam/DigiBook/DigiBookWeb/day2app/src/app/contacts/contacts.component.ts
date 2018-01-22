import { Component, OnInit } from '@angular/core';
import { ContactService } from '../services/contactservice';
import { Contact } from '../domain/contacts/contact';
import { ContactClass } from '../domain/contacts/contactclass';
import { Message } from "primeng/components/common/api";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";

import { ConfirmationService, MenuItem,DataTable } from 'primeng/primeng';
import { GlobalService } from '../services/globalservice';
import { PaginationResult } from '../domain/paginationresult';
import { Employee } from '../domain/employee';
import { ViewChild } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  providers: [ContactService, GlobalService, ConfirmationService]
})
export class ContactsComponent implements OnInit {
  items: MenuItem[];
  displayDialog: boolean;
  dialogName: string;
  confirmHeader: string;
  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  home: MenuItem;

  contactList: Contact[];
  selectedContact: Contact;
  isNewContact: boolean;
  cloneContact: Contact;

  paginationResult: PaginationResult<Employee>;

  totalRecords: number = 0;
  searchFilter: string = "";

  constructor(private contactService: ContactService,
    private fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private globalService: GlobalService,
    public auth: AuthService
  ) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    // this.globalService.getSomething<Contact>("Contacts")
    //   .then(contacts =>{ this.contactList = contacts; console.log(contacts)});

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobile': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.required),
      'street': new FormControl('', Validators.required),
      'city': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required)
    });

    this.items = [
      { label: "Contacts" }
    ]

    this.home = { icon: 'fa fa-home', routerLink: "/dashboard" };
  }


 paginate(event) {
  this.globalService.getSomethingWithPagination<PaginationResult<Contact>>("Contacts", event.first, event.rows,
    this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.contactList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalCount;
      });
}
setCurrentPage(n: number) {
  this.dataTable.reset();
  // let paging = {
  //   first: ((n - 1) * this.dataTable.rows),
  //   rows: this.dataTable.rows
  // };
  //this.dataTable.paginate();
}

searchContact() {
  if (this.searchFilter.length != 1) {
    this.setCurrentPage(1);
  }
}


  addContact() {
    this.isNewContact = true;
    this.selectedContact = new ContactClass();
    this.dialogName = "Add Contact"
    this.displayDialog = true;
  }
  editContact(con: Contact) {
    this.isNewContact = false;
    this.selectedContact = con;
    this.onRowSelect();
    this.dialogName = "Edit Contact"
    this.displayDialog = true;
  }
  deleteContact(selectedCon: Contact) {
    this.confirmHeader = "Delete Confirmation"
    this.selectedContact = selectedCon;
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        if (this.selectedContact == null) {
          this.selectedContact = new ContactClass();
        } else {
          this.globalService.deleteSomething<Contact>("Contacts",selectedCon.contactId);
          let index = this.contactList.indexOf(this.selectedContact);
          this.contactList = this.contactList.filter((val, i) => i != index);
          this.selectedContact = null;
          this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully deleted.' }];
        }
      }
    });
  }

  SaveAndNewContact() {
    let tmpContactList = [...this.contactList];
    this.globalService.addSomething<Contact>("Contacts",this.selectedContact).then(emp => {
      tmpContactList.push(this.selectedContact);
      this.contactList = tmpContactList;
      this.selectedContact = new ContactClass();

      this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
    });
    this.userform.markAsPristine();
    this.displayDialog = true;
  }

  SaveContact() {
    let tmpcontactList = [...this.contactList];
    if (this.isNewContact) {
      this.globalService.addSomething<Contact>("Contacts",this.selectedContact).then(con => {
        tmpcontactList.push(con);
        this.contactList = tmpcontactList;
        this.selectedContact = null;

        this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
      });
    }
    else {
      this.globalService.updateSomething<Contact>("Contacts",this.selectedContact.contactId, this.selectedContact);
      tmpcontactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
      this.contactList = tmpcontactList;
      this.selectedContact = null;

      this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully updated.' }];
    }
    this.userform.markAsPristine();
    this.displayDialog = false;
    this.isNewContact = false;
  }

  closeDisplayNew() {
    this.displayDialog = false;
    this.isNewContact = false;
    this.selectedContact = null;
  }

  confirmCancel() {
    this.isNewContact = false;
    let tmpcontactList = [...this.contactList];
    tmpcontactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
    this.contactList = tmpcontactList;
    this.selectedContact = Object.assign({}, this.cloneContact);
    this.selectedContact = new ContactClass();
    this.displayDialog = false;
    this.userform.markAsPristine();
  }

  cancelContact() {
    this.confirmHeader = "Discard Changes"
    if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        accept: () => {
          this.confirmCancel();
        }
      });
    }
    else {
      this.displayDialog = false;
    }
  }

  onRowSelect() {
    this.isNewContact = false;
    this.cloneContact = Object.assign({}, this.selectedContact);
  }

  cloneRecord(r: Contact): Contact {
    let contact = new ContactClass();
    for (let prop in r) {
      contact[prop] = r[prop];
    }
    return contact;
  }
}
class PaginationResultClass implements PaginationResult<Employee>{
  constructor(public results, public pageNo, public recordPage, public totalCount) {

  }
}