import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../domain/employee/employee';
import { EmployeeClass } from '../domain/employee/employee.class';

//validation
import { Message, SelectItem } from 'primeng/components/common/api';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { GenericService } from '../services/common/generic.service';
import { Pagination } from '../domain/common/pagination';

import { invoke } from 'q';

import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { AuthService } from '../services/common/Authentication/auth.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css'],
  providers: [EmployeeService, GenericService]
})
export class EmployeeComponent implements OnInit {

  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;

  // pagination
  entity: string = "Employee";
  searchQuery: string = "";
  pageNumber: number = 0;
  rowCount: number = 5;
  totalRecords: number;
  @ViewChild('dt') public dataTable: DataTable;


  indexSelected: any; // for editing
  clonedSelectedEmployee: Employee; // for editing

  //validations
  msgs: Message[] = [];

  userform: FormGroup;


  submitted: boolean;

  description: string;

  constructor(
    private employeeService: EmployeeService,
    private genericService: GenericService,
    private fb: FormBuilder,
    public authService: AuthService
  ) { }

  ngOnInit() {

    //validations
    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'officePhone': new FormControl('', Validators.required),
      'emailAddress': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required),
      'mobilePhone': new FormControl('', Validators.compose([Validators.required, Validators.minLength(11)])),
      'description': new FormControl(''),
      //'gender': new FormControl('', Validators.required)
    });

  }
  formSubmitted(message): void {
    this.submitted = true;
    this.msgs = [];
    this.msgs.push({ severity: 'info', summary: 'Success', detail: message });
  }

  clearContent(tmpEmployeeList) {
    this.employeeList = tmpEmployeeList;
    //this.selectedEmployee = null;
    // this.clonedSelectedEmployee = null;
    this.clonedSelectedEmployee = new EmployeeClass();
    this.userform.markAsPristine();
    this.isNewEmployee = false;
  }

  // v2
  cancelForEdit: boolean = true;
  displayModal: boolean = false;

  showAddAndNew: boolean = false;
  showAdd: boolean = false;
  showDelete: boolean = false;

  disableForm: boolean = false;

  checkIfEmpty = (event) => {
    console.log(event);
    if (event == "") {
      this.search();
    }
  }

  search = () => {

    if(this.searchQuery.length >= 2) {
      this.dataTable.reset();
    } 

    if(this.searchQuery.length == 0){
      this.dataTable.reset();
    }

    if (this.searchQuery.length == 1) {
      this.formSubmitted("Search key must be 2 or more characters!");
    }
  }


  hideModalBtn = () => {
    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;
  }


  add = () => {
    this.cancelForEdit = false;
    // this.toggleForm(false);
    this.userform.markAsPristine();
    this.userform.enable();
    this.isNewEmployee = true;
    this.clonedSelectedEmployee = new EmployeeClass();

    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  cancel = () => {
    if (this.cancelForEdit) {

      if(JSON.stringify(this.clonedSelectedEmployee) === JSON.stringify(this.selectedEmployee)) {
        this.isNewEmployee = false;
        this.clonedSelectedEmployee = new EmployeeClass();
        this.userform.markAsPristine();
        this.displayModal = false;
        this.hideModalBtn();
      } else {
        this.clonedSelectedEmployee = JSON.parse(JSON.stringify(this.selectedEmployee));
      }
      // this.hideModalBtn();
    }
    else {
      this.isNewEmployee = false;
      this.clonedSelectedEmployee = new EmployeeClass();
      this.userform.markAsPristine();
      this.displayModal = false;
      this.hideModalBtn();
    }
  }

  save = (invoker) => {
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {

      this.genericService.Create<Employee>(this.entity, this.clonedSelectedEmployee)
        .then(data => {
          tmpEmployeeList.push(data);
          this.clearContent(tmpEmployeeList);
          // alert('Success!');
          this.formSubmitted("Employee Details Submitted!");

          this.displayModal = invoker == "Save" ? false : true;
          this.isNewEmployee = invoker == "Save" ? false : true;
          this.dataTable.reset();
        });

    } else {
      this.genericService.Update<Employee>(this.entity, this.clonedSelectedEmployee.employeeId, this.clonedSelectedEmployee)
        .then(data => {
          tmpEmployeeList[this.indexSelected] = this.clonedSelectedEmployee;
          this.clearContent(tmpEmployeeList);
          // alert('Success!');
          this.formSubmitted("Updated Employee Details!");
          this.displayModal = false;
          this.hideModalBtn();
          this.dataTable.reset();
        });
    }
  }

  edit = (rowData) => {
    this.cancelForEdit = true;

    // this.toggleForm(false);
    this.userform.enable();
    this.selectedEmployee = rowData;
    this.indexSelected = this.employeeList.indexOf(this.selectedEmployee); // value will be the index used for editing
    this.clonedSelectedEmployee = JSON.parse(JSON.stringify(this.selectedEmployee)); // cloned value of selected
    this.isNewEmployee = false;

    this.showAddAndNew = false;
    this.showAdd = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  setdeletion = (rowData) => {
    // this.toggleForm(true, rowData);
    this.cancelForEdit = false;

    this.userform.disable();
    this.selectedEmployee = rowData;

    this.indexSelected = this.employeeList.indexOf(this.selectedEmployee); // value will be the index used for editing

    this.clonedSelectedEmployee = JSON.parse(JSON.stringify(this.selectedEmployee)); // cloned value of selected
    this.isNewEmployee = false;

    this.showAddAndNew = false;
    this.showAdd = false;
    this.showDelete = true;

    this.displayModal = true;
  }

  delete = () => {
    if (this.indexSelected > -1) {
      let tmpEmployeeList = [...this.employeeList];

      this.genericService
        .Delete(this.entity, this.clonedSelectedEmployee.employeeId)
        .then(res => {
          console.log(res);
          if (res === 204) {
            tmpEmployeeList.splice(this.indexSelected, 1);
            this.employeeList = tmpEmployeeList;
            this.clonedSelectedEmployee = null;
            this.indexSelected = -1;
            // alert('Success!');
            this.displayModal = false;
            // this.toggleForm(false, new EmployeeClass);
            this.disableForm = false;
            this.hideModalBtn();
            this.userform.markAsPristine();
            this.formSubmitted("Employee Deleted!");
            this.dataTable.reset();
          } else {
            alert("Failed to Delete!");
          }
        });

    }
  }

  paginate = (event) => {
    this.genericService.Retrieve<Pagination<Employee>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.employeeList = result.result;
        this.totalRecords = result.totalCount;
      });
  }
  
}
