import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { ConfirmationService } from 'primeng/primeng';

import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [EmployeeService, ConfirmationService]
})
export class EmployeesComponent implements OnInit {

  display: boolean = false;
  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  isDeleteEmployee: boolean;
  globalIndex: number;

  constructor(private _httpClient: HttpClient, private employeeService: EmployeeService
    , private _confirmationService: ConfirmationService) { }

  ngOnInit() {
    //this.employeeService.getEmployees().then(employees =>this.employeeList=employees);

    //get employees
    this.getEmployees();
    //this.selectedEmployee = this.employeeList[0];

  }

  onRowSelect(employee) {
    this.isNewEmployee = false;
    this.isDeleteEmployee = false;
    //this.display = true;
    this.showDisplay();
    //this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.globalIndex = this.employeeList.indexOf(this.selectedEmployee);
    this.selectedEmployee = Object.assign({}, employee);
    this.cloneEmployee = Object.assign({}, this.selectedEmployee);
    console.log("Selected Employee on Row Select");
    console.log(employee);
  }

  getEmployees() {
    this.employeeService._getEmployees().then(e => { console.log(e); this.employeeList = e; });
  }

  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
    this.showDisplay();
  }

  saveEmployee(isSaveAndNew: boolean) {
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {

      this.employeeService._addEmployee(this.selectedEmployee)
        .then(employees => {

          tmpEmployeeList.push(employees["result"]);
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = isSaveAndNew ? new EmployeeClass() : null;
          this.isNewEmployee = isSaveAndNew ? true : false;
          console.log("emp: " + JSON.stringify(employees));
        });
      this.display = isSaveAndNew;
    }
    else {
      ////removed b'coz we create a global index for selected employee
      //tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
      tmpEmployeeList[this.globalIndex] = this.selectedEmployee;
      this.employeeService._saveEmployee(this.selectedEmployee)
        .then(employees => {
          tmpEmployeeList[this.globalIndex] = this.selectedEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = null;
          this.isNewEmployee = isSaveAndNew ? true : false;
        });
      this.display = isSaveAndNew;
    }

    //this.selectedEmployee = null;
  }

  // cancelEmployee() {
  //   if (this.isNewEmployee) {
  //     this.selectedEmployee = null;
  //   }
  //   else {
  //     this.isNewEmployee = false;
  //     let tmpEmployeeList = [...this.employeeList];
  //     tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
  //     this.employeeList = tmpEmployeeList;
  //     this.selectedEmployee = Object.assign({}, this.cloneEmployee);
  //   }
  //   this.display = false;
  // }

  //removed because of Object.assign({}, object) clonning object in angular
  cloneRecord(r: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }

  // removed b'coz of global indez
  findSelectedEmployeeIndex(): number {
    return this.employeeList.indexOf(this.selectedEmployee);
  }

  deleteEmployee(employee) {
    this.showDisplay();
    this.selectedEmployee = employee;
    this.globalIndex = this.employeeList.indexOf(employee);
    this.isDeleteEmployee = true;


    // let index = this.globalIndex;
    // this.employeeService._deleteEmployee(this.selectedEmployee.id);
    // this.employeeList = this.employeeList.filter((val, i) => i != index);
    // this.selectedEmployee = null;
  }

  showDisplay() {
    this.display = true;
  }

  confirmationDelete() {
    console.log("Confirm Delete");
    console.log(this.selectedEmployee);

    this._confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Information',
      icon: 'fa fa-trash',
      accept: () => {
        let index = this.globalIndex;
        this.employeeService._deleteEmployee(this.selectedEmployee.id);
        this.employeeList = this.employeeList.filter((val, i) => i != index);
        this.selectedEmployee = null;
        this.isNewEmployee = false;
        this.isDeleteEmployee = true;
        this.display = false;
      }
    });
  }


  confirmationCancel() {

    if (this.isNewEmployee) {
      this.selectedEmployee = null;
      this.isDeleteEmployee = false;
      this.isNewEmployee = false
    }
    else {
      this.isNewEmployee = false;
      this._confirmationService.confirm({
        message: 'Are you sure you want to discard the changes?',
        header: 'Cancel Confirmation',
        icon: 'fa fa-ban',
        accept: () => {
          let tmpEmployeeList = [...this.employeeList];
          tmpEmployeeList[this.globalIndex] = this.cloneEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = Object.assign({}, this.cloneEmployee);
        }
      });
    }

    this.display = false;
  }

}
