import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../services/employeeservice';
import { Employee } from '../domain/employees/employee';
import { EmployeeClass } from '../domain/employees/employeeclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';


@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class EmployeesComponent implements OnInit {

  items: MenuItem[];

  home: MenuItem;

  displayDialog: boolean;

  employee: Employee = new EmployeeClass();

  selectedEmployee: Employee;

  newEmployee: boolean;

  employees: Employee[];

  isDelete: boolean;

  isEdit: boolean;

  isNewEmployee: boolean;

  loading: boolean;

  userform: FormGroup;

  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';

  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  paginationResult: PaginationResult<Employee>;

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService, private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      // this.employeeService.getEmployees().then(employees => this.employees = employees);
      // this.employeeService.getEmployees()
      //   .then(employees => this.employees = employees);
      this.loading = false;
    }, 1000);

    this.items = [
      { label: 'Home', routerLink: ['/dashboard'] },
      { label: 'Empployee' }
    ];

    this.home = { icon: 'fa fa-home' };

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.required),
      'officephone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required)
    });

  }

  showDialogToAdd() {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = true;
    this.isDelete = false;
    this.newEmployee = true;
    this.selectedEmployee = null;
    this.employee = new EmployeeClass();
    this.displayDialog = true;
  }

  onRowSelect(clickEmployee: Employee) {
    this.newEmployee = false;
    this.employee = this.cloneContact(clickEmployee);
    this.selectedEmployee = clickEmployee;
    this.displayDialog = true;
  }

  cloneContact(e: Employee): Employee {
    const employee = new EmployeeClass();
    // tslint:disable-next-line:forin
    for (const prop in e) {
      employee[prop] = e[prop];
    }
    return employee;
  }
  save(number: boolean) {
    this.isNewEmployee = number;

    const employees = [...this.employees];
    if (this.newEmployee) {
      this.globalService.addSomething<Employee>('Employees', this.employee).then(
        data => {
          this.employee = data;
          employees.push(this.employee);
          this.employees = employees;
          this.employee = new EmployeeClass();
          this.dataTable.reset();
        }
      );

    } else {
      this.globalService.updateSomething('Employees', this.employee.employeeId, this.employee).then(
        data => {
          this.employee = data;
          employees[this.findSelectedEmployeeIndex()] = this.employee;
          this.employees = employees;
        }
      );
    }
    if (this.isNewEmployee) {
      this.userform.markAsPristine();
      // this.employees = employees;
      this.newEmployee = true;
      this.selectedEmployee = null;

    } else {
      this.userform.markAsPristine();
      // this.employees = employees;
      this.employee = null;
      this.displayDialog = false;
      // this.setCurrentPage(1);
    }
  }

  findSelectedEmployeeIndex(): number {
    return this.employees.indexOf(this.selectedEmployee);
  }

  delete(clickEmployee: Employee) {
    this.userform.disable();
    this.isDelete = true;
    this.onRowSelect(clickEmployee);
  }

  deleteEmployee() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        this.globalService.deleteSomething('Employees', this.employee.employeeId);
        const index = this.findSelectedEmployeeIndex();
        this.employees = this.employees.filter((val, i) => i !== index);
        this.employee = null;
        this.displayDialog = false;
        this.dataTable.reset();
      }
    });
  }

  edit(clickEmployee: Employee) {
    this.userform.enable();
    this.isEdit = false;
    this.isDelete = false;
    this.onRowSelect(clickEmployee);
  }

  cancel() {
    if (this.isDelete) {
      this.displayDialog = false;
    }else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to cancel?',
        accept: () => {
          this.employee = Object.assign({}, this.selectedEmployee);
        }
      });
    }
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Employee>>('Employees', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
          this.paginationResult = paginationResult;
          this.employees = this.paginationResult.results;
          this.totalRecords = this.paginationResult.totalRecords;
        });
  }

  searchEmployee() {
    if (this.searchFilter.length !== 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    // let paging = {
    //   first: ((n - 1) * this.dataTable.rows),
    //   rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }

}

class PaginationResultClass implements PaginationResult<Employee> {
  constructor (public results, public pageNo, public recordPage, public totalRecords) {

  }
}
