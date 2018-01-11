import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';

import {BreadcrumbModule,MenuItem} from 'primeng/primeng';
import {ConfirmDialogModule,ConfirmationService} from 'primeng/primeng';
import {CalendarModule} from 'primeng/primeng';

import {AppointmentClass} from '../domain/AppointmentClass';
import {Appointment} from '../domain/Appointment';
import {AppointmentService} from '../services/AppointmentService';

import {Employee} from '../domain/Employee';
import {EmployeeService} from '../services/EmployeeService';

import {Contact} from '../domain/Contact';
import {ContactService} from '../services/ContactService';

import { ContactClass } from "../domain/ContactClass";
import { EmployeeClass } from "../domain/EmployeeClass";

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [AppointmentService, ContactService, EmployeeService, ConfirmationService]
})
export class AppointmentsComponent implements OnInit {

  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;

  guestList: Contact[];
  selectedGuest: Contact;

  hostList: Employee[];
  selectedHost: Employee;

  brAppointment: MenuItem[];
  home: MenuItem;

  display: boolean;
  isDelete: boolean;

  appointmentForm: FormGroup;

  constructor(private appointmentService:AppointmentService,
    private contactService:ContactService,
    private employeeService:EmployeeService,
    private http:HttpClient,
    private fb: FormBuilder,
    private confirmationService: ConfirmationService) {}

  ngOnInit() {
    this.appointmentForm = this.fb.group({
      'appointmentdate': new FormControl('', Validators.required),
      'guestid': new FormControl('', Validators.required),
      'hostid': new FormControl('', Validators.required),
      'starttime': new FormControl('', Validators.required),
      'endtime': new FormControl('', Validators.required),
      'iscancelled': new FormControl(''),
      'isdone': new FormControl(''),
      'notes': new FormControl('')
    });

    this.contactService.getContacts().then(contacts => {
      this.guestList = contacts
      this.employeeService.getEmployees().then(employees =>{
        this.hostList = employees
        this.appointmentService.getAppointments().then(appointments => {
          this.appointmentList = appointments
          for(let i=0;i < this.appointmentList.length; i++){
            this.appointmentList[i].guestName = this.guestList.find(id=>id.contactId==this.appointmentList[i].guestId).firstName;
            this.appointmentList[i].hostName = this.hostList.find(id=>id.employeeId==this.appointmentList[i].hostId).firstName;
            this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate);
          }});
      });
    });

    this.brAppointment=[
      {label: 'Appointments', url: '/appointments'}
    ]
    this.home = {icon: 'fa fa-home', routerLink: '/dashboard'};
    this.isDelete = false;
  }

  addAppointment(){
    this.isDelete = false;
    this.appointmentForm.enable();
    this.appointmentForm.markAsPristine();
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedGuest = new ContactClass();
    this.selectedHost = new EmployeeClass();
    this.display = true;
  }

  saveAppointment(){
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    if(this.isNewAppointment){
      this.appointmentService.postAppointments(this.selectedAppointment).then(appointment => {
        tmpAppointmentList.push(appointment);
        this.appointmentList=tmpAppointmentList;       
        this.selectedAppointment=null;
      });
    }
    else{
      this.appointmentService.putAppointments(this.selectedAppointment).then(appointment =>{
        tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = appointment;
        this.appointmentList=tmpAppointmentList;
        this.selectedAppointment=null;
        this.isNewAppointment = false;
      });
    }
  }

  cancelAppointment(){
    let index = this.findSelectedAppointmentIndex();
    if (this.isNewAppointment)
      this.selectedAppointment = null;
    else {
      let tmpAppointmentList = [...this.appointmentList];
      tmpAppointmentList[index] = this.cloneAppointment;
      this.appointmentList = tmpAppointmentList;
      this.selectedAppointment = Object.assign({}, this.cloneAppointment);
    }
  }

  onRowSelect(){
    this.isNewAppointment = false;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate);

    this.selectedGuest = this.guestList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedHost = this.hostList.find(x => x.employeeId == this.selectedAppointment.hostId);
  }

  cloneRecord(r: Appointment): Appointment{
    let appointment = new AppointmentClass();
    for(let prop in r){
      appointment[prop]=r[prop];
    }
    return appointment;
  }

  findSelectedAppointmentIndex():number{
    return this.appointmentList.indexOf(this.selectedAppointment);
  }

  deleteAppointment(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
          //Actual logic to perform a confirmation
          let index = this.findSelectedAppointmentIndex();
          this.appointmentList = this.appointmentList.filter((val,i) => i!=index);
          this.appointmentService.deleteAppointments(this.selectedAppointment.appointmentId);
          this.selectedAppointment = null;
      }
    });
  }

  saveAndNewAppointment(){
    this.appointmentForm.markAsPristine();
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;

    tmpAppointmentList.push(this.selectedAppointment);
    this.appointmentService.postAppointments(this.selectedAppointment).then(appointment => {
      tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = appointment;
      this.appointmentList = tmpAppointmentList;
    });

    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedGuest = new ContactClass();
    this.selectedHost = new EmployeeClass();
  }

  editAppointment(Appointment: Appointment){
    this.appointmentForm.enable();
    this.isDelete = false;
    this.selectedAppointment=Appointment;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.display=true;
    this.isNewAppointment = false;
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate);
    this.selectedGuest = this.guestList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedHost = this.hostList.find(x => x.employeeId == this.selectedAppointment.hostId);
  }

  confirmDelete(Appointment: Appointment){
    this.appointmentForm.markAsPristine();
    this.selectedAppointment=Appointment;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.isDelete = true;
    this.display=true;
    this.appointmentForm.disable();
    this.isNewAppointment = false;
  }

}
