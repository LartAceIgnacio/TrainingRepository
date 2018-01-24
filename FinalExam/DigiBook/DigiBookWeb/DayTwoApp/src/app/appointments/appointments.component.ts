import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointmentservice';
import { Appointment } from '../domain/appointments/appointment';
import { AppointmentClass } from '../domain/appointments/appointmentclass';
import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employees/employee';
import { EmployeeClass } from '../domain/employees/employeeclass';
import { ContactService } from '../services/contactservice';
import { Contact } from '../domain/contacts/contact';
import { ContactClass } from '../domain/contacts/contactclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [GlobalService, ConfirmationService, FormBuilder]
})
export class AppointmentsComponent implements OnInit {
  items: MenuItem[];
  home: MenuItem;
  employeeList: Employee[];
  contactList: Contact[];
  appointmentList: Appointment[];
  selectedAppointment: Appointment;
  selectedEmployee: Employee;
  selectedContact: Contact;
  isNewAppointment: boolean;
  cloneAppointment: Appointment;
  display: boolean;
  userform: FormGroup;
  isDelete: boolean;
  isEdit: boolean;
  dateFilter: Date[];
  // tslint:disable-next-line:no-inferrable-types
  ctr: number = 0;
  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';
  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;
  paginationResult: PaginationResult<Appointment>;


  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {

    this.items = [
      { label: 'Appointments' }
    ];
    this.home = { icon: 'fa fa-home', label: 'Home', routerLink: '/dashboard' };

    this.userform = this.fb.group({
      'appointmentdate': new FormControl('', Validators.required),
      'guestname': new FormControl('', Validators.required),
      'hostname': new FormControl('', Validators.required),
      'starttime': new FormControl('', Validators.required),
      'endtime': new FormControl('', Validators.required),
      'iscancelled': new FormControl(''),
      'isdone': new FormControl(''),
      'notes': new FormControl('')
    });
  }

  showDialog() {
    this.isEdit = false;
    this.isDelete = false;
    this.userform.enable();
    this.userform.markAsPristine();
    this.isNewAppointment = true;
    this.selectedAppointment = new AppointmentClass();
    this.display = true;
  }

  searchAppointment() {
    if (this.dateFilter != null && (this.dateFilter[0] != null || this.dateFilter[1] != null)) {
      this.searchFilter = this.dateFilter[0].toLocaleDateString() + ',' + this.dateFilter[1].toLocaleDateString();
      if (this.searchFilter.length > 0) {
        this.searchFilter = this.searchFilter.replace(/\//g, '%2F');
      }
    }

    this.setCurrentPage(1);
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    const paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }

  paginate(event) {
    if (this.ctr === 0) {
      this.globalService.getSomething<Contact>('Contacts').then(contacts => {
        this.contactList = contacts;
        this.globalService.getSomething<Employee>('Employees').then(employees => {
          this.employeeList = employees;
          this.globalService.getSomethingWithPagination<PaginationResult<Appointment>>('Appointments', event.first, event.rows,
            this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
              this.paginationResult = paginationResult;
              this.appointmentList = this.paginationResult.results;
              this.totalRecords = this.paginationResult.totalRecords;
              this.getFullName();
            });
        });
      });
    } else {
      this.globalService.getSomethingWithPagination<PaginationResult<Appointment>>('Appointments', event.first, event.rows,
        this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.appointmentList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
          this.getFullName();
        });
    }
  }

  editAppointment(appointmentToEdit: Appointment) {
    this.isEdit = true;
    this.isDelete = false;
    this.userform.enable();
    this.isNewAppointment = false;
    this.selectedAppointment = this.cloneRecord(appointmentToEdit);
    this.cloneAppointment = appointmentToEdit;
    this.display = true;

    this.selectedAppointment.appointmentDate = new Date(this.cloneAppointment.appointmentDate).toLocaleDateString();
    this.selectedContact = this.contactList.find(contact => contact.contactId === this.cloneAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(employee => employee.employeeId === this.cloneAppointment.hostId);
  }

  deleteAppointment(appointmentToDelete: Appointment) {
    this.isDelete = true;
    this.userform.disable();
    this.display = true;
    this.isNewAppointment = false;
    this.selectedAppointment = this.cloneRecord(appointmentToDelete);
    this.cloneAppointment = appointmentToDelete;
    this.userform.markAsPristine();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        this.globalService.deleteSomething<Appointment>('Appointments', this.selectedAppointment.appointmentId);
        const index = this.appointmentList.indexOf(this.cloneAppointment);
        this.appointmentList = this.appointmentList.filter((val, i) => i !== index);
        this.selectedAppointment = null;
        this.isNewAppointment = false;
        this.display = false;
        this.setCurrentPage(1);
      }
    });
  }

  saveAppointment() {
    const tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;
    if (this.isNewAppointment) {
      this.globalService.postSomething<Appointment>('Appointments', this.selectedAppointment)
        .then(appointment => {
          this.selectedAppointment = appointment;
          this.selectedAppointment.guestName = this.contactList.find(id => id.contactId === this.selectedAppointment.guestId).firstName;
          this.selectedAppointment.hostName = this.employeeList.find(id => id.employeeId === this.selectedAppointment.hostId).firstName;
          tmpAppointmentList.push(this.selectedAppointment);
          this.appointmentList = tmpAppointmentList;
          this.selectedAppointment = null;
          this.display = false;
          this.setCurrentPage(1);
        });
    } else {
      this.globalService.putSomething<Appointment>('Appointments', this.selectedAppointment.appointmentId, this.selectedAppointment)
        .then(appointment => {
          tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)]
            = this.selectedAppointment;
          this.appointmentList = tmpAppointmentList;
          this.getFullName();
          this.selectedAppointment = null;
          this.display = false;
        });
    }
    this.isNewAppointment = false;
  }

  newSaveAppointment() {
    const tmpAppointmentList = [...this.appointmentList];
    this.selectedAppointment.guestId = this.selectedContact.contactId;
    this.selectedAppointment.hostId = this.selectedEmployee.employeeId;
    this.globalService.postSomething<Appointment>('Appointments', this.selectedAppointment)
      .then(appointment => {
        this.selectedAppointment = appointment;
        this.selectedAppointment.guestName = this.contactList.find(id => id.contactId === this.selectedAppointment.guestId).firstName;
        this.selectedAppointment.hostName = this.employeeList.find(id => id.employeeId === this.selectedAppointment.hostId).firstName;
        tmpAppointmentList.push(this.selectedAppointment);
        this.appointmentList = tmpAppointmentList;
        this.selectedAppointment = new AppointmentClass;
        this.display = true;
      });
  }

  cancelAppointment() {
    this.isNewAppointment = false;
    const tmpAppointmentList = [...this.appointmentList];
    tmpAppointmentList[this.appointmentList.indexOf(this.selectedAppointment)] = this.cloneAppointment;
    this.appointmentList = tmpAppointmentList;
    this.selectedAppointment = Object.assign({}, this.cloneAppointment);
    this.selectedAppointment = new AppointmentClass();
    this.display = false;
    this.userform.markAsPristine();
  }

  onRowSelect() {
    this.isNewAppointment = false;
    this.cloneAppointment = this.cloneRecord(this.selectedAppointment);
    this.selectedAppointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();

    this.selectedContact = this.contactList.find(x => x.contactId === this.selectedAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(x => x.employeeId === this.selectedAppointment.hostId);
  }

  cloneRecord(r: Appointment): Appointment {
    // tslint:disable-next-line:no-shadowed-variable
    const Appointment = new AppointmentClass();
    // tslint:disable-next-line:forin
    for (const prop in r) {
      Appointment[prop] = r[prop];
    }
    return Appointment;
  }

  getFullName() {
    if (this.ctr === 0) {
      for (let i = 0; i < this.contactList.length; i++) {
        this.contactList[i].fullName = this.contactList[i].firstName + ' ' + this.contactList[i].lastName;
      }

      for (let i = 0; i < this.employeeList.length; i++) {
        this.employeeList[i].fullName = this.employeeList[i].firstName + ' ' + this.employeeList[i].lastName;
      }
    }

    for (let i = 0; i < this.appointmentList.length; i++) {
      this.appointmentList[i].hostName = this.employeeList.find(x => x.employeeId === this.appointmentList[i].hostId).fullName;
      this.appointmentList[i].guestName = this.contactList.find(x => x.contactId === this.appointmentList[i].guestId).fullName;
      this.appointmentList[i].appointmentDate = new Date(this.appointmentList[i].appointmentDate).toLocaleDateString();
    }

    this.ctr++;
  }
}
