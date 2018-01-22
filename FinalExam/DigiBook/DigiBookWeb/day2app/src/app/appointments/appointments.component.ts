import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointmentservice';
import { Appointment } from '../domain/appointments/appointment';
import { AppointmentClass } from '../domain/appointments/appointmentclass';


import { ContactService } from '../services/contactservice';
import { Contact } from '../domain/contacts/contact';
import { ContactClass } from '../domain/contacts/contactclass';

import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { Message } from "primeng/components/common/api";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { ConfirmationService, MenuItem } from "primeng/primeng";

import { GlobalService } from '../services/globalservice';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [AppointmentService, ContactService, EmployeeService, ConfirmationService, GlobalService]
})
export class AppointmentsComponent implements OnInit {
  displayDialog: boolean;
  dialogName: string;
  confirmHeader: string;
  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;
  items: MenuItem[];
  home: MenuItem;
  selectedAppointment: Appointment;
  selectedHost: Employee;
  selectedGuest: Contact;

  appointmentList: Appointment[];
  isNewAppointment: boolean;
  contactList: Contact[];
  employeeList: Employee[];
  date3: Date;
  cloneAppointment: Appointment;
  constructor(private appointmentService: AppointmentService
    , private contactService: ContactService
    , private employeeService: EmployeeService
    , private fb: FormBuilder
    , private confirmationService: ConfirmationService
    , private globalService: GlobalService
    , public auth: AuthService) { }

  ngOnInit() {
    this.contactService.getContacts().then(contacts => {
      this.contactList = contacts
      this.employeeService.getEmployees().then(employees => {
        this.employeeList = employees
        this.appointmentService.getAppointments().then(appointments => {
          this.appointmentList = appointments
          for (let i = 0; i < this.appointmentList.length; i++) {
            this.appointmentList[i].guestName = this.contactList.find(id => id.contactId == this.appointmentList[i].guestId).firstName
              + " " + this.contactList.find(id => id.contactId == this.appointmentList[i].guestId).lastName;
            this.appointmentList[i].hostName = this.employeeList.find(id => id.employeeId == this.appointmentList[i].hostId).firstName
              + " " + this.employeeList.find(id => id.employeeId == this.appointmentList[i].hostId).lastName;

            this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
          }
        });
      });
    });

    this.userform = this.fb.group({
      'host': new FormControl('', Validators.required),
      'guest': new FormControl('', Validators.required),
      'date': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required),
      'startTime': new FormControl('', Validators.required),
      'endTime': new FormControl('', Validators.required),
    });

    this.items = [
      { label: "Appointments", icon:"fa fa-calendar" }
    ]
    this.home = {icon: 'fa fa-home' ,  routerLink: "/dashboard" };

  }

  addAppointment() {
    this.isNewAppointment = true;
    this.selectedGuest = null;
    this.selectedHost = null;
    this.selectedAppointment = new AppointmentClass();
    this.dialogName = "Add Appointment"
    this.displayDialog = true;
  }

  SaveAndNewAppointment() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    if (this.isNewAppointment) {
      this.globalService.addSomething<Appointment>("Appointments",this.selectedAppointment).then(emp => {
        emp.hostName = this.employeeList.find(c => c.employeeId == emp.hostId).firstName + ' ' +
          this.employeeList.find(c => c.employeeId == emp.hostId).lastName;
        emp.guestName = this.contactList.find(e => e.contactId == emp.guestId).firstName + ' ' +
          this.contactList.find(e => e.contactId == emp.guestId).lastName;
        tmpAppointmentList.push(emp);
        this.selectedGuest = null;
        this.selectedHost = null;

        this.selectedAppointment = new AppointmentClass();
        this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
        this.appointmentList = tmpAppointmentList;
      });
    }

    this.userform.markAsPristine();
    this.displayDialog = true;
  }

  SaveAppointment() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    if (this.isNewAppointment) {
      this.globalService.addSomething<Appointment>("Appointments",this.selectedAppointment).then(emp => {
        emp.hostName = this.employeeList.find(c => c.employeeId == emp.hostId).firstName + ' ' +
          this.employeeList.find(c => c.employeeId == emp.hostId).lastName;
        emp.guestName = this.contactList.find(e => e.contactId == emp.guestId).firstName + ' ' +
          this.contactList.find(e => e.contactId == emp.guestId).lastName;
        tmpAppointmentList.push(emp);
        this.appointmentList = tmpAppointmentList;
        this.selectedAppointment = null;

        this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
      });
    }
    else {
      this.globalService.updateSomething<Appointment>("Appointments",this.selectedAppointment.appointmentId, this.selectedAppointment)
        .then(contact => {
          tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)]
            = this.selectedAppointment;
          this.appointmentList = tmpAppointmentList;
          this.selectedAppointment = null;

          this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully updated.' }];
        });
    }

    this.userform.markAsPristine();
    this.displayDialog = false;
    this.isNewAppointment = false;
  }

  editAppointment(app: Appointment) {
    this.isNewAppointment = false;
    this.selectedAppointment = app;
    this.onRowSelect();
    this.dialogName = "Edit Appointment"
    this.displayDialog = true;
  }

  closeDisplayNew() {
    this.displayDialog = false;
    this.isNewAppointment = false;
    this.selectedAppointment = null;
  }

  confirmCancel() {
    this.isNewAppointment = false;
    let tmpAppointmentList = [...this.appointmentList];
    tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
    this.appointmentList = tmpAppointmentList;
    this.selectedAppointment = Object.assign({}, this.cloneAppointment);
    this.selectedAppointment = new AppointmentClass();
    this.displayDialog = false;
    this.userform.markAsPristine();
  }

  cancelAppointment() {
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


  deleteAppointment(selectedApp: Appointment) {
    this.confirmHeader = "Delete Confirmation"
    this.selectedAppointment = selectedApp;
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        if (this.selectedAppointment == null) {
          this.selectedAppointment = new AppointmentClass();
        } else {
          this.globalService.deleteSomething<Appointment>("Appointments",selectedApp.appointmentId);
          let index = this.appointmentList.indexOf(this.selectedAppointment);
          this.appointmentList = this.appointmentList.filter((val, i) => i != index);
          this.selectedAppointment = null;
          this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully deleted.' }];
        }
      }
    });
  }

  onRowSelect() {
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.selectedGuest = this.contactList.find(x => x.contactId === this.selectedAppointment.guestId);
    this.selectedHost = this.employeeList.find(x => x.employeeId === this.selectedAppointment.hostId);
    this.isNewAppointment = false;
  }

  cloneRecord(r: Appointment): Appointment {
    let appointment = new AppointmentClass();
    for (let prop in r) {
      appointment[prop] = r[prop];
    }
    return appointment;
  }

}
