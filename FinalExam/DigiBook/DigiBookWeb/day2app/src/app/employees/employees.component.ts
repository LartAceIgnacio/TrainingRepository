import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employee';
import { EmployeeClass } from '../domain/employeeclass';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { ConfirmationService, MenuItem, DataTable } from "primeng/primeng";
import { GlobalService } from '../services/globalservice';
import { PaginationResult } from "../domain/paginationresult";
import { ViewChild } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [EmployeeService, GlobalService, ConfirmationService]
})
export class EmployeesComponent implements OnInit {
  home: MenuItem;
  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  dialogName: string;

  employeeList: Employee[];
  selectedEmployee: Employee;
  isNewEmployee: boolean;
  cloneEmployee: Employee;
  employee: Employee = new EmployeeClass();
  displayDialog: boolean;
  paginationResult: PaginationResult<Employee>;

  totalRecords: number = 0;
  searchFilter: string = "";
  items: MenuItem[];

  constructor(private employeeService: EmployeeService,
    private fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private globalService: GlobalService,
    public auth: AuthService
  ) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    // this.globalService.getSomething<Employee>("Employees")
    //   .then(employees => this.employeeList = employees);
    // this.selectedEmployee = this.employeeList[0];
    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.required),
      'mobile': new FormControl('', Validators.compose([Validators.required, Validators.minLength(11)])),
      'office': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required)
    });

    this.items = [
      { label: "Employees" }
    ]

    this.home = { icon: 'fa fa-home', routerLink: "/dashboard" };
  }
 paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Employee>>("Employees", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.employeeList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalCount;
        });
  }
  setCurrentPage(n: number) {
    this.dataTable.reset();
    // let paging = {
    //   first: ((n - 1) * this.dataTable.rows),
    //   rows: this.dataTable.rows
    // };
    //this.dataTable.paginate();
  }

  searchEmployee() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  deleteEmployee(selectedEmp: Employee) {
    this.selectedEmployee = selectedEmp;
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        if (this.selectedEmployee == null) {
          this.selectedEmployee = new EmployeeClass();
        } else {
          this.globalService.deleteSomething<Employee>("Employees", selectedEmp.employeeId);
          let index = this.employeeList.indexOf(this.selectedEmployee);
          this.employeeList = this.employeeList.filter((val, i) => i != index);
          this.selectedEmployee = null;
          this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully deleted.' }];
        }
      }
    });

  }

  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
    this.dialogName = "Add Employee"
    this.displayDialog = true;
  }

  editEmployee(emp: Employee) {
    this.isNewEmployee = false;
    this.selectedEmployee = emp;
    this.onRowSelect();
    this.dialogName = "Edit Employee"
    this.displayDialog = true;
  }

  SaveAndNewEmployee() {
    let tmpEmployeeList = [...this.employeeList];
    this.globalService.addSomething<Employee>("Employees", this.selectedEmployee).then(emp => {
      tmpEmployeeList.push(this.selectedEmployee);
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = new EmployeeClass();

      this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
    });
    this.userform.markAsPristine();
    this.displayDialog = true;
  }

  saveEmployee() {
    let tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.globalService.addSomething<Employee>("Employees", this.selectedEmployee).then(emp => {
        tmpEmployeeList.push(emp);
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = null;

        this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully saved.' }];
      });
    }
    else {
      this.globalService.updateSomething<Employee>("Employees", this.selectedEmployee.employeeId, this.selectedEmployee);
      tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
      this.employeeList = tmpEmployeeList;
      this.selectedEmployee = null;

      this.msgs = [{ severity: 'success', summary: 'Success!', detail: 'Record has been successfully updated.' }];
    }
    this.userform.markAsPristine();
    this.displayDialog = false;
    this.isNewEmployee = false;
  }

  closeDisplayNew() {
    this.displayDialog = false;
    this.isNewEmployee = false;
    this.selectedEmployee = null;
  }

  confirmCancel() {
    this.isNewEmployee = false;
    let tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee = Object.assign({}, this.cloneEmployee);
    this.selectedEmployee = new EmployeeClass();
    this.displayDialog = false;
    this.userform.markAsPristine();
  }

  cancelEmployee() {
    if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        accept: () => {
          this.confirmCancel();
        }
      });
    } else {
      this.displayDialog = false;
    }
  }
  onRowSelect() {
    this.isNewEmployee = false;
    this.cloneEmployee = Object.assign({}, this.selectedEmployee);
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
  constructor(public results, public pageNo, public recordPage, public totalCount) {

  }
}