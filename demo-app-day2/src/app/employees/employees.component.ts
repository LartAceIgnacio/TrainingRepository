import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {EmployeeService} from '../services/employee.service';
import {Employee} from '../domain/employee';
import {EmployeeClass} from '../domain/employeeclass';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers:[EmployeeService]
})
export class EmployeesComponent implements OnInit {

  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  globalIndex: number;

  constructor(private _httpClient: HttpClient, private employeeService: EmployeeService) { }

  ngOnInit() {
    //this.employeeService.getEmployees().then(employees =>this.employeeList=employees);

    //get employees
    this.getEmployees();
    //this.selectedEmployee = this.employeeList[0];
    
  }

  onRowSelect(){
    this.isNewEmployee = false;
    //this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.globalIndex = this.employeeList.indexOf(this.selectedEmployee);
    this.selectedEmployee = Object.assign({}, this.selectedEmployee);
    this.cloneEmployee = Object.assign({}, this.selectedEmployee );
  }

  getEmployees()
  {
    this.employeeService._getEmployees().then(e => {console.log(e); this.employeeList=e;});
  }

  addEmployee(){
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  saveEmployee(){
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee)
    {
      tmpEmployeeList.push(this.selectedEmployee);
      this.employeeService._addEmployee(this.selectedEmployee);
    }
    else
    {
      ////removed b'coz we create a global index for selected employee
      //tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
      tmpEmployeeList[this.globalIndex] = this.selectedEmployee;
      this.employeeService._saveEmployee(this.selectedEmployee);
    }

    this.employeeList=tmpEmployeeList;
    this.selectedEmployee=null;
    this.isNewEmployee = false;

    console.log("------------ Retrieving Information ------------");
    alert("------------ Retrieving Information ------------");
    this.getEmployees();
  }

  cancelEmployee(){
    this.isNewEmployee = false;
    let tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee =  Object.assign({}, this.cloneEmployee );
  }

  //removed because of Object.assign({}, object) clonning object in angular
  cloneRecord(r: Employee): Employee{
    let employee = new EmployeeClass();
    for(let prop in r){
      employee[prop] = r[prop];
    }
    return employee;
  }

  // removed b'coz of global indez
  findSelectedEmployeeIndex(): number {
    return this.employeeList.indexOf(this.selectedEmployee);
  }

  deleteEmployee(){
    let index = this.globalIndex;
    this.employeeService._deleteEmployee(this.selectedEmployee.id);
    this.employeeList = this.employeeList.filter((val,i) => i!=index);
    this.selectedEmployee = null;
  }

}
