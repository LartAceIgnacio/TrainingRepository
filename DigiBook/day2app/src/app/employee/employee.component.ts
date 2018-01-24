import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';
import { AuthService } from "../services/auth.service";
import { GlobalService } from "../services/globalservice";
import { Pagination } from "../domain/pagination";
import { DataTable } from "primeng/components/datatable/datatable";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class EmployeeComponent implements OnInit {

  employee: Employee = new EmployeeClass();
  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;

  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  items: MenuItem[];

  home: MenuItem;

  description: string;

  display: boolean = false;

  constructor(private globalService: GlobalService, private fb: FormBuilder, private conf: ConfirmationService
  ,public auth: AuthService) { }

  newEmployee() {
    this.display = true
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  editEmployee(employee: Employee) {
    this.display = true;
    this.selectedEmployee = employee;
    this.isNewEmployee = false;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  }
  deleteEmp(employee: Employee) {
    this.selectedEmployee = employee;
    this.conf.confirm({
      message: 'Are you sure that you want to delete this data?',
      accept: () => {
        if (this.selectedEmployee.employeeId == null)
          this.selectedEmployee = new EmployeeClass();
        else {
          this.globalService.deleteSomething<Employee>("employees",this.selectedEmployee.employeeId)
          let index = this.employeeList.indexOf(this.selectedEmployee);
          this.employeeList = this.employeeList.filter((val, i) => i != index);
          this.employee = null;
        }
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Employee Deleted!' });
        this.selectedEmployee = new EmployeeClass();
      }
    });

  }
  saveAndNewEmployee() {
    let tmpEmployeeList = [...this.employeeList];
    this.globalService.addSomething<Employee>("employees",this.selectedEmployee).then(emp => {
      tmpEmployeeList.push(emp);
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = new EmployeeClass();
    });
    this.userform.markAsPristine();
    this.msgs = [];
    this.msgs.push({ severity: 'success', summary: 'Employee Saved!' });
  }
  ngOnInit() {
    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.required),
      'phone': new FormControl('', Validators.required),
      'officephone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required)
    });

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Employee', routerLink: ['/employees'] }
    ];

    this.home = { icon: 'fa fa-home' };

  }
  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }
  saveEmployee(value: string) {
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.globalService.addSomething<Employee>("employees",this.selectedEmployee).then(emp => {
        tmpEmployeeList.push(emp);
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = null;
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Employee Added!' });
      });
    }

    else {
      this.globalService.updateSomething("employees", this.selectedEmployee.employeeId,this.selectedEmployee)
      tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
      this.msgs = [];
      this.msgs.push({ severity: 'success', summary: 'Employee Saved!' });
    }
    this.userform.markAsPristine();
    this.isNewEmployee = false; 

    this.display = false;
  }
  onRowSelect() {
    this.isNewEmployee = false;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  }
  cloneRecord(r: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }
  cancelEmployee() {
    this.isNewEmployee = false;
    let tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee = this.cloneEmployee;
    this.selectedEmployee = new EmployeeClass();
    this.display = false;
    this.userform.markAsPristine();
  }
  cancelEmp() {
    if (this.isNewEmployee == true) {
      this.display = false;
    }
    else {
      if (this.userform.dirty) {
        this.conf.confirm({
          message: 'Do you want to discard changes?',
          accept: () => {
            this.cancelEmployee()
          }
        });
      }
      else {
        this.display = false;
      }
    }
    this.userform.markAsPristine();
  }
  deleteEmployee() {
    if (this.selectedEmployee.employeeId == null)
      this.selectedEmployee = new EmployeeClass();
    else {
      this.globalService.deleteSomething<Employee>("employees",this.selectedEmployee.employeeId)
      let index = this.employeeList.indexOf(this.selectedEmployee);
      this.employeeList = this.employeeList.filter((val, i) => i != index);
      this.employee = null;
    }
    this.selectedEmployee = new EmployeeClass();
  }

  entity: string = "employees";
  searchQuery: string = "";
  totalRecord: number;

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Employee>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.employeeList = result.result;
        this.totalRecord = result.totalCount;
      });
  }

  @ViewChild('dt') public dataTable: DataTable;
  search() {
    this.dataTable.reset();
  }
}
