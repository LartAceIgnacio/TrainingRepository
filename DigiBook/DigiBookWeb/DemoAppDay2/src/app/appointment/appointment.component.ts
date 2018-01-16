import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Appointment } from '../domain/appointment/appointment';
import { AppointmentClass } from '../domain/appointment/appointment.class';

import { ContactService } from '../services/contact.service';
import { Contact } from '../domain/contact/contact';
import { ContactClass } from '../domain/contact/contact.class';

import { EmployeeService } from '../services/employee.service';
import { Employee } from '../domain/employee/employee';
import { EmployeeClass } from '../domain/employee/employee.class';

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
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css'],
  providers: [ContactService, EmployeeService, GenericService]
})

export class AppointmentComponent implements OnInit {

  contactList: Contact[];
  employeeList: Employee[];

  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;
  entity: string = "Appointments";

  // pagination
  searchQuery: string = "";
  pageNumber: number = 0;
  rowCount: number = 5;
  totalRecords: number;
  @ViewChild('dt') public dataTable: DataTable;


  indexSelected: any; // for editing
  clonedSelectedAppointment: Appointment; // for editing
  finalCloneSelectedAppointment: Appointment; // for editing

  //validations
  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  //genders: SelectItem[];
  constructor(

    private genericService: GenericService,
    private http: HttpClient,
    private contactService: ContactService,
    private employeeService: EmployeeService,
    private fb: FormBuilder,
    public authService: AuthService
  ) { }

  ngOnInit() {
    //validations
    this.userform = this.fb.group({
      'appointmentDate': new FormControl('', Validators.required),
      'hostName': new FormControl('', Validators.required),
      'guestName': new FormControl('', Validators.required),
      'startTime': new FormControl('', Validators.required),
      'endTime': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required),
    });
  }


  getContact() {
    this.contactService.getContacts()
      .then(data => {
        console.log(data);
        this.contactList = data;
      });
  }

  getContactName(guid): any {
    let result: string;
    this.contactList.forEach((element) => {
      if (element.contactId == guid) {
        console.log(element.firstName + ' ' + element.lastName);
        result = element.firstName + ' ' + element.lastName;
      };
    });
    return result;
  }

  getEmployeeName(guid): any {
    let result: string;
    this.employeeList.forEach((element) => {
      if (element.employeeId == guid) {
        console.log(element.firstName + ' ' + element.lastName);
        result = element.firstName + ' ' + element.lastName;
      };
    });
    return result;

  }

  getEmployee(entity, pageNumber, rowRecord, key) {
    this.employeeService.getEmployees()
      .then(data => {
        console.log(data);
        this.employeeList = data;
        this.genericService.Retrieve<Pagination<Appointment>>(entity, pageNumber, rowRecord, key)
          .then(result => {
            for (var i = 0; i < result.result.length; i++) {

              result.result[i].guestName = this.getContactName(result.result[i].guestId);
              result.result[i].hostName = this.getEmployeeName(result.result[i].hostId);
              // data[i].startTime = new Date(data[i].startTime);
              // data[i].endTime = new Date
              console.log(result.result[i].appointmentDate);

              result.result[i].appointmentDate = new Date(result.result[i].appointmentDate).toLocaleDateString();
            }
            console.log(result);
            this.appointmentList = result.result;
            this.totalRecords = result.totalCount;
          });
      });
  }



  addAppointment() {
    this.isNewAppointment = true;
    //this.selectedAppointment = new AppointmentClass();
    this.clonedSelectedAppointment = new AppointmentClass();
    this.clonedSelectedAppointment.guestId = this.contactList[0];
    this.clonedSelectedAppointment.hostId = this.employeeList[0];

  }

  saveAppointment() {
    let tmpAppointmentList = [...this.appointmentList];

    this.finalCloneSelectedAppointment = JSON.parse(JSON.stringify(this.clonedSelectedAppointment));
    
    this.finalCloneSelectedAppointment.guestName = this.clonedSelectedAppointment.guestId["firstName"] + ' ' + this.clonedSelectedAppointment.guestId["lastName"];
    this.finalCloneSelectedAppointment.hostName = this.clonedSelectedAppointment.hostId["firstName"] + ' ' + this.clonedSelectedAppointment.hostId["lastName"];
   
    this.finalCloneSelectedAppointment.guestId =  this.clonedSelectedAppointment.guestId["contactId"];
    this.finalCloneSelectedAppointment.hostId =  this.clonedSelectedAppointment.hostId["employeeId"];

    this.clonedSelectedAppointment.guestId = this.clonedSelectedAppointment.guestId["contactId"];
    this.clonedSelectedAppointment.hostId = this.clonedSelectedAppointment.hostId["employeeId"];

    console.log(this.finalCloneSelectedAppointment.guestId);
    console.log(this.finalCloneSelectedAppointment.hostId);
    console.log(this.clonedSelectedAppointment);
    if (this.isNewAppointment) {
      this.genericService.Create<Appointment>(this.entity, this.clonedSelectedAppointment)
        .then(data => {
          this.finalCloneSelectedAppointment.appointmentId = data.appointmentId;
          this.finalCloneSelectedAppointment.isDone = data.isDone;
          this.finalCloneSelectedAppointment.appointmentDate = new Date(data.appointmentDate).toLocaleDateString();
          tmpAppointmentList.push(this.finalCloneSelectedAppointment);
          this.clearContent(tmpAppointmentList);
          // alert('Success!');
          this.formSubmitted("Appointment Details Submitted!");
        });

    } else {
      this.genericService.Update<Appointment>(this.entity, this.clonedSelectedAppointment.appointmentId, this.clonedSelectedAppointment)
        .then(data => {
          tmpAppointmentList[this.indexSelected] = this.finalCloneSelectedAppointment;
          this.clearContent(tmpAppointmentList);
          // alert('Success!');
          this.formSubmitted("Updated Appointment Details!");

        });
    }

  }

  deleteAppointment() {
    if (this.indexSelected > -1) {
      let tmpAppointmentList = [...this.appointmentList];

      this.genericService
        .Delete<Appointment>(this.entity, this.clonedSelectedAppointment.appointmentId)
        .then(res => {
          console.log(res);
          if (res === 204) {   
            tmpAppointmentList.splice(this.indexSelected, 1);
            this.appointmentList = tmpAppointmentList;
            this.clonedSelectedAppointment = null;
            this.indexSelected = -1;
            // alert('Success!');
            this.formSubmitted("Deleted Appointment Details!");
          } else {
            alert("Failed to Delete!");
          }
        });

    }
  }
  cancelProcess() {
    this.isNewAppointment = false;
    this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment));
    // this.indexSelected = null;

  }

  onRowSelect() {
    console.log(this.selectedAppointment);
    this.indexSelected = this.appointmentList.indexOf(this.selectedAppointment); // value will be the index used for editing

    this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment)); // cloned value of selected
    
    this.clonedSelectedAppointment.guestId = this.contactList.find(x => x.contactId == this.clonedSelectedAppointment.guestId);
    this.clonedSelectedAppointment.hostId = this.employeeList.find(x => x.employeeId == this.clonedSelectedAppointment.hostId);
    
    console.log(this.clonedSelectedAppointment);
    this.isNewAppointment = false;
  }



  // v2

  formSubmitted(message): void {
    this.submitted = true;
    this.msgs = [];
    this.msgs.push({ severity: 'info', summary: 'Success', detail: message });
  }

  clearContent(tmpAppointmentList) {
    this.appointmentList = tmpAppointmentList;
    //this.selectedAppointment = null;
    // this.clonedSelectedAppointment = null;
    this.clonedSelectedAppointment = new AppointmentClass();
    this.userform.markAsPristine();
    this.isNewAppointment = false;
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
    this.isNewAppointment = true;
    this.clonedSelectedAppointment = new AppointmentClass();


    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;

    this.displayModal = true;

    this.clonedSelectedAppointment.guestId = this.contactList[0];
    this.clonedSelectedAppointment.hostId = this.employeeList[0];
    //this.selectedAppointment = new AppointmentClass();
    
  }

  cancel = () => {
    if (this.cancelForEdit) {

      if(JSON.stringify(this.clonedSelectedAppointment) === JSON.stringify(this.selectedAppointment)) {
        this.isNewAppointment = false;
        this.clonedSelectedAppointment = new AppointmentClass();
        this.userform.markAsPristine();
        this.displayModal = false;
        this.hideModalBtn();
      } else {
        this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment));
      }
      // this.hideModalBtn();
    }
    else {
      this.isNewAppointment = false;
      this.clonedSelectedAppointment = new AppointmentClass();
      this.userform.markAsPristine();
      this.displayModal = false;
      this.hideModalBtn();
    }
  }

  save = (invoker) => {
    
    let tmpAppointmentList = [...this.appointmentList];

    this.finalCloneSelectedAppointment = JSON.parse(JSON.stringify(this.clonedSelectedAppointment));
    
    this.finalCloneSelectedAppointment.guestName = this.clonedSelectedAppointment.guestId["firstName"] + ' ' + this.clonedSelectedAppointment.guestId["lastName"];
    this.finalCloneSelectedAppointment.hostName = this.clonedSelectedAppointment.hostId["firstName"] + ' ' + this.clonedSelectedAppointment.hostId["lastName"];
   
    this.finalCloneSelectedAppointment.guestId =  this.clonedSelectedAppointment.guestId["contactId"];
    this.finalCloneSelectedAppointment.hostId =  this.clonedSelectedAppointment.hostId["employeeId"];

    this.clonedSelectedAppointment.guestId = this.clonedSelectedAppointment.guestId["contactId"];
    this.clonedSelectedAppointment.hostId = this.clonedSelectedAppointment.hostId["employeeId"];

    console.log(this.finalCloneSelectedAppointment.guestId);
    console.log(this.finalCloneSelectedAppointment.hostId);
    console.log(this.clonedSelectedAppointment);
    if (this.isNewAppointment) {
      this.genericService.Create<Appointment>(this.entity, this.clonedSelectedAppointment)
        .then(data => {
          this.finalCloneSelectedAppointment.appointmentId = data.appointmentId;
          this.finalCloneSelectedAppointment.isDone = data.isDone;
          this.finalCloneSelectedAppointment.appointmentDate = new Date(data.appointmentDate).toLocaleDateString();
          tmpAppointmentList.push(this.finalCloneSelectedAppointment);
          this.clearContent(tmpAppointmentList);
          // alert('Success!');
          this.formSubmitted("Appointment Details Submitted!");
          this.displayModal = invoker == "Save" ? false : true;
          this.isNewAppointment = invoker == "Save" ? false : true;
          this.dataTable.reset();
        });

    } else {
      this.genericService.Update<Appointment>(this.entity, this.clonedSelectedAppointment.appointmentId, this.clonedSelectedAppointment)
        .then(data => {
          tmpAppointmentList[this.indexSelected] = this.finalCloneSelectedAppointment;
          this.clearContent(tmpAppointmentList);
          // alert('Success!');
          this.formSubmitted("Updated Appointment Details!");
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
    this.selectedAppointment = rowData;
    
    console.log(this.selectedAppointment);
    this.indexSelected = this.appointmentList.indexOf(this.selectedAppointment); // value will be the index used for editing

    this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment)); // cloned value of selected
    
    this.clonedSelectedAppointment.guestId = this.contactList.find(x => x.contactId == this.clonedSelectedAppointment.guestId);
    this.clonedSelectedAppointment.hostId = this.employeeList.find(x => x.employeeId == this.clonedSelectedAppointment.hostId);
    
    console.log(this.clonedSelectedAppointment);
    this.isNewAppointment = false;

    this.showAddAndNew = false;
    this.showAdd = true;
    this.showDelete = false;
    this.displayModal = true;

  }

  setdeletion = (rowData) => {
    // this.toggleForm(true, rowData);
    this.cancelForEdit = false;

    this.userform.disable();
    this.selectedAppointment = rowData;

    console.log(this.selectedAppointment);
    this.indexSelected = this.appointmentList.indexOf(this.selectedAppointment); // value will be the index used for editing

    this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment)); // cloned value of selected
    
    this.clonedSelectedAppointment.guestId = this.contactList.find(x => x.contactId == this.clonedSelectedAppointment.guestId);
    this.clonedSelectedAppointment.hostId = this.employeeList.find(x => x.employeeId == this.clonedSelectedAppointment.hostId);

    this.showAddAndNew = false;
    this.showAdd = false;
    this.showDelete = true;

    this.displayModal = true;
  }

  delete = () => {
    if (this.indexSelected > -1) {
      let tmpAppointmentList = [...this.appointmentList];

      this.genericService
        .Delete(this.entity, this.clonedSelectedAppointment.appointmentId)
        .then(res => {
          console.log(res);
          if (res === 204) {
            tmpAppointmentList.splice(this.indexSelected, 1);
            this.appointmentList = tmpAppointmentList;
            this.clonedSelectedAppointment = null;
            this.indexSelected = -1;
            // alert('Success!');
            this.displayModal = false;
            // this.toggleForm(false, new AppointmentClass);
            this.disableForm = false;
            this.hideModalBtn();
            this.userform.markAsPristine();
            this.formSubmitted("Appointment Deleted!");
            this.dataTable.reset();
          } else {
            alert("Failed to Delete!");
          }
        });
    }
  }

  paginate = (event) => {
    this.getContact();
    this.getEmployee(this.entity, event.first, event.rows, this.searchQuery);
  }
}
