import { Component, OnInit } from '@angular/core';
import { Appointment } from "../domain/appointment";
import { AppointmentClass } from "../domain/appointmentClass";
import { Contact } from "../domain/contact";
import { ContactClass } from "../domain/contactClass";
import { Employee } from "../domain/employee";
import { EmployeeClass } from "../domain/employeeClass";
import { GlobalService } from "../services/globalService";

import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { API_URL } from "../services/constants";

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css'],
  providers: [GlobalService]
})

export class AppointmentComponent implements OnInit {
  //contact variable
  appointmentList: Appointment[];
  selectedAppointment:Appointment;
  cloneAppointment:Appointment;
  isNewAppointment:boolean;  
  contactList: Contact[];  
  selectedContact: Contact; 
  employeeList: Employee[];  
  selectedEmployee: Employee; 
  selectedCancel: boolean;
  selectedDone: boolean;
  //regex  
  regExpTimeFormat: string = "^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])(:[0-5][0-9])?$";
  //db path
  serviceUrlContact: string = `${API_URL}/contacts/`;
  serviceUrlEmployee: string = `${API_URL}/employees/`;
  serviceUrlAppointment: string = `${API_URL}/appointments/`;
  //form
  appointmentForm: FormGroup;
  dialogTitle: String;
  //displayable
  displayInputBox: boolean = false;
  onEdit: boolean = false;
  onDelete: boolean = false;
  onAdd: boolean = false;
  //confirmation
  confirmEditCancel: boolean = false;
  confirmDeleteRecord: boolean = false;

  constructor(private globalService: GlobalService, private formbuilder: FormBuilder) {}

  ngOnInit() {
    this.globalService.retrieve(this.serviceUrlContact)

    .then(contact => {
      this.contactList = contact;

      this.globalService.retrieve(this.serviceUrlEmployee)
      .then(employee => {
        this.employeeList = employee;

        this.globalService.retrieve(this.serviceUrlAppointment)
        .then(appointments => {
          this.appointmentList = appointments;
          this.appointmentList = this.getGuestNHostNameAndDate(0, this.appointmentList.length);
          this.contactList = this.getFullNameContact();
          this.employeeList = this.getFullNameEmployee();      
        });
      });
    });

    this.appointmentForm = this.formbuilder.group({
      'appointmentDate': new FormControl('', Validators.required),
      'guestName': new FormControl('', Validators.required),
      'hostName': new FormControl('', Validators.required),      
      'startTime': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regExpTimeFormat)])),
      'endTime': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regExpTimeFormat)])),
      'isCancelled': new FormControl(''),  
      'isDone': new FormControl(''),  
      'notes': new FormControl(''),
      });
  }  

  //--------------------EXTRA FUNCTIONS-----------------------
  getGuestNHostNameAndDate(index: number,length: number):Appointment[]{
    for (let i = index; i < length; i++) {
      this.selectedContact = this.cloneContact(this.contactList.find(contact => contact.contactId == this.appointmentList[i].guestId));
      this.appointmentList[i].guestName = this.selectedContact.firstName + " " + this.selectedContact.lastName;

      this.selectedEmployee = this.cloneEmployee(this.employeeList.find(employee => employee.employeeId == this.appointmentList[i].hostId));
      this.appointmentList[i].hostName = this.selectedEmployee.firstName + " " + this.selectedEmployee.lastName;
      
      this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
    }
    return this.appointmentList;
  }

  getFullNameContact():Contact[]{
    for (let i = 0; i < this.contactList.length; i++) {
      this.contactList[i].fullName = this.contactList[i].firstName + " " + this.contactList[i].lastName;
    }
    return this.contactList;
  }

  getFullNameEmployee():Employee[]{
    for (let i = 0; i < this.employeeList.length; i++) {
      this.employeeList[i].fullName = this.employeeList[i].firstName + " " + this.employeeList[i].lastName;
    }
    return this.employeeList;
  }

  //--------------------CREATE-----------------------
  addAppointment(){
    this.isNewAppointment = true;
    this.clearVariables();
    this.selectedAppointment = new AppointmentClass(); 
    //form
    this.displayForm("New Appointment", true);
    //displayable      
    this.onEdit= false;
    this.onDelete = false;
    this.onAdd = true;
    //confirmation
    this.confirmEditCancel = false;
    this.confirmDeleteRecord = false;
  }  
  //--------------------EDIT-----------------------
  editAppointment(appointment: Appointment){    
    //this.isNewAppointment = false;  
    this.selectedAppointment = appointment;  
    this.onRowSelect();
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    //form
    this.displayForm("Edit Appointment", true);
    //displayable      
    this.onEdit= true;
    this.onDelete = false;
    this.onAdd = false;
  }
  //--------------------DELETE-----------------------
  deleteAppointment(appointment: Appointment, rowSelect: boolean){
    //displayable      
    this.onEdit= false;
    this.onDelete = true;
    this.onAdd = false;

    if(rowSelect){
      this.selectedAppointment = appointment;
      this.onRowSelect();
      //form
      this.displayForm("Delete Appointment", false);
    }
    else{
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = true;
    }
  } 
  //--------------------FORM-----------------------
  saveAppointment(withNew:boolean){
    let tempAppointmentList = [...this.appointmentList]; 
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;
    this.selectedAppointment.isCancelled = this.selectedCancel ? true : false;
    this.selectedAppointment.isDone = this.selectedDone? true : false;

    if(this.isNewAppointment){    
      this.globalService.add(this.serviceUrlAppointment, this.selectedAppointment).then(appointment => {
        tempAppointmentList.push(appointment);
        this.appointmentList = tempAppointmentList;   
        this.appointmentList = this.getGuestNHostNameAndDate(this.appointmentList.indexOf(appointment),this.appointmentList.indexOf(appointment)+1);      
        this.clearVariables();
        this.displayInputBox = false;
        if(withNew){
          this.addAppointment();
        }
      })
    }
    else{      
      this.globalService.update(this.serviceUrlAppointment, this.selectedAppointment, this.selectedAppointment.appointmentId).then(appointment => {
        tempAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)];
        this.appointmentList = tempAppointmentList;  
        this.appointmentList = this.getGuestNHostNameAndDate(this.appointmentList.indexOf(appointment),this.appointmentList.indexOf(appointment)+1);      
        this.clearVariables();
        this.displayInputBox = false;
        });
    } 
    this.isNewAppointment = false;  
  }  

  cancelAppointment(){
    if(this.onAdd){
      this.toCancel(true);
    }
    else if(this.onEdit){
      if(this.appointmentForm.dirty){  
        this.confirmEditCancel = true;
      }
      if(this.appointmentForm.pristine){
        this.displayInputBox = false;
      }
    }
    else if(this.onDelete){
      this.confirmDeleteRecord = false;
      this.displayInputBox = false;
    }    
    else{
      this.displayInputBox = false;
    }
  }
  //--------------------CONFIRMATION-----------------------
  toCancel(discard:boolean){  
    if(discard) {
      if(this.onEdit){
        this.isNewAppointment = false;
        let tempAppointmentList = [...this.appointmentList];
        tempAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
        this.appointmentList = tempAppointmentList;
        this.selectedAppointment = this.cloneAppointment;        
        this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
        this.appointmentForm.markAsPristine();
      }
      else{
        //displayable
        this.displayInputBox = false;
      }      
    }
    this.confirmEditCancel = false;
  }

  toDelete(toDelete: boolean){
    if(toDelete){    
      this.displayInputBox = true;
      this.globalService.delete(this.serviceUrlAppointment, this.selectedAppointment, this.selectedAppointment.appointmentId).then(appointment => {
      let tempAppointmentList = [...this.appointmentList];
      tempAppointmentList.splice(this.appointmentList.indexOf(this.selectedAppointment), 1);   
      this.appointmentList = tempAppointmentList;
      this.clearVariables(); 
      })
    }   
    this.displayInputBox = false; 
    this.confirmDeleteRecord = false;
  }   
  //--------------------MISC-----------------------
  cloneRecord(r: Appointment): Appointment {
    let appointment = new AppointmentClass();
    for (let prop in r) {
      appointment[prop] = r[prop];
    }
    return appointment;
  }
  
  cloneContact(r: Contact): Contact {
    let contact = new ContactClass();
    for (let prop in r) {
      contact[prop] = r[prop];
    }
    return contact;
  }

  cloneEmployee(r: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }

  displayForm(title:string, editable: boolean){
    this.dialogTitle = title;
    this.appointmentForm.markAsPristine();
    if(editable){
      this.appointmentForm.enable();
    }
    else{
      this.appointmentForm.disable();
    }
    this.displayInputBox = true;
  }

  clearVariables(){    
    this.selectedAppointment = null; 
    this.selectedCancel=false;
    this.selectedDone=false;
    this.selectedContact = null;
    this.selectedEmployee = null;       
  }

  onRowSelect() {
    this.isNewAppointment = false;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment); 
    this.selectedContact = this.contactList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.selectedCancel = this.selectedAppointment.isCancelled ? true : false; 
    this.selectedDone = this.selectedAppointment.isDone ? true : false; 
  }  
}
