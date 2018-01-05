import { Component, OnInit } from '@angular/core';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GlobalService]
})

export class EmployeesComponent implements OnInit {
  employeeList: Employee[];
  cloneEmployee: Employee;
  selectedEmployee: Employee;
  isNewEmployee: boolean;
  indexOfEmployee: number;
  userform: FormGroup;
  rexExpEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

  constructor(private globalService: GlobalService, private fb: FormBuilder) { }

  ngOnInit() {
    this.globalService.getSomething<Employee>("Employees").then(employees => this.employeeList = employees);
    //this.selectedEmployee = this.employeeList[0]

    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.rexExpEmailFormat)])),
      'extension': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.required)
    });
  }

  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  onRowSelect(event) {
    this.isNewEmployee = false;
    this.indexOfEmployee = this.employeeList.indexOf(this.selectedEmployee);
    this.cloneEmployee = Object.assign({}, this.selectedEmployee);
    this.selectedEmployee = Object.assign({}, this.selectedEmployee);
    //this.cloneEmployee = this.cloneEmployees(event.data);
  }

  btnSave() {
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.globalService.addSomething<Employee>("Employees", this.selectedEmployee).then(employee => {
        tmpEmployeeList.push(employee);
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = null;
      });
    }
    else {
      this.globalService.updateSomething<Employee>("Employees", this.selectedEmployee.employeeId, this.selectedEmployee).then(employees => {
        tmpEmployeeList[this.indexOfEmployee] = this.selectedEmployee;
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = null;
      });
    }
    this.isNewEmployee = false;
  }

  btnCancel() {
    if (this.isNewEmployee)
      this.selectedEmployee = null;
    else {
      let tmpEmployeeList = [...this.employeeList];
      tmpEmployeeList[this.indexOfEmployee] = this.cloneEmployee;
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = Object.assign({}, this.cloneEmployee);
      //this.cloneEmployee = this.cloneEmployees(this.selectedEmployee);
    }
  }

  btnDelete() {
    let tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList.splice(this.indexOfEmployee, 1);
    this.globalService.deleteSomething<Employee>("Employees", this.selectedEmployee.employeeId).then(employees => {
      tmpEmployeeList.splice(this.indexOfEmployee, 1);
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = null;
      this.isNewEmployee = false;
    });
  }

  cloneEmployees(c: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in c) {
      employee[prop] = c[prop];
    }
    return employee;
  }
}
