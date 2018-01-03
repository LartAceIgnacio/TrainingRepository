import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { AppointmentService } from '../services/appointments.service';
import { IAppointments } from '../domain/IAppointments';
import { AppointmentsClass } from '../domain/appointments.class';

import { ContactsService } from '../services/contacts.service';
import { IContacts } from '../domain/IContacts';
import { ContactsClass } from '../domain/contacts.class';

import { EmployeeService } from '../services/employee.service';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';

import * as moment from 'moment';//moment.js for time formatting
import { Time } from '@angular/common/src/i18n/locale_data_api';
import { VariableAst } from '@angular/compiler';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [AppointmentService, EmployeeService, ContactsService]
})
export class AppointmentsComponent implements OnInit {

  contactList: IContacts[];
  employeeList: Employee[];

  appointmentList: IAppointments[];
  selectedAppointment: IAppointments;
  clonedAppointment: IAppointments;
  isNewAppointment: boolean;
  globalIndex: number;
  startTime: Time;
  endTime: Time;


  constructor(private httpClient: HttpClient, private appointmentService: AppointmentService
    , private employeeService: EmployeeService, private contactService: ContactsService) { }

  appointmentDate: string;
  checked: boolean = true;

  ngOnInit() {
    // retrieves contacts from db.
    this.getHostandGuestNames();
  }

  getHostandGuestNames() {
    this.employeeService._getEmployees()
      .then(e => {
        this.employeeList = e;
        // employee names
        for(var i = 0; i < this.employeeList.length; i++)
        {
          this.employeeList[i].employeeFullname = this.employeeList[i].firstname + " " + this.employeeList[i].lastname;
        }

        //contact names service
        this.contactService._getContacts().
          then(c => { 
            this.contactList = c;

            for(var i = 0; i < this.contactList.length; i++)
            {
              this.contactList[i].contactFullName = this.contactList[i].firstname + " " + this.contactList[i].lastname;
            }


            this.appointmentService._getAppointments()
            .then(appointments => {
              this.appointmentList = appointments;
            
              for(var i = 0; i < this.appointmentList.length; i ++){
                this.appointmentList[i].guestName = this.contactList.find(f => f.contactId == this.appointmentList[i].guestId).contactFullName;
                this.appointmentList[i].hostName = this.employeeList.find(f => f.id == this.appointmentList[i].hostId).employeeFullname;
              }
            });


          });
        
        
      });
  }

  getContacts() {
    this.contactService._getContacts()
      .then(c => {
        this.contactList = c;
      });
  }

  getAppointments() {
    this.appointmentService._getAppointments()
      .then(appointments => {
        console.log("Get Appointments: " + JSON.stringify(appointments));
        this.appointmentList = appointments;
      });
  }

  onRowSelect() {
    this.isNewAppointment = false;
    //this.clonedAppointment = this.cloneRecord(this.selectedAppointment);
    this.globalIndex = this.appointmentList.indexOf(this.selectedAppointment);
    this.selectedAppointment = Object.assign({}, this.selectedAppointment);
    this.clonedAppointment = Object.assign({}, this.selectedAppointment);
    
  }

  addAppointment() {
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentsClass();
  }

  saveContact() {
    let tmpContactList = [...this.appointmentList];
    if (this.isNewAppointment) {
      this.appointmentService._addAppointment(this.selectedAppointment)
        .then(contacts => {
          tmpContactList.push(contacts["result"]);
          this.appointmentList = tmpContactList;
          this.selectedAppointment = null;
        });
    }
    else {
      this.appointmentService._updateAppointment(this.selectedAppointment)
        .then(contacts => {
          tmpContactList[this.globalIndex] = this.selectedAppointment;
          this.appointmentList = tmpContactList;
          this.selectedAppointment = null;
        });
    }
    this.isNewAppointment = false;

  }

  cancelContact() {
    if (this.isNewAppointment) {
      this.selectedAppointment = null;
    }
    else {
      this.isNewAppointment = false;
      let tmpContactList = [...this.appointmentList];
      tmpContactList[this.appointmentList.indexOf(this.selectedAppointment)] = this.clonedAppointment;
      this.appointmentList = tmpContactList;
      this.selectedAppointment = Object.assign({}, this.clonedAppointment); //null;
    }
  }

  deleteContact() {
    let index = this.globalIndex;
    this.appointmentService._deleteAppointment(this.selectedAppointment.appointmentId);

    this.appointmentList = this.appointmentList.filter((val, i) => i != index);
    this.selectedAppointment = null;
  }

}
