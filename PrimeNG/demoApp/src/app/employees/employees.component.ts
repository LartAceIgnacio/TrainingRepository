import { Component, OnInit, ViewChild } from '@angular/core';
import {EmployeeService} from '../services/EmployeeService';
import {Employee} from '../domain/Employee';
import {EmployeeClass} from '../domain/EmployeeClass';

import {BreadcrumbModule,MenuItem} from 'primeng/primeng';
import {DialogModule} from 'primeng/primeng';
import { ConfirmDialogModule, ConfirmationService, Message } from 'primeng/primeng';

import {HttpClient} from '@angular/common/http';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import { PaginationResult } from "../domain/paginationresult";
import { GlobalService } from "../services/globalservice";
import { DataTable } from "primeng/components/datatable/datatable";

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers:[EmployeeService, ConfirmationService, GlobalService]
})
export class EmployeesComponent implements OnInit {

  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  tempSelectedEmployee: Employee;
  userform: FormGroup;  
  brEmployee: MenuItem[];
  home: MenuItem;
  display: boolean;
  msgs: Message[] = [];
  isDelete: boolean = false;

  // Pagination
  searchFilter: string = "";
  totalRecords: number = 0;
  searchButtonClickCtr: number = 0;
  paginationResult: PaginationResult<Employee>;
  // end pagination

  constructor(private employeeService: EmployeeService,
    private http:HttpClient,
    private fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private globalService: GlobalService) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    //this.employeeService.getEmployees().then(employees =>this.employeeList=employees);

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required)
    });

    this.brEmployee=[
      {label: 'Employees', url: '/employees'}
    ]
    this.home = {icon: 'fa fa-home', routerLink: '/dashboard'};
  }

  searchEmployee() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
  }

  addEmployee(){
    this.isDelete = false;
    this.display = true;
    this.userform.markAsPristine();
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  saveEmployee(){
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee){
      this.employeeService.postEmployees(this.selectedEmployee).then(employee => {
        tmpEmployeeList.push(employee);
        this.employeeList=tmpEmployeeList;
        this.selectedEmployee=null;
      });
    }    
    else{
      this.employeeService.putEmployees(this.selectedEmployee).then(employee =>{
        tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
        this.employeeList=tmpEmployeeList;
        this.selectedEmployee=null;
      });
    }
    this.isNewEmployee = false;
  }

  saveAndNewEmployee(){
    this.userform.markAsPristine();

    let tmpEmployeeList = [...this.employeeList];

    this.employeeService.postEmployees(this.selectedEmployee).then(employee => {
      tmpEmployeeList.push(employee);
      this.employeeList=tmpEmployeeList;
    });

    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  cancelEmployee(){
    let index = this.findSelectedEmployeeIndex();
    if (this.isNewEmployee){
        this.selectedEmployee = null;
    }
    else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want discard changes?',
        header: 'Confirmation',
        icon: 'fa fa-question-circle',
        accept: () => {
          let tmpEmployeeList = [...this.employeeList];
          tmpEmployeeList[index] = this.cloneEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = Object.assign({}, this.cloneEmployee);
          this.display = false;
        }
    });
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
    
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let index = this.findSelectedEmployeeIndex();
        this.employeeList = this.employeeList.filter((val,i) => i!=index);
        this.employeeService.deleteEmployees(this.selectedEmployee.employeeId);
        this.selectedEmployee = null;
      }
    });
  }

  editEmployee(Employee: Employee){
    this.isDelete = false;
    this.selectedEmployee=Employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.display=true;
    this.isNewEmployee = false;
  }

  confirmDelete(Employee: Employee){
    this.userform.markAsPristine();
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.selectedEmployee=Employee;
    this.isDelete = true;
    this.display=true;
    this.userform.disable();
    this.isNewEmployee = false;
  }

  // Pagination
  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Employee>>("Employees", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.employeeList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
        });
  }
  // end pagination

}
