import { Component, OnInit } from '@angular/core';

import { Contact } from '../domain/contact';
import { ContactClass } from '../domain/contactclass';
import { ContactService } from '../services/contactservice';

import { Appointment } from '../domain/appointment';
import { AppointmentClass } from '../domain/appointmentclass';
import { AppointmentService } from '../services/appointmentservice';

import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { EmployeeService } from '../services/employeeService';

import { Message, SelectItem } from 'primeng/components/common/api';
import { ConfirmationService } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [ AppointmentService, ContactService, EmployeeService, ConfirmationService]
})
export class AppointmentsComponent implements OnInit {
  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  isNewAppointment: boolean;
  cloneAppointment: Appointment;
  appointment: Appointment;

  contactList: Contact[];
  selectedContact: Contact;

  employeeList: Employee[];
  selectedEmployee: Employee;

  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  constructor(private appointmentService: AppointmentService,
              private contactService: ContactService, private employeeService: EmployeeService, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.contactService.getContact(10, 10).then(contact =>{
      this.contactList = contact;
        this.employeeService.getEmployees().then(employee =>{
          this.employeeList = employee;
            this.appointmentService.getAppointment().then(appointment => {
              this.appointmentList = appointment;
                for(let i = 0; this.appointmentList.length > i; i++){
                  let tempEmployee;
                  let tempContact;
                  this.employeeList.forEach( x => {
                      if(x.employeeId == this.appointmentList[i].hostId)
                      {
                        tempEmployee = x;
                      }});
                   this.contactList.forEach( x => {
                      if(x.contactId == this.appointmentList[i].guestId)
                      {
                        tempContact= x;
                      }});
                  this.appointmentList[i].guestName = tempContact.firstName+' '+tempContact.lastName;
                  this.appointmentList[i].hostName = tempEmployee.firstName+' '+tempEmployee.lastName;

                  this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
                }
            })
          })
        });
  }

  addAppointment(){
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedContact = new ContactClass();
    this.selectedEmployee = new EmployeeClass();
  }

  saveAppointment(){
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;

    if(this.isNewAppointment)
    {
      this.appointmentService.postAppointment(this.selectedAppointment).then(appointments => 
        {this.appointment = appointments; 
        this.appointment.guestName = this.contactList.find( id => id.contactId = appointments.guestId).firstName;
        this.appointment.appointmentDate = new Date(this.appointment.appointmentDate).toLocaleDateString();
        tmpAppointmentList.push(this.appointment);
        this.appointmentList = tmpAppointmentList;
      });
    }
    else{
      this.appointmentService.putAppointment(this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointment =>
        {this.appointment = appointment; 
        tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = appointment;
        this.appointmentList = tmpAppointmentList;
        });
    }
    this.selectedAppointment   = null;
    this.isNewAppointment = false;  
  }
  
  onRowSelect(){
    this.isNewAppointment = false;
    this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.selectedContact =  this.contactList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
  }

  cloneRecord(r: Appointment): Appointment{
    let appointment = new AppointmentClass();
    for(let prop in r){
      appointment[prop] = r[prop];
    }
    return appointment;
  }

  deleteAppointment(){
    if(this.selectedAppointment != null && !this.isNewAppointment){
      this.confirmationService.confirm({
        message: 'Do you want to delete this record?',
        header: 'Delete Confirmation',
        icon: 'fa fa-trash',
        accept: () => {
          let tmpAppointmentList = [...this.appointmentList];
          this.appointmentService.deleteAppointment(this.selectedAppointment.appointmentId);
          tmpAppointmentList.splice(this.appointmentList.indexOf(this.selectedAppointment), 1);

          this.appointmentList = tmpAppointmentList;
          this.selectedAppointment   = null;
          this.isNewAppointment = false;  
            this.msgs = [{severity:'info', summary:'Confirmed', detail:'Record deleted'}];
        },
        reject: () => {
            this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
        }
      });
    } 
  }
  
  cancelAppointment(){
    if(this.isNewAppointment){
      this.selectedAppointment = null;
    }
    else{
      let tmpAppointment = [...this.appointmentList];
      tmpAppointment[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
      this.appointmentList = tmpAppointment;
      this.selectedAppointment = null;
    }
  }
}
