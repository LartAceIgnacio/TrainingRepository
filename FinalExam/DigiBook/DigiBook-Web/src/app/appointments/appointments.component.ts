import { DataTable } from 'primeng/components/datatable/datatable';

import { Component, OnInit, ViewChild } from '@angular/core';
import { Appointment } from '../domain/appointment';
import { AppointmentService } from '../services/appointmentService';
import { Appointmentclass } from '../domain/appointmentclass';
import { Contact } from '../domain/contact';
import { Employee } from '../domain/employee';
import { Contactclass } from '../domain/contactclass';
import { Employeeclass } from '../domain/employeeclass';
import { MenuItem, ConfirmationService } from 'primeng/primeng';
import { Message } from 'primeng/components/common/api';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { GlobalService } from '../services/global.service';
import { Pagination } from '../domain/pagination';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers:[GlobalService,ConfirmationService,DatePipe]
})
export class AppointmentsComponent implements OnInit {
  clonedSelectedAppointment: Appointment;
  indexSelected: number;
  checked: boolean;
  msgs: Message[] = [];
  selectedDate: Date;
  appointmentForm: FormGroup;
  invalidDates: Array<Date>;

  indexOfAppointment:number;
  serviceName: string = "Appointments";
  pagination: Pagination<Appointment>;
  totalRecords: number = 0;
  ctr: number = 0;
  searchFilter: string = "";
  dateFilter: Date[];

  selectedGuest: Contact;
  selectedHost: Employee;
  guestList: Contact[];
  hostList : Employee[];
  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  cloneAppointment: Appointment;
  isNewAppointment: boolean;
  displayDialog: boolean;
  loading: boolean;
  delete : boolean;
  breadcrumb: MenuItem[];
  appointment: Appointment = new Appointmentclass();

  constructor(private globalService: GlobalService, private fb: FormBuilder, 
              private confirmationService: ConfirmationService, private datePipe: DatePipe) { }
              
  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {

    this.appointmentForm = this.fb.group({
      'guestName': new FormControl('', Validators.required),
      'hostName': new FormControl('', Validators.required),
      'startTime': new FormControl('', Validators.required),
      'endTime': new FormControl('', Validators.required),
      'isCancelled': new FormControl('', Validators.required),
      'isDone': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required),
      'appointmentDate': new FormControl('', Validators.required)
    });

    let today = new Date();
    let invalidDate = new Date();
    invalidDate.setDate(today.getDate() - 1);
    this.invalidDates = [today,invalidDate];
    
    this.breadcrumb = [
      {label:'Dashboard', routerLink:['/dashboard']},
      {label:'Appointments', routerLink:['/appointments']},
  ];
  }
  paginate(event) {
    if (this.ctr == 0) {
      this.globalService.getData<Contact>("Contacts").then(contacts => {
        this.guestList = contacts;
        this.globalService.getData<Employee>("Employees").then(employees => {
          this.hostList = employees;
          this.globalService.getPagination<Pagination<Appointment>>(this.serviceName, event.first, event.rows,
            this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
              this.pagination = pagination;
              this.appointmentList = this.pagination.results;
              this.totalRecords = this.pagination.totalRecords;
              for(let i=0;i < this.appointmentList.length;i++){
              this.appointmentList[i].guestName = this.guestList.find(id=>id.contactId==this.appointmentList[i].guestId).firstName;
              this.appointmentList[i].hostName = this.hostList.find(id=>id.employeeId==this.appointmentList[i].hostId).firstName;
              this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
            }
            });
        });
      });
    }
    else {
      this.globalService.getPagination<Pagination<Appointment>>(this.serviceName, event.first, event.rows,
        this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
          this.pagination = pagination;
          this.appointmentList = this.pagination.results;
          this.totalRecords = this.pagination.totalRecords;
          for(let i=0;i < this.appointmentList.length;i++){
            this.appointmentList[i].guestName = this.guestList.find(id=>id.contactId==this.appointmentList[i].guestId).firstName;
            this.appointmentList[i].hostName = this.hostList.find(id=>id.employeeId==this.appointmentList[i].hostId).firstName;
            this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
          }
        });
    }
  }
  // searchAppointments() {
  //   if (this.dateFilter != null && (this.dateFilter[0] != null || this.dateFilter[1] != null)) {
  //     this.searchFilter = this.dateFilter[0].toLocaleDateString() + "," + this.dateFilter[1].toLocaleDateString();
  //     if (this.searchFilter.length > 0) {
  //       this.searchFilter = this.searchFilter.replace(/\//g, "%2F");
  //     }
  //   }

  //   this.setCurrentPage(1);
  // }
  searchAppointments() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }

    this.setCurrentPage(1);
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    // let paging = {
    //   first: ((n - 1) * this.dataTable.rows),
    //   rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }
  // keyPressFunction(event) {
  //   if (this.searchFilter.length != 1) {
  //     this.setCurrentPage(1);
  //   }

  //   this.setCurrentPage(1);
  // }
  addAppointment() {
    this.isNewAppointment = true;
     this.selectedAppointment = new Appointmentclass;
     this.selectedGuest = new Contactclass;
     this.selectedHost = new Employeeclass;
    this.displayDialog = true;
    this.appointmentForm.enable();
    this.delete = false;
  }
  editAppointment(Appointment : Appointment){
    this.isNewAppointment = false;
    this.selectedAppointment = Appointment;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.delete = false;
    this.selectedGuest = this.guestList.find(x => x.contactId == this.selectedAppointment.guestId);
    this.selectedHost = this.hostList.find(x => x.employeeId == this.selectedAppointment.hostId);
    this.selectedDate = this.selectedAppointment.appointmentDate;

    this.displayDialog = true;
    this.appointmentForm.enable();
  }

  saveAppointment() {
    let tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedGuest.contactId;
    this.selectedAppointment.hostId = this.selectedHost.employeeId;
    this.selectedAppointment.appointmentDate = this.datePipe.transform(this.selectedDate, 'MM/dd/yyyy');

    if(this.isNewAppointment){  
      this.globalService.addData<Appointment>(this.serviceName,this.selectedAppointment).then(appointments => {
        tmpAppointmentList.push(appointments);
        this.appointmentList = tmpAppointmentList;
        
      });
      
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Appointment'});
      this.selectedAppointment = null;
      this.dataTable.reset();
    }else{
        this.globalService.updateData<Appointment>(this.serviceName,this.selectedAppointment.appointmentId, this.selectedAppointment).then(appointments => {     
          tmpAppointmentList[this.indexOfAppointment] = this.selectedAppointment;
          this.appointmentList = tmpAppointmentList;
          
        });
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Appointment Details'});
        this.selectedAppointment = null;
    }
    
    this.appointmentForm.markAsPristine();
    this.selectedAppointment = null;
  }
  saveAndNewAppointment(){
    this.appointmentForm.markAsPristine();
    let tmpAppointmentList = [...this.appointmentList];
    tmpAppointmentList.push(this.selectedAppointment);

    if(this.isNewAppointment){
      this.globalService.addData<Appointment>(this.serviceName,this.selectedAppointment);
      this.appointmentList=tmpAppointmentList;
      this.isNewAppointment = true;
      this.selectedAppointment = new Appointmentclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Appointment'});
    }
    this.dataTable.reset();
  }

  deleteAppointment(Appointment : Appointment){
    this.selectedAppointment = Appointment;
    this.appointmentForm.disable();
    this.delete = true;
    this.displayDialog = true;
  }
  delAppointment(){
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        let index = this.findSelectedAppointmentIndex();
        this.appointmentList = this.appointmentList.filter((val,i) => i!=index);
        this.globalService.deleteData<Appointment>(this.serviceName,this.selectedAppointment.appointmentId);
        this.selectedAppointment = null;
        this.displayDialog = false; 
        this.msgs = [];
        this.msgs.push({severity:'error', summary:'Danger', detail:'Deleted Employee'});
         
      }
  });
  }

  // onRowSelect(event) {
  //         this.isNewAppointment = false;
  //         this.selectedAppointment;
  //         // this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
  //         // this.displayDialog = true;

  //         //this.clonedSelectedAppointment = JSON.parse(JSON.stringify(this.selectedAppointment)); // cloned value of selected

  //         this.selectedGuest = this.guestList.find(x => x.contactId == this.selectedAppointment.guestId);
  //         this.selectedHost = this.hostList.find(x => x.employeeId == this.selectedAppointment.hostId);
  //         this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();
  // } 

  cloneRecord(r: Appointment): Appointment {
      let appointment = new Appointmentclass();
      for(let prop in r) {
          appointment[prop] = r[prop];
      }
      return appointment;
  }

  cancelAppointment(){
    if(this.appointmentForm.dirty){  
      this.confirmationService.confirm({
        message: 'Do you want to Discard this changes?',
        accept: () => {
          this.isNewAppointment = false;
          let tmpAppointmentList = [...this.appointmentList];
          tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
          this.appointmentList = tmpAppointmentList;
          this.selectedAppointment = this.cloneAppointment;
          this.selectedAppointment = null;
        }
      });
    }

    
  }

  findSelectedAppointmentIndex(): number {
      return this.appointmentList.indexOf(this.selectedAppointment);
  }

}