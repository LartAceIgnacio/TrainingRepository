import { Component, OnInit } from '@angular/core';
import { EmployeeService } from './../services/employeeService';
import { Employee } from './../domain/employee';
import { EmployeeClass } from './../domain/employeeclass';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem, ConfirmationService, DataTable } from "primeng/primeng";
import { ViewChild } from '@angular/core';
import { Pagination } from "../domain/pagination";
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [EmployeeService, ConfirmationService,
    GlobalService]
})
export class EmployeesComponent implements OnInit {

  employeeItems: MenuItem[];
  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  clonedSelectedEmployee: Employee;
  indexSelected: number;

  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  displayDialog: boolean;
  loading: boolean;
  employee: Employee = new EmployeeClass();

  indexOfEmployee: number;
  totalRecords: number = 0;
  searchFilter: string = "";
  paginationResult: Pagination<Employee>;

  btnSaveNew: boolean;
  btnDelete: boolean;
  btnSave: boolean;

  regexEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
  numberFormat: string = '^\\d+$';

  display: boolean = false;

  showDialog() {
    this.display = true;
  }

  constructor(private employeeService: EmployeeService, private globalService: GlobalService,
    private fb: FormBuilder, private confirmationService: ConfirmationService) { }
  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    // this.loading = true;
    // setTimeout(() => {
    //   this.employeeService.getEmployees().then(employees => this.employeeList = employees);
    //   this.loading = false;
    // }, 1000);
    //this.selectedEmployee = this.employeeList[0];

    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberFormat)])),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexEmailFormat)])),
      'photo': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberFormat)])),
      'extension': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.numberFormat)])),
    });

    this.employeeItems = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Employees', routerLink: ['/employees'] }
    ]
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Employee>>("Employees", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.employeeList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
      });
  }

  searchEmployee() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    let paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }

  addEmployees() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass;
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  saveEmployees() {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];

        let tmpEmployeeList = [...this.employeeList];
        if (this.isNewEmployee) {
          this.employeeService.addEmployees(this.selectedEmployee).then(employees => {
          this.employee = employees;
            tmpEmployeeList.push(this.employee);
            this.employeeList = tmpEmployeeList;
          });
        } else {
          this.employeeService.saveEmployees(this.selectedEmployee);
          tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
        }
        //this.employeeService.saveEmployees(this.selectedEmployee);
        this.selectedEmployee = null;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
    this.userform.markAsPristine();
  }

  saveNewEmployees() {
    this.userform.enable();
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.employeeService.addEmployees(this.selectedEmployee).then(employees => {
      this.employee = employees;
        tmpEmployeeList.push(this.employee);
        this.employeeList = tmpEmployeeList;
      });
    } else {
      this.employeeService.saveEmployees(this.selectedEmployee);
      tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
    }
    //this.employeeService.saveEmployees(this.selectedEmployee);
    this.selectedEmployee = new EmployeeClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Employee: Employee) {
    this.userform.disable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedEmployee = Employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  }

  deleteEmployees() {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];
        this.selectedEmployee;
        let index = this.findSelectedEmployeeIndex();
        this.employeeList = this.employeeList.filter((val, i) => i != index);
        this.employeeService.deleteEmployees(this.selectedEmployee.employeeId);
        this.selectedEmployee = null;
        this.displayDialog = false;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
  }

  editEmployees(Employee: Employee) {
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewEmployee = false;
    this.selectedEmployee = Employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  // onRowSelect(event) {
  //         this.isNewEmployee = false;
  //         this.selectedEmployee;
  //         this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  //         this.displayDialog = true;
  // } 

  cloneRecord(r: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }

  cancelEmployees() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];

        this.isNewEmployee = false;
        let tmpEmployeeList = [...this.employeeList];
        tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = this.cloneEmployee;
        this.selectedEmployee = null;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
  }

  findSelectedEmployeeIndex(): number {
    return this.employeeList.indexOf(this.selectedEmployee);
  }
}