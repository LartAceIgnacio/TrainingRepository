import { Component, OnInit } from '@angular/core';
import { GlobalService } from "../services/globalService";
import {Employee} from "../domain/employee";
import {EmployeeClass} from "../domain/employeeClass";
import { HttpClient } from '@angular/common/http';
import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';
import { PaginationResult } from "../domain/paginationresult";
import { API_URL } from "../services/constants";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css'],
  providers: [GlobalService]
})

export class EmployeeComponent implements OnInit {
  //contact variable
  employeeList: Employee[];
  selectedEmployee:Employee;
  cloneEmployee:Employee;
  isNewEmployee:boolean;
  //regex  
  regexNumberFormat: string = "^[0-9]*$";
  regexEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
  //url 
  service: string = 'employees';
  serviceUrl: string = `${API_URL}/${this.service}/`;
  //form
  employeeForm: FormGroup;  
  dialogTitle: String;
  //displayable
  displayInputBox: boolean = false;
  onEdit: boolean = false;
  onDelete: boolean = false;
  onAdd: boolean = false;
  //confirmation
  confirmEditCancel: boolean = false;
  confirmDeleteRecord: boolean = false;
  //pagination
  searchFilter: string = "";
  totalRecords: number = 0;
  searchButtonClickCtr: number = 0;
  paginationResult: PaginationResult<Employee>;

  constructor(private globalService : GlobalService, private formbuilder: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    //this.globalService.retrieve(this.path).then(employees => this.employeeList = employees);
        
    this.employeeForm = this.formbuilder.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexNumberFormat)])),
      'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexEmailFormat)])),
      'officePhone': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexNumberFormat)])),
      'extension': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexNumberFormat)])),      
      });
  }

  paginate(event) {
    this.globalService.retrieveWithPagination<PaginationResult<Employee>>(this.serviceUrl, event.first, event.rows,
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
    // let paging = {
    //   first: ((n - 1) * this.dataTable.rows),
    //   rows: this.dataTable.rows
    // };
    //this.dataTable.paginate();
  }

  //--------------------CREATE-----------------------
  addEmployee(){
      this.isNewEmployee = true;
      this.selectedEmployee = new EmployeeClass();
      //form
      this.displayForm("New Employee", true);
      //displayable      
      this.onEdit= false;
      this.onDelete = false;
      this.onAdd = true;
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = false;
  }
  //--------------------EDIT-----------------------
  editEmployee(employee: Employee){    
    this.isNewEmployee = false;
    this.selectedEmployee = employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    //form
    this.displayForm("Edit Employee", true);
    //displayable
    this.onEdit = true;
    this.onDelete = false;
    this.onAdd = false;
  }
  //--------------------DELETE-----------------------
  deleteEmployee(employee: Employee, rowSelect: boolean){
    //displayable      
    this.onEdit= false;
    this.onDelete = true;
    this.onAdd = false;

    if(rowSelect){
      this.selectedEmployee = employee;
      //form
      this.displayForm("Delete Employee", false);
    }
    else{
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = true;
    }
  }
  //--------------------FORM-----------------------
  saveEmployee(withNew: boolean){
    let tempEmployeeList = [...this.employeeList];
    if(this.isNewEmployee){
      this.globalService.add(this.serviceUrl,this.selectedEmployee).then(employee => {
        tempEmployeeList.push(employee);
        this.employeeList = tempEmployeeList;
        this.selectedEmployee = null;
        if(withNew)
          this.addEmployee();
        })
    }
    else{      
      this.globalService.update(this.serviceUrl, this.selectedEmployee, this.selectedEmployee.employeeId).then(employee => {
        tempEmployeeList[this.employeeList.indexOf(this.selectedEmployee)];
        this.employeeList = tempEmployeeList;
        this.selectedEmployee = null;
        this.displayInputBox = false;
        })
    }
    this.isNewEmployee = false;
  }

  cancelEmployee(){
    if(this.onAdd){
      this.toCancel(true);
    }
    else if(this.onEdit){
      if(this.employeeForm.dirty){  
        this.confirmEditCancel = true;
      }
      if(this.employeeForm.pristine){
        this.displayInputBox = false;
      }
    }
    else if(this.onDelete){
      this.confirmDeleteRecord = false;
      this.displayInputBox = false;
    }    
    else{
      this.displayInputBox = false;
    }
  }
  //--------------------CONFIRMATION-----------------------
  toCancel(discard:boolean){
    if(discard) {
      if(this.onEdit){
        this.isNewEmployee = false;
        let tempEmployeeList = [...this.employeeList];
        tempEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
        this.employeeList = tempEmployeeList;
        this.selectedEmployee = this.cloneEmployee; 
        this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
        this.employeeForm.markAsPristine();
      }
      else{
          //displayable
          this.displayInputBox = false;
        }      
      }
    this.confirmEditCancel = false;
  }

  toDelete(toDelete: boolean){
    if(toDelete){    
      this.displayInputBox = true;
      this.globalService.delete(this.serviceUrl, this.selectedEmployee, this.selectedEmployee.employeeId).then(employee => {
      let tempEmployeeList = [...this.employeeList];
      tempEmployeeList.splice(this.employeeList.indexOf(this.selectedEmployee), 1);   
      this.employeeList = tempEmployeeList;
      this.selectedEmployee = null;
      })
    }   
    this.displayInputBox = false; 
    this.confirmDeleteRecord = false;
  }  
  //--------------------MISC-----------------------
  displayForm(title: string, editable: boolean){
    this.dialogTitle = title;
    this.employeeForm.markAsPristine();
    if(editable){
      this.employeeForm.enable();
    }
    else{
      this.employeeForm.disable();
    }
    this.displayInputBox = true;
  }

  cloneRecord(r: Employee): Employee {
    let employee = new EmployeeClass();
    for (let prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }
}

class PaginationResultClass implements PaginationResult<Employee>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}