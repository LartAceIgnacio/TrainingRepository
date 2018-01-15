import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../services/appointmentservice';
import { Appointment } from '../domain/appointments/appointment';
import { AppointmentClass } from '../domain/appointments/appointmentclass';
import { EmployeeService } from '../services/employeeservice';
import { ContactService } from '../services/contactservice';
import { Employee } from '../domain/employees/employee';
import { Contact } from '../domain/contacts/contact';
import { EmployeeClass } from '../domain/employees/employeeclass';
import { ContactClass } from '../domain/contacts/contactclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { SelectItem } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';


@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class AppointmentsComponent implements OnInit {

  items: MenuItem[];

  types: SelectItem[];

  home: MenuItem;

  displayDialog: boolean;

  appointment: Appointment = new AppointmentClass();

  selectedAppointment: Appointment;

  newAppointment: boolean;

  appointments: Appointment[];

  loading: boolean;

  employeeList: Employee[];

  selectedEmployee: Employee;

  selectedContact: Contact;

  contactList: Contact[];

  selectedAppointmentDate: Date;

  isDelete: boolean;

  isEdit: boolean;

  isNewAppointment: boolean;

  userform: FormGroup;

  dateFilter: Date[];

  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';

  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  paginationResult: PaginationResult<Appointment>;

  // tslint:disable-next-line:no-inferrable-types
  ctr: number = 0;

  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) {

    this.types = [
      { label: 'True', value: true },
      { label: 'False', value: false }

    ];

  }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.loading = true;
    setTimeout(() => {

      this.loading = false;
    }, 1000);

    this.items = [
      { label: 'Home', routerLink: ['/dashboard'] },
      { label: 'Appointment' }
    ];

    this.home = { icon: 'fa fa-home' };

    this.userform = this.fb.group({
      'appointmnetdate': new FormControl('', Validators.required),
      'guestname': new FormControl('', Validators.required),
      'hostname': new FormControl('', Validators.required),
      'starttime': new FormControl('', Validators.required),
      'endtime': new FormControl('', Validators.required),
      'iscancelled': new FormControl(''),
      'isdone': new FormControl(''),
      'notes': new FormControl('')
    });
  }

  showDialogToAdd() {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = true;
    this.isDelete = false;
    this.selectedEmployee = null;
    this.selectedContact = null;
    this.selectedAppointment = null;
    this.newAppointment = true;
    this.appointment = new AppointmentClass();
    this.displayDialog = true;
  }

  onRowSelect(clickAppointment) {
    this.newAppointment = false;
    this.appointment = this.cloneAppointment(clickAppointment);
    this.selectedAppointment = clickAppointment;
    this.displayDialog = true;

    this.appointment.appointmentDate = new Date(this.selectedAppointment.appointmentDate).toLocaleDateString();
    this.selectedContact = this.contactList.find(x => x.contactId === this.selectedAppointment.guestId);
    this.selectedEmployee = this.employeeList.find(x => x.employeeId === this.selectedAppointment.hostId);
  }

  cloneAppointment(a: Appointment): Appointment {
    const appointment = new AppointmentClass();
    // tslint:disable-next-line:forin
    for (const prop in a) {
      appointment[prop] = a[prop];
    }
    return appointment;
  }

  save(number: boolean) {
    this.isNewAppointment = number;

    const appointments = [...this.appointments];
    this.appointment.guestId = this.selectedContact.contactId;
    this.appointment.hostId = this.selectedEmployee.employeeId;

    if (this.newAppointment) {
      this.globalService.addSomething('Appointments', this.appointment);
      this.appointment.guestName = this.contactList.find(id => id.contactId === this.appointment.guestId).firstName;
      this.appointment.hostName = this.employeeList.find(id => id.employeeId === this.appointment.hostId).firstName;
      appointments.push(this.appointment);
    } else {
      this.appointment.guestId = this.selectedContact.contactId;
      this.appointment.hostId = this.selectedEmployee.employeeId;
      this.globalService.updateSomething('Appointments', this.appointment.appointmentId, this.appointment);
      this.appointment.guestName = this.contactList.find(id => id.contactId === this.appointment.guestId).firstName;
      this.appointment.hostName = this.employeeList.find(id => id.employeeId === this.appointment.hostId).firstName;
      appointments[this.findSelectedAppointmentIndex()] = this.appointment;
    }

    if (this.isNewAppointment) {
      this.appointments = appointments;
      this.newAppointment = true;
      this.selectedAppointment = null;
      this.appointment = new AppointmentClass;
    } else {
      this.appointments = appointments;
      this.appointment = null;
      this.displayDialog = false;
      // this.setCurrentPage(1);
    }

  }

  findSelectedAppointmentIndex(): number {
    return this.appointments.indexOf(this.selectedAppointment);
  }

  delete(clickContact: Contact) {
    this.userform.disable();
    this.isDelete = true;
    this.onRowSelect(clickContact);

  }

  deleteAppointment() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        this.globalService.deleteSomething('Appointments', this.selectedAppointment.appointmentId);
        const index = this.findSelectedAppointmentIndex();
        this.appointments = this.appointments.filter((val, i) => i !== index);
        this.appointment = null;
        this.displayDialog = false;
      }
    });
  }

  edit(clickAppointment: Appointment) {
    this.userform.enable();
    this.isEdit = false;
    this.isDelete = false;
    this.onRowSelect(clickAppointment);
  }

  cancel() {
    if (this.isDelete) {
      this.displayDialog = false;
    } else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to cancel?',
        accept: () => {
          this.appointment = Object.assign({}, this.selectedAppointment);
          this.selectedContact = this.contactList.find(x => x.contactId === this.appointment.guestId);
          this.selectedEmployee = this.employeeList.find(x => x.employeeId === this.appointment.hostId);

        }
      });
    }
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
              this.appointments = this.paginationResult.results;
              this.totalRecords = this.paginationResult.totalRecords;
              this.getFullName();
            });
        });
      });
    }else {
      this.globalService.getSomethingWithPagination<PaginationResult<Appointment>>('Appointments', event.first, event.rows,
        this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.appointments = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
          this.getFullName();
        });
    }
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
    // tslint:disable-next-line:prefer-const
    let paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }

  getFullName() {
    if (this.ctr === 0) {
      // tslint:disable-next-line:no-shadowed-variable
      for (let i = 0; i < this.contactList.length; i++) {
        this.contactList[i].fullName = this.contactList[i].firstName + ' ' + this.contactList[i].lastName;
      }

      // tslint:disable-next-line:no-shadowed-variable
      for (let i = 0; i < this.employeeList.length; i++) {
        this.employeeList[i].fullName = this.employeeList[i].firstName + ' ' + this.employeeList[i].lastName;
      }
    }

    for (let i = 0; i < this.appointments.length; i++) {
      this.appointments[i].hostName = this.employeeList.find(x => x.employeeId === this.appointments[i].hostId).fullName;
      this.appointments[i].guestName = this.contactList.find(x => x.contactId === this.appointments[i].guestId).fullName;
      this.appointments[i].appointmentDate = new Date(this.appointments[i].appointmentDate).toLocaleDateString();
    }

    this.ctr++;
  }
}

class PaginationResultClass implements PaginationResult<Contact> {
  constructor (public results, public pageNo, public recordPage, public totalRecords) {

  }
}
