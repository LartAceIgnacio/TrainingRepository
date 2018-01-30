

import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Contact } from '../domain/contact';
import { Contactclass } from '../domain/contactclass';
import { SelectItem, ConfirmationService, LazyLoadEvent } from 'primeng/primeng';
import { Message } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MenuItem } from 'primeng/components/common/menuitem';
import { Pagination } from '../domain/pagination';
import { PaginationClass } from '../domain/paginationClass';

import { DataTable } from 'primeng/components/datatable/datatable';
import { GlobalService } from '../services/global.service';

@Component({
  selector: 'app-contacs',
  templateUrl: './contacs.component.html',
  styleUrls: ['./contacs.component.css'],
  providers:[GlobalService, ConfirmationService]
})
export class ContacsComponent implements OnInit {

  indexSelected: number;
  isNewContact: boolean;
  displayDialog: boolean;
  loading: boolean;
  delete:boolean ;
  submitted: boolean;

  serviceName: string = "Contacts";

  countryList: SelectItem[];
  clonedSelectedContact: Contact;
  contactList: Contact[];
  selectedContact: Contact;
  cloneContact: Contact;

  contact: Contact = new Contactclass();

  pagination: Pagination<Contact>;
  totalRecords: number = 0;
  searchFilter: string = "";

  msgs: Message[] = [];
  contactForm: FormGroup;
  breadcrumb: MenuItem[];

constructor(private globalService: GlobalService, private fb: FormBuilder, private confirmationService: ConfirmationService) {
}

@ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {

    this.contactForm = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.required),
      'streetaddress': new FormControl('', Validators.required),
      'cityaddress': new FormControl('', Validators.required),
      'zipcode': new FormControl('', Validators.required),
      'country': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.required),
    });

    this.breadcrumb = [
      {label:'Dashboard', routerLink:['/dashboard']},
      {label:'Contacts', routerLink:['/contacts']},
  ];
  }
  paginate(event) {
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
    this.globalService.getPagination<Pagination<Contact>>(this.serviceName, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
        this.pagination = pagination;
        this.contactList = this.pagination.results;
        this.totalRecords = this.pagination.totalRecords;
        // for (var i = 0; i < this.contactList.length; i++) {
        //   this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null :
        //     new Date(this.contactList[i].dateActivated).toLocaleDateString();
        // }
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
    this.isNewContact = true;
    this.selectedContact = new Contactclass;
    this.displayDialog = true;
    this.contactForm.enable();
    this.delete = false;
  }
  editContact(contacts : Contact){
    this.contactForm.enable();
    this.isNewContact = false;
    this.delete = false;
    this.selectedContact = contacts;
    this.cloneContact = this.cloneRecord(this.selectedContact);
    this.displayDialog = true;
  }

  saveContact() {
    let tmpContactList = [...this.contactList];
    if(this.isNewContact){
        this.globalService.addData<Contact>(this.serviceName,this.selectedContact).then(contacts => {
          tmpContactList.push(contacts);
          this.contactList = tmpContactList;
          this.selectedContact=null;
        });
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Success', detail:'Added Contact Details'});
    }else{
        this.globalService.updateData<Contact>(this.serviceName,this.selectedContact.contactId, this.selectedContact).then(contacts =>{
        tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.selectedContact;
          this.contactList=tmpContactList;
          this.selectedContact=null;
        });
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Contact Details'});

    }
    this.contactForm.markAsPristine();
    this.contactList = tmpContactList;
    this.selectedContact = null;
    this.displayDialog = false;

  }
  saveAndNewContact(){
    this.contactForm.markAsPristine();
    let tmpContactList = [...this.contactList];
    tmpContactList.push(this.selectedContact);

    if(this.isNewContact){
      this.globalService.addData<Contact>(this.serviceName,this.selectedContact);
      this.contactList=tmpContactList;
      this.isNewContact = true;
      this.selectedContact = new Contactclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Contact'});
    }
  }

  deleteContact(contacts : Contact){
     this.contactForm.disable();
     this.selectedContact = contacts;
     this.displayDialog = true;
     this.delete = true;
  }

  delContact(){
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
      let index = this.findSelectedContactIndex();
      this.contactList = this.contactList.filter((val,i) => i!=index);
      this.globalService.deleteData<Contact>(this.serviceName,this.selectedContact.contactId);
      this.submitted = true;
      this.msgs = [];
      this.msgs.push({severity:'error', summary:'Deleted', detail:'Deleted Contact Details'});
      this.selectedContact = null;
      this.displayDialog = false;
      }
    });
  }

  // onRowSelect(event) {
  //         this.isNewContact = false;
  //         this.selectedContact;
  //         this.cloneContact = this.cloneRecord(this.selectedContact);
  // }

  cloneRecord(r: Contact): Contact {
      let contact = new Contactclass();
      for(let prop in r) {
          contact[prop] = r[prop];
      }
      return contact;
  }

  cancelContact(){
    if(this.contactForm.dirty){
      this.confirmationService.confirm({
        message: 'Do you want to Discard this changes?',
        accept: () => {
          this.isNewContact = false;
          let tmpContactList = [...this.contactList];
          tmpContactList[this.contactList.indexOf(this.selectedContact)] = this.cloneContact;
          this.contactList = tmpContactList;
          this.selectedContact = this.cloneContact;
          this.selectedContact = null;

        }
      });
    }
  }

  findSelectedContactIndex(): number {
      return this.contactList.indexOf(this.selectedContact);
  }

}
