import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employees/employee';
import { EmployeeClass } from '../domain/employees/employeeclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';



@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GlobalService, ConfirmationService, FormBuilder]
})

export class EmployeesComponent implements OnInit {

  items: MenuItem[];
  home: MenuItem;
  employeeList: Employee[];
  selectedEmployee: Employee;
  isNewEmployee: boolean;
  cloneEmployee: Employee;
  display: boolean;
  userform: FormGroup;
  isDelete: boolean;
  isEdit: boolean;
  paginationResult: PaginationResult<Employee>;
  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';
  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

    @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {

    this.items = [
      { label: 'Employees' }
    ];
    this.home = { icon: 'fa fa-home', label: 'Home', routerLink: '/dashboard' };

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.compose([Validators.required, Validators.email])),
      'mobilephone': new FormControl('', Validators.compose([Validators.required, Validators.minLength(11)])),
      'officephone': new FormControl('', Validators.required),
      'extensionNumber': new FormControl('', Validators.required)
    });
  }

  showDialog() {
    this.isEdit = false;
    this.isDelete = false;
    this.userform.enable();
    this.userform.markAsPristine();
    this.isNewEmployee = true;
    this.selectedEmployee = new EmployeeClass();
    this.display = true;
  }

  searchEmployee() {
    if (this.searchFilter.length !== 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Employee>>('Employees', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.employeeList = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
        });
  }

  editEmployee(employeeToEdit: Employee) {
    this.isEdit = true;
    this.isDelete = false;
    this.userform.enable();
    this.isNewEmployee = false;
    this.selectedEmployee = this.cloneRecord(employeeToEdit);
    this.cloneEmployee = employeeToEdit;
    this.display = true;
  }

  deleteEmployee(employeeToDelete: Employee) {
    this.isDelete = true;
    this.userform.disable();
    this.display = true;
    this.isNewEmployee = false;
    this.selectedEmployee = this.cloneRecord(employeeToDelete);
    this.cloneEmployee = employeeToDelete;
    this.userform.markAsPristine();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        this.globalService.deleteSomething<Employee>('Employees', this.selectedEmployee.employeeId);
        const index = this.employeeList.indexOf(this.cloneEmployee);
        this.employeeList = this.employeeList.filter((val, i) => i !== index);
        this.selectedEmployee = null;
        this.isNewEmployee = false;
        this.display = false;

      }
    });
  }

  saveEmployee() {
    const tmpEmployeeList = [...this.employeeList];
    if (this.isNewEmployee) {
      this.globalService.postSomething<Employee>('Employees', this.selectedEmployee)
        .then(employee => {
          tmpEmployeeList.push(employee);
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = null;
          this.display = false;
        });
    } else {
      this.globalService.putSomething<Employee>('Employees', this.selectedEmployee.employeeId, this.selectedEmployee)
        .then(employee => {
          tmpEmployeeList[this.employeeList.indexOf(this.cloneEmployee)] = this.selectedEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = null;
          this.display = false;
        });
    }
    this.isNewEmployee = false;
  }

  newSaveEmployee() {
    this.userform.markAsPristine();
    const tmpEmployeeList = [...this.employeeList];
    this.globalService.postSomething<Employee>('Employees', this.selectedEmployee)
      .then(employees => {
        tmpEmployeeList.push(employees);
        this.employeeList = tmpEmployeeList;
        this.selectedEmployee = new EmployeeClass;
        this.display = true;
      });
  }

  confirmCancel() {
    this.isNewEmployee = false;
    const tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee = Object.assign({}, this.cloneEmployee);
    this.selectedEmployee = new EmployeeClass();
    this.display = false;
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
      this.display = false;
    }
  }

  onRowSelect() {
    this.isNewEmployee = false;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  }

  cloneRecord(r: Employee): Employee {
    // tslint:disable-next-line:prefer-const
    let employee = new EmployeeClass();
    // tslint:disable-next-line:forin
    for (const prop in r) {
      employee[prop] = r[prop];
    }
    return employee;
  }
}
