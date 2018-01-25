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
import { ConfirmationService, MenuItem, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';

import { GenericService } from '../services/genericservice';
import { Record } from '../domain/record';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [ AppointmentService, ContactService, EmployeeService, ConfirmationService, GenericService]
})
export class AppointmentsComponent implements OnInit {
  items : MenuItem[] = [];
  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  isNewAppointment: boolean;
  cloneAppointment: Appointment;
  appointment: Appointment;
  
  display : boolean = false;

  contactList: Contact[];
  selectedContact: Contact;

  employeeList: Employee[];
  selectedEmployee: Employee;

  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  searchFilter: string = "";
  totalRecord: number = 0;
  searchButtonClickCtr: number = 0;
  retrieveRecordResult: Record<Appointment>;

  service: string = "Appointments";

  constructor(private appointmentService: AppointmentService,
              private contactService: ContactService, private employeeService: EmployeeService, 
              private genericService: GenericService,
              private confirmationService: ConfirmationService) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.items = [
      {label: 'Dashboard', icon: 'fa fa-book fa-5x', routerLink: ['/dashboard']},
      {label: 'Appointments', icon: 'fa fa-book fa-5x', routerLink: ['/appointments']}
    ]
    
    // this.userform = this.fb.group({
    //   'firstName' : new FormControl('', Validators.required),
    //   'lastName' : new FormControl('', Validators.required),
    //   'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern('^\\d+$')])),
    //   'streetAddress': new FormControl('', Validators.required),
    //   'cityAddress': new FormControl('', Validators.required),
    //   'zipCode' : new FormControl('', Validators.required),
    //   'country': new FormControl('', Validators.required),
    //   'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(REGEXEMAIL)])),
    // });
  }
  
  retrieveRecord(event){
    this.genericService.getRecord<Contact>("Contacts").then(contacts =>{
      this.contactList = contacts;
      this.genericService.getRecord<Employee>("Employees").then(employees =>{
        this.employeeList = employees;
        this.genericService.getPaginatedRecord<Record<Appointment>>(this.service, event.first, event.rows,
          this.searchFilter.length == 1 ? "" : this.searchFilter).then(appointmentsRecord => { 
          this.retrieveRecordResult = appointmentsRecord;
          this.appointmentList = this.retrieveRecordResult.result;
          this.totalRecord = this.retrieveRecordResult.totalRecord;
          for(let i = 0; this.appointmentList.length > i; i++){
            let tempEmployee = this.employeeList.find( id => id.employeeId == this.appointmentList[i].hostId);
            let tempContact= this.contactList.find( id => id.contactId == this.appointmentList[i].guestId);
            this.appointmentList[i].guestName = tempContact.firstName+' '+tempContact.lastName;
            this.appointmentList[i].hostName = tempEmployee.firstName+' '+tempEmployee.lastName;
            this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
          }
        });
      });
    });
  }

  search(){
    this.dataTable.reset();
  }

  cloneRecord(r: Appointment): Appointment{
    let appointment = new AppointmentClass();
    for(let prop in r){
      appointment[prop] = r[prop];
    }
    return appointment;
  }

  
  addAppointment(){
    this.isNewAppointment = true;
    this.display = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedContact = new ContactClass();
    this.selectedEmployee = new EmployeeClass();
  }

  saveAppointment(isNewSave: boolean){
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;  
    let selectedDate = this.selectedAppointment.appointmentDate;
    if(this.isNewAppointment)
    {
      this.genericService.updateRecord(this.service, this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointments => 
        {this.appointment = appointments; 
        this.appointment.guestName = this.contactList.find( x => x.contactId == appointments.guestId).firstName;
        this.appointment.hostName = this.employeeList.find( x => x.employeeId == appointments.hostId).firstName;
        this.appointment.appointmentDate = new Date(this.appointment.appointmentDate).toLocaleDateString();
        tmpAppointmentList.push(this.appointment);
        this.selectedAppointment = isNewSave? new AppointmentClass(): null;
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

  editAppointment(appointment: Appointment){
    
    // this.userform.enable();
    // this.btnSave = true;
    // this.btnDelete = false;
    // this.btnSaveNew = false;
    this.cloneAppointment =  Object.assign({}, appointment);
    this.selectedContact = this.contactList.find( x => x.contactId == appointment.guestId);
    this.selectedEmployee = this.employeeList.find( x => x.employeeId == appointment.hostId);
    this.selectedAppointment = this.cloneAppointment;
    
    this.isNewAppointment = false;
    this.display = true;
  }
}

