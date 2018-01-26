import { Component, OnInit } from '@angular/core';
import { GenericService } from '../services/genericservice';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';

import { Message, SelectItem } from 'primeng/components/common/api';
import { ConfirmationService, MenuItem, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

import { ViewChild } from '@angular/core';
import { Record } from '../domain/record';
import { AuthService } from '../services/auth.service'; 

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GenericService, ConfirmationService]
})
export class EmployeesComponent implements OnInit {
  items : MenuItem[] = [];
  
  isEdit : boolean = false;
  display: boolean = false;
  btnSave: boolean = false;
  btnSaveNew: boolean = false;
  btnDelete: boolean = false;

  msgs: Message[] = [];
  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  employee : Employee;

  //Datatable Components
  searchFilter: string = "";
  totalRecord: number = 0;
  searchButtonClickCtr: number = 0;
  retrieveRecordResult: Record<Employee>;

  service : string = "Employees";
  userform : FormGroup;
  constructor(private genericService: GenericService,
              public auth: AuthService,
              private confirmationService: ConfirmationService,
              private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.items = [
      {label: 'Dashboard', icon: 'fa fa-book fa-5x', routerLink: ['/dashboard']},
      {label: 'Employees', icon: 'fa fa-book fa-5x', routerLink: ['/employees']}
    ]

    this.userform = this.fb.group({
      'firstName' : new FormControl('', Validators.required),
      'lastName' : new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern('^\\d+$')])),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")])),
      'officePhone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required),
    });
  }

  retrieveRecord(event) {
    this.genericService.getPaginatedRecord<Record<Employee>>(this.service, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then( result => {
          this.retrieveRecordResult = result;
          this.employeeList = this.retrieveRecordResult.result;
          this.totalRecord = this.retrieveRecordResult.totalRecord;
        });
  }

  addEmployee(){
    this.userform.markAsPristine();
    this.btnSaveNew = true;
    this.btnSave = true;
    this.btnDelete = false;
    this.display = true;
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
  }

  saveEmployee(saveNew: boolean){
    let tmtEmployeeList = [...this.employeeList];
    this.msgs = [];
    if(this.isNewEmployee)
    {
      this.genericService.insertRecord(this.service, this.selectedEmployee).then(employee => 
        {
        this.employee = employee; 
        tmtEmployeeList.push(this.employee);
        this.employeeList = tmtEmployeeList;
        this.selectedEmployee = saveNew? new EmployeeClass(): null;
        this.isNewEmployee = saveNew ? true : false;
        this.dataTable.reset();
        })
        .then( emp => this.msgs.push({severity:'success', summary:'Success Message', detail:'New Employee: '+ this.employee.lastName +' Added'}));
    }
    else{
      this.genericService.updateRecord(this.service, this.selectedEmployee.employeeId, this.selectedEmployee).then(employee =>
        {this.employee = employee; 
        tmtEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = employee;
        this.employeeList = tmtEmployeeList;
        this.display = false;
        })
        .then( emp => this.msgs.push({severity:'success', summary:'Success Message', detail:' Edited Employee: '+ this.employee.lastName +'Succesfully Edited'}));;
    }
    
    this.userform.markAsPristine();
  }

  search(){
    this.dataTable.reset();
  }

  cancelEmployee(){
    if(this.isNewEmployee ){
      this.selectedEmployee = null;
    }
    else{
      if(this.isEdit)
        {
          if(this.userform.dirty){
            this.confirmationService.confirm({
              message: 'Are you sure you want to discard changes?',
              header: 'Discard Changes',
              icon: 'fa fa-pencil',
              accept: () => {
                let tmpEmployeeList = [...this.employeeList];
                tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
                this.employeeList = tmpEmployeeList;
                this.selectedEmployee = null;
              },
              reject: () => {

              }
            });
            this.userform.markAsPristine();
          }else{
            this.selectedEmployee = null;
          }
        }else{
          this.selectedEmployee = null;
        }
      }
      this.userform.markAsPristine();
    }

  cloneRecord(r: Employee): Employee{
    let employee = new EmployeeClass();
    for(let prop in r){
      employee[prop] = r[prop];
    }
    return employee;
  }

  deleteEmployee(employee: Employee){
    this.userform.markAsPristine();
    this.isEdit = false;
    this.userform.disable();
    this.cloneEmployee = this.cloneRecord(employee);
    this.selectedEmployee = employee;
    this.display = true;
    // hide the Saving button
    this.btnSave = false; 
    this.btnSaveNew = false;     
    
    this.btnDelete = true;
  } 
  
  deleteEmployeeConfirmation(){
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this record?',
      header: 'Discard Changes',
      icon: 'fa fa-pencil',
      accept: () => {
        let tmpEmployeeList = [...this.employeeList];
        this.genericService.deleteRecord(this.service, this.selectedEmployee.employeeId);
        tmpEmployeeList.splice(this.employeeList.indexOf(this.selectedEmployee), 1);
    
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee   = null;
        this.msgs = [{severity:'info', summary:'Confirmed', detail:'Record deleted'}];
      },
      reject: () => {
      }
    });
  }
    
  editEmployee(employee: Employee){
    this.isEdit = true;
    this.userform.enable();
    this.btnSave = true;
    this.btnDelete = false;
    this.btnSaveNew = false;
    this.cloneEmployee =  Object.assign({}, employee);
    this.isNewEmployee = false;
    this.selectedEmployee = employee;
    this.display = true;
  }
}