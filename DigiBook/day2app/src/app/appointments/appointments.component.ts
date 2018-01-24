import { Component, OnInit, ViewChild } from '@angular/core';
import { Appointment } from '../domain/appointment';
import { AppointmentService } from '../services/appointmentservice';
import { AppointmentClass } from '../domain/appointmentclass';

import { Contact } from '../domain/contact';
import { ContactService } from '../services/contactservice';
import { ContactClass } from '../domain/contactclass';

import { Employee } from '../domain/employee';
import { EmployeeService } from '../services/employeeservice';
import { EmployeeClass } from '../domain/employeeclass';

import { MenuItem } from 'primeng/primeng';
import { Message, SelectItem } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { GlobalService } from "../services/globalservice";
import { Pagination } from "../domain/pagination";
import { DataTable } from "primeng/components/datatable/datatable";
import { AuthService } from "../services/auth.service";

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [GlobalService, ContactService, EmployeeService, ConfirmationService]
})
export class AppointmentsComponent implements OnInit {

  appointment: Appointment = new AppointmentClass();
  appointmentList: Appointment[];
  contactList: Contact[];
  employeeList: Employee[];

  selectedHost: Employee;
  selectedGuest: Contact;

  indexSelected: number;

  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;
  isCancel: object[];
  isDone: object[];
  selectedDone: string;
  selectedCancel: string;
  selectHost: string;
  selectGuest: string;
  dateActivated: any;

  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  description: string;

  items: MenuItem[];

  home: MenuItem;

  display: boolean;

  constructor(private globalService: GlobalService, private contactservice: ContactService,
    private employeeservice: EmployeeService, private fb: FormBuilder, private conf: ConfirmationService
  ,public auth: AuthService) {
  }

  newAppointment() {
    this.display = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedGuest = null;
    this.selectedHost = null;
    this.isNewAppointment = true;
  }

  editAppointment(appointment: Appointment) {
    this.display = true;
    this.selectedAppointment = appointment;
    this.selectedGuest = this.contactList.find(x => x.contactId = this.selectedAppointment.guestId);
    this.selectedHost = this.employeeList.find(x => x.employeeId = this.selectedAppointment.hostId);
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();
    this.isNewAppointment = false;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
  }

  deleteAppoint(appointment : Appointment)
  {
    this.selectedAppointment = appointment;
    this.conf.confirm({
      message: 'Are you sure that you want to delete this data?',
      accept: () => {
        if (this.selectedAppointment.appointmentId == null)
          this.selectedAppointment = new AppointmentClass();
        else {
          this.globalService.deleteSomething("appointments",this.selectedAppointment.appointmentId)
          let index = this.appointmentList.indexOf(this.selectedAppointment);
          this.appointmentList = this.appointmentList.filter((val, i) => i != index);
          this.appointment = null;
        }
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Employee Deleted!' });
        this.selectedAppointment = new AppointmentClass();
      }
    });

  }

  saveAndNewAppointment() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    this.selectedAppointment.isCancelled = this.selectedCancel == 'False' ? false : true;
    this.selectedAppointment.isDone = this.selectedDone == 'No' ? false : true;

    
      this.globalService.addSomething("appointments",this.selectedAppointment).then(emp => {
        emp.hostName = this.employeeList.find(c => c.employeeId == emp.hostId).firstName +
          this.employeeList.find(c => c.employeeId == emp.hostId).lastName;
        emp.guestName = this.contactList.find(e => e.contactId == emp.guestId).firstName +
          this.contactList.find(e => e.contactId == emp.guestId).lastName;
        tmpAppointmentList.push(emp);
        this.appointmentList = tmpAppointmentList;
        this.selectedAppointment = new AppointmentClass();
      });

      this.userform.markAsPristine();
      this.msgs = [];
      this.msgs.push({ severity: 'success', summary: 'Appointment Saved!' });
  }

  ngOnInit() {
    this.userform = this.fb.group({
      'hostName': new FormControl('', Validators.required),
      'guestName': new FormControl('', Validators.required),
      'startTime': new FormControl('', Validators.required),
      'endTime': new FormControl('', Validators.required),
      'date': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required),
      'cancel': new FormControl(''),
      'done': new FormControl('')
    });

    this.isCancel = [];
    this.isCancel.push({ label: 'True', value: 'true' });
    this.isCancel.push({ label: 'False', value: 'false' });

    this.isDone = [];
    this.isDone.push({ label: 'Yes', value: 'true' });
    this.isDone.push({ label: 'No', value: 'false' });

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Appointment', routerLink: ['/appointments'] }
    ];
    this.home = { icon: 'fa fa-home' };
  }

  addAppointment() {
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedCancel = 'False';
    this.selectedDone = 'No';
    this.selectedGuest = new ContactClass();
    this.selectedHost = new EmployeeClass();
  }

  saveAppointment() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    this.selectedAppointment.isCancelled = this.selectedCancel == 'False' ? false : true;
    this.selectedAppointment.isDone = this.selectedDone == 'No' ? false : true;

    if (this.isNewAppointment) {
      this.globalService.addSomething("appointments",this.selectedAppointment).then(emp => {
        emp.hostName = this.employeeList.find(c => c.employeeId == emp.hostId).firstName +
          this.employeeList.find(c => c.employeeId == emp.hostId).lastName;
        emp.guestName = this.contactList.find(e => e.contactId == emp.guestId).firstName +
          this.contactList.find(e => e.contactId == emp.guestId).lastName;
        tmpAppointmentList.push(emp);
        this.appointmentList = tmpAppointmentList;
        this.selectedAppointment = null;
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Appointment Added!' });
      });
    }

    else {
      this.globalService.updateSomething("appointments",this.selectedAppointment.appointmentId,this.selectedAppointment)
      tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.selectedAppointment;
      this.msgs = [];
      this.msgs.push({ severity: 'success', summary: 'Appointment Saved!' });
    }
    this.userform.markAsPristine();
    this.isNewAppointment = false;

    this.display = false;
  }

  onRowSelect() {
    this.isNewAppointment = false;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);

    this.selectedGuest = this.contactList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedHost = this.employeeList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate);

    this.selectedCancel = this.selectedAppointment.isCancelled + "";
    this.selectedDone = this.selectedAppointment.isDone + "";
  }

  cloneRecord(r: Appointment): Appointment {
    let appointment = new AppointmentClass();
    for (let prop in r) {
      appointment[prop] = r[prop];
    }
    return appointment;
  }

  cancelAppointment() {
    this.isNewAppointment = false;
    let tmpAppointmentList = [...this.appointmentList];
    tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
    this.appointmentList = tmpAppointmentList;
    this.selectedAppointment = this.cloneAppointment;
    this.selectedAppointment = new AppointmentClass();
    this.display = false;
    this.userform.markAsPristine();
  }

  cancelAppoint() {
    if (this.isNewAppointment == true) {
      this.display = false;
    }
    else {
      if (this.userform.dirty) {
        this.conf.confirm({
          message: 'Do you want to discard changes?',
          accept: () => {
            this.cancelAppointment()
          }
        });
      }
      else {
        this.display = false;
      }
    }
    this.userform.markAsPristine();
  }

  deleteAppointment() {
    if (this.selectedAppointment.appointmentId == null)
      this.selectedAppointment = new AppointmentClass();
    else {
      this.globalService.deleteSomething("appointments",this.selectedAppointment.appointmentId)
      let index = this.appointmentList.indexOf(this.selectedAppointment);
      this.appointmentList = this.appointmentList.filter((val, i) => i != index);
      this.appointment = null;
    }
    this.selectedAppointment = new AppointmentClass();
  }

  getNames()
  {
    this.contactservice.getContacts().then(contacts => {
      this.contactList = contacts
      this.employeeservice.getEmployees().then(employees => {
        this.employeeList = employees

          for (let i = 0; i < this.appointmentList.length; i++) {
            this.appointmentList[i].guestName = this.contactList.find(id => id.contactId == this.appointmentList[i].guestId).firstName
              + " " + this.contactList.find(id => id.contactId == this.appointmentList[i].guestId).lastName;
            this.appointmentList[i].hostName = this.employeeList.find(id => id.employeeId == this.appointmentList[i].hostId).firstName
              + " " + this.employeeList.find(id => id.employeeId == this.appointmentList[i].hostId).lastName;

              this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
          }
      });
    });

  }

  entity: string = "appointments";
  searchQuery: string = "";
  totalRecord: number;

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Appointment>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.appointmentList = result.result;
        this.getNames();
        this.totalRecord = result.totalCount;
      });
  }

  @ViewChild('dt') public dataTable: DataTable;
  search() {
    this.dataTable.reset();
  }
}
