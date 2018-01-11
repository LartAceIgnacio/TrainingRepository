import { Component, OnInit } from '@angular/core';
import { AppointmentService } from './../services/appointmentService';
import { Appointment } from './../domain/appointment';
import { AppointmentClass } from './../domain/appointmentclass';
import { EmployeeService } from './../services/employeeService';
import { Employee } from './../domain/employee';
import { EmployeeClass } from './../domain/employeeclass';
import { ContactService } from './../services/contactService';
import { Contact } from './../domain/contact';
import { ContactClass } from './../domain/contactclass';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import {Message,SelectItem} from 'primeng/components/common/api';
import { MenuItem , ConfirmationService} from "primeng/primeng";

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [AppointmentService , EmployeeService , ContactService, ConfirmationService]
})
export class AppointmentsComponent implements OnInit {

  appointmentItems: MenuItem[];

  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  selectedHost: Employee;
  selectedGuest: Contact;

  clonedSelectedappointment: Appointment;
  indexSelected: number;

  contactList: Contact[];
  employeeList: Employee[];
  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;
  displayDialog: boolean;
  loading: boolean;
  appointment: Appointment = new AppointmentClass();

  btnSaveNew: boolean;
  btnDelete: boolean;
  btnSave: boolean;

  display: boolean = false;
  
      showDialog() {
          this.display = true;
      }

  constructor(private appointmentService: AppointmentService , 
    private contactService: ContactService,
    private employeeService: EmployeeService, 
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) {
   
   }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.contactService.getContacts().then(contacts => { this.contactList = contacts
      this.employeeService.getEmployees().then(employees => {this.employeeList = employees
      this.appointmentService.getAppointments().then(appointments => {this.appointmentList = appointments
        for(let i=0; i < this.appointmentList.length;i++){
          this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
          this.appointmentList[i].guestName = this.contactList.find(id=>id.contactId==this.appointmentList[i].guestId).firstName;
          this.appointmentList[i].hostName = this.employeeList.find(id=>id.employeeId==this.appointmentList[i].hostId).firstName;
        }});});});
      this.loading = false; 
    }, 1000);
    //this.selectedappointment = this.appointmentList[0]; 
    this.userform = this.fb.group({
      'date': new FormControl('', Validators.required),
      'guestName': new FormControl('', Validators.required),
      'hostName': new FormControl('', Validators.required),
      'startTime': new FormControl('', Validators.required),
      'endTime': new FormControl('', Validators.required),
      'isCancelled': new FormControl('', Validators.required),
      'isDone': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required)
  });

    this.appointmentItems = [
      {label:'Dashboard', routerLink:['/dashboard'] },
      {label:'Appointments', routerLink:['/appointments']}
    ]
  }

  addAppointments() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass;
    this.displayDialog = true;
    this.selectedGuest = new ContactClass;
    this.selectedHost = new EmployeeClass;
    this.userform.markAsPristine();
  }

  saveAppointments() {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
      
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;
    
    if(this.isNewAppointment)
      {
        this.appointmentService.addAppointments(this.selectedAppointment)
        .then(appointments => {this.appointment = appointments; 
          tmpAppointmentList.push(this.appointment);
          this.appointmentList = tmpAppointmentList;
          for(let i=0; i < this.appointmentList.length;i++){
            this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
            this.appointmentList[i].guestName = this.contactList.find(id=>id.contactId==this.appointmentList[i].guestId).firstName;
            this.appointmentList[i].hostName = this.employeeList.find(id=>id.employeeId==this.appointmentList[i].hostId).firstName;
          }
        });
      }
      else{
        this.appointmentService.saveAppointments(this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointment =>
          {this.appointment = appointment; 
          tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = appointment;
          this.appointmentList = tmpAppointmentList;
          });
          
    }
    //this.appointmentservice.saveappointments(this.selectedappointment);
    this.selectedAppointment = null;
    this.isNewAppointment = false;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
this.userform.markAsPristine();
  }

  saveNewAppointments() {
    this.userform.enable();
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;
    
    if(this.isNewAppointment)
      {
        this.appointmentService.addAppointments(this.selectedAppointment)
        .then(appointments => {this.appointment = appointments; 
          tmpAppointmentList.push(this.appointment);
          this.appointmentList = tmpAppointmentList;
        });
      }
      else{
        this.appointmentService.saveAppointments(this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointment =>
          {this.appointment = appointment; 
          tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = appointment;
          this.appointmentList = tmpAppointmentList;
          });
    }
    //this.appointmentservice.saveappointments(this.selectedappointment);
    this.selectedAppointment = new AppointmentClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Appointment: Appointment){
    this.userform.disable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedAppointment = Appointment;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
  }

  deleteAppointments(){
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
    this.selectedAppointment;
    let index = this.findSelectedAppointmentIndex();
    this.appointmentList = this.appointmentList.filter((val,i) => i!=index);
    this.appointmentService.deleteAppointments(this.selectedAppointment.appointmentId);
    this.selectedAppointment = null;
    this.displayDialog = false;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  editAppointments(Appointment: Appointment){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewAppointment = false;
    this.selectedAppointment = Appointment;
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();
    this.selectedGuest = this.contactList.find(guest => guest.contactId == this.selectedAppointment.guestId);
    this.selectedHost = this.employeeList.find(host => host.employeeId == this.selectedAppointment.hostId);
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  // onRowSelect(event) {
  //         this.isNewAppointment = false;
  //         this.selectedAppointment;
  //         this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
  //         // this.displayDialog = true;
  //         this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();
  //         this.selectedGuest = this.contactList.find(guest => guest.contactId == this.selectedAppointment.guestId);
  //         this.selectedHost = this.employeeList.find(host => host.employeeId == this.selectedAppointment.hostId);
  // } 

  cloneRecord(r: Appointment): Appointment {
      let appointment = new AppointmentClass();
      for(let prop in r) {
          appointment[prop] = r[prop];
      }
      return appointment;
  }

  cancelAppointments(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
    
    this.isNewAppointment = false;
    let tmpappointmentList = [...this.appointmentList];
    tmpappointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
    this.appointmentList = tmpappointmentList;
    this.selectedAppointment = this.cloneAppointment;
    this.selectedAppointment = null;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  findSelectedAppointmentIndex(): number {
      return this.appointmentList.indexOf(this.selectedAppointment);
  }
}