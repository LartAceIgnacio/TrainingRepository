import { Component, OnInit } from '@angular/core';
import {EmployeeService} from '../services/employeeservices';
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

  constructor(private employeeService: EmployeeService) { }

  ngOnInit() {
    this.employeeService.getEmployees().then(employees =>this.employeeList=employees);
    //this.selectedEmployee = this.employeeList[0];
  }

  addEmployee(){
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  saveEmployee(){
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee)
      tmpEmployeeList.push(this.selectedEmployee);
    else
      tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;

    this.employeeList=tmpEmployeeList;
    this.selectedEmployee=null;
    this.isNewEmployee = false;
  }

  cancelEmployee(){
    this.isNewEmployee = false;
    let tmpEmployeeList = [...this.employeeList];
    var temp = this.cloneEmployee;
    this.onRowSelect();
    tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = temp;
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee =this.cloneEmployee;
    
  }

  onRowSelect(){
    this.isNewEmployee = false;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  }

  cloneRecord(r: Employee): Employee{
    let employee = new EmployeeClass();
    for(let prop in r){
      employee[prop] = r[prop];
    }
    return employee;
  }

  findSelectedEmployeeIndex(): number {
    return this.employeeList.indexOf(this.selectedEmployee);
  }

  deleteEmployee(){
    let index = this.findSelectedEmployeeIndex();
    this.employeeList = this.employeeList.filter((val,i) => i!=index);
    this.selectedEmployee = null;
  }
}
