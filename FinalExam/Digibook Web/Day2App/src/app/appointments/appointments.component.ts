import { Component, OnInit } from '@angular/core';
import { Appointment } from '../domain/appointment';
import { AppointmentClass } from '../domain/appointmentclass';
import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';
import { Employee } from '../domain/employee';
import { GlobalService } from '../services/globalservice';
import { EmployeeClass } from '../domain/employeeclass';
import { Message } from '../domain/message';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { PaginationResult } from "../domain/paginationresult";
import { AuthService } from "../services/authservice";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [GlobalService, ConfirmationService, DatePipe]
})

export class AppointmentsComponent implements OnInit {
  appointmentList: Appointment[];
  cloneAppointment: Appointment;
  selectedAppointment: Appointment;
  isNewAppointment: boolean;
  isDeleteAppointment: boolean = false;
  indexOfAppointment: number;
  isTrueFalse: object[];
  selectedCancelled: string;
  selectedDone: string;
  contactList: Contact[];
  employeeList: Employee[];
  selectedContact: Contact = new ContactClass();
  selectedEmployee: Employee = new EmployeeClass();
  ctr: number = 0;
  dateActivated: Date;
  minDate: Date = new Date();
  userform: FormGroup;
  searchFilter: string = "";
  //dateFilter: Date[];
  totalRecords: number = 0;
  paginationResult: PaginationResult<Appointment>;
  rexExpTimeFormat: string = "^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])(:[0-5][0-9])?$";

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService,
    private fb: FormBuilder, public auth: AuthService, private datePipe: DatePipe) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.isTrueFalse = [];
    this.isTrueFalse.push({ label: 'true', value: 'true' });
    this.isTrueFalse.push({ label: 'false', value: 'false' });

    this.userform = this.fb.group({
      'appoitmentDate': new FormControl('', Validators.required),
      'guestName': new FormControl('', Validators.required),
      'hostName': new FormControl('', Validators.required),
      'notes': new FormControl(''),
      'done': new FormControl(''),
      'cancelled': new FormControl(''),
      'startTime': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.rexExpTimeFormat)])),
      'endTime': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.rexExpTimeFormat)]))
    });
  }

  paginate(event) {
    if (this.ctr == 0) {
      this.globalService.getSomething<Contact>("Contacts").then(contacts => {
        this.contactList = contacts;
        this.globalService.getSomething<Employee>("Employees").then(employees => {
          this.employeeList = employees;
          this.globalService.getSomethingWithPagination<PaginationResult<Appointment>>("Appointments", event.first, event.rows,
            this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
              this.paginationResult = paginationResult;
              this.appointmentList = this.paginationResult.results;
              this.totalRecords = this.paginationResult.totalRecords;
              this.getFullName();
            });
        });
      });
    }
    else {
      this.globalService.getSomethingWithPagination<PaginationResult<Appointment>>("Appointments", event.first, event.rows,
        this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.appointmentList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
          this.getFullName();
        });
    }
  }

  searchAppointment() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
    // if (this.dateFilter != null && (this.dateFilter[0] != null || this.dateFilter[1] != null)) {
    //   this.searchFilter = this.dateFilter[0].toLocaleDateString() + "," + this.dateFilter[1].toLocaleDateString();
    //   if (this.searchFilter.length > 0) {
    //     this.searchFilter = this.searchFilter.replace(/\//g, "%2F");
    //   }
    // }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    // let paging = {
    //   first: ((n - 1) * this.dataTable.rows),
    //   rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }

  addAppointment() {
    this.userform.enable();
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedContact = new ContactClass();
    this.selectedEmployee = new EmployeeClass();
    this.selectedCancelled = "false";
    this.selectedDone = "false";
    this.dateActivated = null;
  }

  onRowSelect(appointment) {
    this.isNewAppointment = false;
    
    this.userform.enable();
    if(!this.auth.isLoggedIn()) {
      this.userform.controls['guestName'].disable();
    }

    this.selectedAppointment = appointment;
    this.indexOfAppointment = this.appointmentList.indexOf(this.selectedAppointment);

    this.selectedContact = this.contactList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.dateActivated = this.selectedAppointment.appointmentDate;

    this.cloneAppointment = Object.assign({}, this.selectedAppointment);
    this.selectedAppointment = Object.assign({}, this.selectedAppointment);

    this.selectedCancelled = this.selectedAppointment.isCancelled + "";
    this.selectedDone = this.selectedAppointment.isDone + "";
  }

  btnSave(isSaveAndNew: boolean) {
    this.userform.markAsPristine();
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.isCancelled = this.selectedCancelled == 'true' ? true : false;
    this.selectedAppointment.isDone = this.selectedDone == 'true' ? true : false;
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;
    this.selectedAppointment.appointmentDate = this.datePipe.transform(this.dateActivated, 'yyyy-MM-dd');;
    if (this.isNewAppointment) {
      this.globalService.addSomething<Appointment>("Appointments", this.selectedAppointment).then(appointments => {
        tmpAppointmentList.push(appointments);
        this.appointmentList = tmpAppointmentList;
        this.getFullName();
        this.selectedAppointment = isSaveAndNew ? new AppointmentClass() : null;
        this.isNewAppointment = isSaveAndNew ? true : false;
        this.selectedContact = new ContactClass();
        this.selectedEmployee = new EmployeeClass();
        this.selectedCancelled = "false";
        this.selectedDone = "false";
        this.dateActivated = null;
        if (!isSaveAndNew) {
          this.setCurrentPage(1);
        }
      });
    }
    else {
      this.globalService.updateSomething<Appointment>("Appointments", this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointments => {
        tmpAppointmentList[this.indexOfAppointment] = this.selectedAppointment;
        this.appointmentList = tmpAppointmentList;
        this.setCurrentPage(1);
        this.getFullName();
        this.selectedAppointment = null;
        this.isNewAppointment = false;
      });
    }
  }

  btnCancel() {
    if (this.isNewAppointment || this.isDeleteAppointment || !this.userform.dirty) {
      this.selectedAppointment = null;
      this.isDeleteAppointment = false;
      this.isNewAppointment = false
      this.userform.markAsPristine();
    }
    else if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to discard the changes?',
        header: 'Cancel Confirmation',
        icon: 'fa fa-ban',
        accept: () => {
          let tmpAppointmentList = [...this.appointmentList];
          tmpAppointmentList[this.indexOfAppointment] = this.cloneAppointment;
          this.appointmentList = tmpAppointmentList;
          this.selectedContact = this.contactList.find(x => x.contactId == this.cloneAppointment.guestId);
          this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.cloneAppointment.hostId);
          this.selectedAppointment = Object.assign({}, this.cloneAppointment);
          this.dateActivated = this.selectedAppointment.appointmentDate;
          this.userform.markAsPristine();
        }
      });
    }
  }

  btnDelete(appointment) {
    this.userform.disable();
    this.selectedAppointment = appointment;
    this.indexOfAppointment = this.appointmentList.indexOf(appointment);
    this.isDeleteAppointment = true;
  }

  btnOk() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let tmpAppointmentList = [...this.appointmentList];
        tmpAppointmentList.splice(this.indexOfAppointment, 1);
        this.globalService.deleteSomething<Appointment>("Appointments", this.selectedAppointment.appointmentId).then(appointments => {
          tmpAppointmentList.splice(this.indexOfAppointment, 1);
          this.appointmentList = tmpAppointmentList;
          this.selectedAppointment = null;
          this.isNewAppointment = false;
          this.isDeleteAppointment = false;
          this.setCurrentPage(1);
        });
      }
    });
  }

  getFullName() {
    if (this.ctr == 0) {
      for (var i = 0; i < this.contactList.length; i++) {
        this.contactList[i].fullName = this.contactList[i].firstName + " " + this.contactList[i].lastName;
      }

      for (var i = 0; i < this.employeeList.length; i++) {
        this.employeeList[i].fullName = this.employeeList[i].firstName + " " + this.employeeList[i].lastName;
      }
    }

    for (var i = 0; i < this.appointmentList.length; i++) {
      this.appointmentList[i].hostName = this.employeeList.find(x => x.employeeId == this.appointmentList[i].hostId).fullName;
      this.appointmentList[i].guestName = this.contactList.find(x => x.contactId == this.appointmentList[i].guestId).fullName;
      this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
    }

    this.ctr++;
  }
}

class PaginationResultClass implements PaginationResult<Appointment>{
  constructor(public results, public pageNo, public recordPage, public totalRecords) {

  }
}
