import { Component, OnInit } from '@angular/core';
import { ContactService } from '../services/contact.service';
import { Contact } from '../domain/contact/contact';
import { ContactClass } from '../domain/contact/contact.class';

//validation
import { Message, SelectItem } from 'primeng/components/common/api';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { GenericService } from '../services/common/generic.service';
import { Pagination } from '../domain/common/pagination';

import { invoke } from 'q';

import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { AuthService } from '../services/common/Authentication/auth.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css'],
  providers: [ContactService, GenericService]

})

export class ContactComponent implements OnInit {

  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;
  isNewContact: boolean;

  // pagination
  entity: string = "Contacts";
  searchQuery: string = "";
  pageNumber: number = 0;
  rowCount: number = 5;
  totalRecords: number;
  @ViewChild('dt') public dataTable: DataTable;


  indexSelected: any; // for editing
  clonedSelectedContact: Contact; // for editing

  //validations
  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;


  constructor(
    private contactService: ContactService,
    private genericService: GenericService,
    private fb: FormBuilder,
    public authService: AuthService
  ) { }
  ngOnInit() {
    //validations
    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.minLength(11)])),
      'emailAddress': new FormControl('', Validators.required),

      'streetAddress': new FormControl('', Validators.required),
      'cityAddress': new FormControl('', Validators.required),
      'zipCode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),

    });

  }

  formSubmitted(message): void {
    this.submitted = true;
    this.msgs = [];
    this.msgs.push({ severity: 'info', summary: 'Success', detail: message });
  }

  clearContent(tmpContactList) {
    this.contactList = tmpContactList;
    //this.selectedContact = null;
    // this.clonedSelectedContact = null;
    this.clonedSelectedContact = new ContactClass();
    this.userform.markAsPristine();
    this.isNewContact = false;
  }

  // v2
  cancelForEdit: boolean = true;
  displayModal: boolean = false;

  showAddAndNew: boolean = false;
  showAdd: boolean = false;
  showDelete: boolean = false;

  disableForm: boolean = false;

  checkIfEmpty = (event) => {
    console.log(event);
    if (event == "") {
      this.search();
    }
  }

  search = () => {

    if(this.searchQuery.length >= 2) {
      this.dataTable.reset();
    } 

    if(this.searchQuery.length == 0){
      this.dataTable.reset();
    }

    if (this.searchQuery.length == 1) {
      this.formSubmitted("Search key must be 2 or more characters!");
    }
  }


  hideModalBtn = () => {
    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;
  }


  add = () => {
    this.cancelForEdit = false;
    // this.toggleForm(false);
    this.userform.markAsPristine();
    this.userform.enable();
    this.isNewContact = true;
    this.clonedSelectedContact = new ContactClass();

    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  cancel = () => {
    if (this.cancelForEdit) {

      if(JSON.stringify(this.clonedSelectedContact) === JSON.stringify(this.selectedContact)) {
        this.isNewContact = false;
        this.clonedSelectedContact = new ContactClass();
        this.userform.markAsPristine();
        this.displayModal = false;
        this.hideModalBtn();
      } else {
        this.clonedSelectedContact = JSON.parse(JSON.stringify(this.selectedContact));
      }
      // this.hideModalBtn();
    }
    else {
      this.isNewContact = false;
      this.clonedSelectedContact = new ContactClass();
      this.userform.markAsPristine();
      this.displayModal = false;
      this.hideModalBtn();
    }
  }

  save = (invoker) => {
    let tmpContactList = [...this.contactList];
    if (this.isNewContact) {

      this.genericService.Create<Contact>(this.entity, this.clonedSelectedContact)
        .then(data => {
          tmpContactList.push(data);
          this.clearContent(tmpContactList);
          // alert('Success!');
          this.formSubmitted("Contact Details Submitted!");

          this.displayModal = invoker == "Save" ? false : true;
          this.isNewContact = invoker == "Save" ? false : true;
          this.dataTable.reset();
        });

    } else {
      this.genericService.Update<Contact>(this.entity, this.clonedSelectedContact.contactId, this.clonedSelectedContact)
        .then(data => {
          tmpContactList[this.indexSelected] = this.clonedSelectedContact;
          this.clearContent(tmpContactList);
          // alert('Success!');
          this.formSubmitted("Updated Contact Details!");
          this.displayModal = false;
          this.hideModalBtn();
          this.dataTable.reset();
        });
    }
  }

  edit = (rowData) => {
    this.cancelForEdit = true;

    // this.toggleForm(false);
    this.userform.enable();
    this.selectedContact = rowData;
    this.indexSelected = this.contactList.indexOf(this.selectedContact); // value will be the index used for editing
    this.clonedSelectedContact = JSON.parse(JSON.stringify(this.selectedContact)); // cloned value of selected
    this.isNewContact = false;

    this.showAddAndNew = false;
    this.showAdd = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  setdeletion = (rowData) => {
    // this.toggleForm(true, rowData);
    this.cancelForEdit = false;

    this.userform.disable();
    this.selectedContact = rowData;

    this.indexSelected = this.contactList.indexOf(this.selectedContact); // value will be the index used for editing

    this.clonedSelectedContact = JSON.parse(JSON.stringify(this.selectedContact)); // cloned value of selected
    this.isNewContact = false;

    this.showAddAndNew = false;
    this.showAdd = false;
    this.showDelete = true;

    this.displayModal = true;
  }

  delete = () => {
    if (this.indexSelected > -1) {
      let tmpContactList = [...this.contactList];

      this.genericService
        .Delete(this.entity, this.clonedSelectedContact.contactId)
        .then(res => {
          console.log(res);
          if (res === 204) {
            tmpContactList.splice(this.indexSelected, 1);
            this.contactList = tmpContactList;
            this.clonedSelectedContact = null;
            this.indexSelected = -1;
            // alert('Success!');
            this.displayModal = false;
            // this.toggleForm(false, new ContactClass);
            this.disableForm = false;
            this.hideModalBtn();
            this.userform.markAsPristine();
            this.formSubmitted("Contact Deleted!");
            this.dataTable.reset();
          } else {
            alert("Failed to Delete!");
          }
        });

    }
  }

  paginate = (event) => {
    this.genericService.Retrieve<Pagination<Contact>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.contactList = result.result;
        this.totalRecords = result.totalCount;
      });
  }
}
