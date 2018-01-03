import { Component, OnInit } from '@angular/core';
import {Appointment} from '../domain/Appointment';
import {AppointmentService} from '../services/AppointmentService';
import {HttpClient} from '@angular/common/http';
import {AppointmentClass} from '../domain/AppointmentClass';

import {CalendarModule} from 'primeng/primeng';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [AppointmentService]
})
export class AppointmentsComponent implements OnInit {

  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;

  constructor(private appointmentService:AppointmentService,
    private http:HttpClient) { }

  ngOnInit() {
    this.appointmentService.getAppointments().then(appointments => this.appointmentList = appointments);
  }

  addAppointment(){
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
  }

  saveAppointment(){
    let tmpAppointmentList = [...this.appointmentList];
    if(this.isNewAppointment){
      tmpAppointmentList.push(this.selectedAppointment);
      this.appointmentService.postAppointments(this.selectedAppointment);
    }
    else{
      tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.selectedAppointment;
      this.appointmentService.putAppointments(this.selectedAppointment);
    }

    this.appointmentList=tmpAppointmentList;
    this.selectedAppointment=null;
    this.isNewAppointment = false;
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
    let index = this.findSelectedAppointmentIndex();
    this.appointmentList = this.appointmentList.filter((val,i) => i!=index);
    this.appointmentService.deleteAppointments(this.selectedAppointment.appointmentId);
    this.selectedAppointment = null;
  }

}
