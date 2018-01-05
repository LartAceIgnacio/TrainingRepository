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

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [GlobalService]
})

export class AppointmentsComponent implements OnInit {
  appointmentList: Appointment[];
  cloneAppointment: Appointment;
  selectedAppointment: Appointment;
  isNewAppointment: boolean;
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
  rexExpTimeFormat: string = "^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])(:[0-5][0-9])?$";

  constructor(private fb: FormBuilder, private globalservice: GlobalService) {
  }

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

    this.globalservice.getSomething<Contact>("Contacts").then(contacts => {
      this.contactList = contacts;
      this.globalservice.getSomething<Employee>("Employees").then(employees => {
        this.employeeList = employees;
        this.globalservice.getSomething<Appointment>("Appointments").then(appointments => {
          this.appointmentList = appointments;
          this.getFullName();
        });
      });
    });
  }

  addAppointment() {
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.selectedContact = new ContactClass();
    this.selectedEmployee = new EmployeeClass();
    this.selectedCancelled = "false";
    this.selectedDone = "false";
    this.dateActivated = null;
  }

  onRowSelect(event) {
    this.isNewAppointment = false;
    this.indexOfAppointment = this.appointmentList.indexOf(this.selectedAppointment);

    this.selectedContact = this.contactList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.dateActivated = this.selectedAppointment.appointmentDate;

    this.cloneAppointment = Object.assign({}, this.selectedAppointment);
    this.selectedAppointment = Object.assign({}, this.selectedAppointment);

    this.selectedCancelled = this.selectedAppointment.isCancelled + "";
    this.selectedDone = this.selectedAppointment.isDone + "";
  }

  btnSave() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.isCancelled = this.selectedCancelled == 'true' ? true : false;
    this.selectedAppointment.isDone = this.selectedDone == 'true' ? true : false;
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;
    this.selectedAppointment.appointmentDate = this.dateActivated;
    if (this.isNewAppointment) {
      this.globalservice.addSomething<Appointment>("Appointments", this.selectedAppointment).then(appointments => {
        tmpAppointmentList.push(appointments);
        this.appointmentList = tmpAppointmentList;
        this.getFullName();
        this.selectedAppointment = null;
      });
    }
    else {
      this.globalservice.updateSomething<Appointment>("Appointments", this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointments => {
        tmpAppointmentList[this.indexOfAppointment] = this.selectedAppointment;
        this.appointmentList = tmpAppointmentList;
        this.getFullName();
        this.selectedAppointment = null;
      });
    }
    this.isNewAppointment = false;
  }

  btnCancel() {
    if (this.isNewAppointment)
      this.selectedAppointment = null;
    else {
      let tmpAppointmentList = [...this.appointmentList];
      tmpAppointmentList[this.indexOfAppointment] = this.cloneAppointment;
      this.appointmentList = tmpAppointmentList;
      this.selectedContact = this.contactList.find(x => x.contactId == this.cloneAppointment.guestId);
      this.selectedEmployee = this.employeeList.find(x => x.employeeId == this.cloneAppointment.hostId);
      this.selectedAppointment = Object.assign({}, this.cloneAppointment);
    }
  }

  btnDelete() {
    let tmpAppointmentList = [...this.appointmentList];
    tmpAppointmentList.splice(this.indexOfAppointment, 1);
    this.globalservice.deleteSomething<Appointment>("Appointments", this.selectedAppointment.appointmentId).then(appointments => {
      tmpAppointmentList.splice(this.indexOfAppointment, 1);
      this.appointmentList = tmpAppointmentList;
      this.selectedAppointment = null;
      this.isNewAppointment = false;
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
