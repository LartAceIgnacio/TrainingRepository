import { Component, OnInit } from '@angular/core';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { PaginationResult } from "../domain/paginationresult";

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GlobalService, ConfirmationService]
})

export class EmployeesComponent implements OnInit {
  employeeList: Employee[];
  cloneEmployee: Employee;
  selectedEmployee: Employee;
  isNewEmployee: boolean;
  isDeleteEmployee: boolean = false;
  indexOfEmployee: number;
  userform: FormGroup;
  searchFilter: string = "";
  totalRecords: number = 0;
  searchButtonClickCtr: number = 0;
  paginationResult: PaginationResult<Employee>;
  rexExpEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.rexExpEmailFormat)])),
      'extension': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.required)
    });
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Employee>>("Employees", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.employeeList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
        });
  }

  searchEmployee() {
    this.dataTable.reset();
  }

  addEmployee() {
    this.userform.enable();
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  onRowSelect(employee) {
    this.userform.enable();
    this.isNewEmployee = false;
    this.selectedEmployee = employee;
    this.indexOfEmployee = this.employeeList.indexOf(this.selectedEmployee);
    this.cloneEmployee = Object.assign({}, this.selectedEmployee);
    this.selectedEmployee = Object.assign({}, this.selectedEmployee);
  }

  btnSave(isSaveAndNew: boolean) {
    this.userform.markAsPristine();
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.globalService.addSomething<Employee>("Employees", this.selectedEmployee).then(employees => {
        tmpEmployeeList.push(employees);
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = isSaveAndNew ? new EmployeeClass() : null;
        this.isNewEmployee = isSaveAndNew ? true : false;
        if (!isSaveAndNew) {
          this.dataTable.reset();
        }
      });
    }
    else {
      this.globalService.updateSomething<Employee>("Employees", this.selectedEmployee.employeeId, this.selectedEmployee).then(employees => {
        tmpEmployeeList[this.indexOfEmployee] = this.selectedEmployee;
        this.employeeList = tmpEmployeeList;
        this.dataTable.reset();
        this.selectedEmployee = null;
        this.isNewEmployee = false;
      });
    }
  }

  btnCancel() {
    if (this.isNewEmployee || this.isDeleteEmployee || !this.userform.dirty) {
      this.selectedEmployee = null;
      this.isDeleteEmployee = false;
      this.isNewEmployee = false
      this.userform.markAsPristine();
    }
    else if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to discard the changes?',
        header: 'Cancel Confirmation',
        icon: 'fa fa-ban',
        accept: () => {
          let tmpEmployeeList = [...this.employeeList];
          tmpEmployeeList[this.indexOfEmployee] = this.cloneEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = Object.assign({}, this.cloneEmployee);
          this.userform.markAsPristine();
        }
      });
    }
  }

  btnDelete(employee) {
    this.userform.disable();
    this.selectedEmployee = employee;
    this.indexOfEmployee = this.employeeList.indexOf(employee);
    this.isDeleteEmployee = true;
  }

  btnOk() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let tmpEmployeeList = [...this.employeeList];
        this.globalService.deleteSomething<Employee>("Employees", this.selectedEmployee.employeeId).then(employees => {
          tmpEmployeeList.splice(this.indexOfEmployee, 1);
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = null;
          this.isNewEmployee = false;
          this.isDeleteEmployee = false;
          this.dataTable.reset();
        });
      }
    });
  }
}

class PaginationResultClass implements PaginationResult<Employee>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}
