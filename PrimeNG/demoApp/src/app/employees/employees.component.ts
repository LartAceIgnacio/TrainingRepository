import { Component, OnInit } from '@angular/core';
import {EmployeeService} from '../services/EmployeeService';
import {Employee} from '../domain/Employee';
import {EmployeeClass} from '../domain/EmployeeClass';

import {HttpClient} from '@angular/common/http';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';

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

  userform: FormGroup;  

  constructor(private employeeService: EmployeeService,
    private http:HttpClient,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.employeeService.getEmployees().then(employees =>this.employeeList=employees);

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required)
    });
  }

  addEmployee(){
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  saveEmployee(){
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee){
      tmpEmployeeList.push(this.selectedEmployee);
      this.employeeService.postEmployees(this.selectedEmployee);
    }    
    else{
      tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
      this.employeeService.putEmployees(this.selectedEmployee);
    }

    this.employeeList=tmpEmployeeList;
    this.selectedEmployee=null;
    this.isNewEmployee = false;
  }

  cancelEmployee(){
    let index = this.findSelectedEmployeeIndex();
    if (this.isNewEmployee)
      this.selectedEmployee = null;
    else {
      let tmpEmployeeList = [...this.employeeList];
      tmpEmployeeList[index] = this.cloneEmployee;
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = Object.assign({}, this.cloneEmployee);
    }
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
    this.employeeService.deleteEmployees(this.selectedEmployee.employeeId);
    this.selectedEmployee = null;
  }

}
