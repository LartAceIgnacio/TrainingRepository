import { MenuItem, ConfirmationService, DataTable} from 'primeng/primeng';
import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeService } from './../services/employeeService';
import { Employee } from './../domain/employee';
import { Employeeclass } from './../domain/employeeclass';
import { Validators,FormControl,FormGroup,FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { RouterLink } from '@angular/router/src/directives/router_link';
import { GlobalService } from '../services/global.service';
import { Pagination } from '../domain/pagination';


@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [GlobalService,ConfirmationService]
})
export class EmployeesComponent implements OnInit {
  delete: boolean;

  clonedSelectedEmployee: Employee;
  indexSelected: number;

  serviceName: string = "Employees";
  searchFilter: string = "";
  pagination: Pagination<Employee>;
  totalRecords: number = 0;

  employeeList: Employee[];
  selectedEmployee: Employee;
  cloneEmployee: Employee;
  isNewEmployee: boolean;
  displayDialog: boolean;
  loading: boolean;
  employee: Employee = new Employeeclass();

  msgs: Message[] = [];
  employeeForm: FormGroup;
  submitted: boolean;
  breadcrumb: MenuItem[];

constructor(private globalService: GlobalService, private fb: FormBuilder, private confirmationService: ConfirmationService) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {

    this.employeeForm = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'mobilephone': new FormControl('', Validators.required),
      'emailaddress': new FormControl('', Validators.required),
      'officephone': new FormControl('', Validators.required),
      'extension': new FormControl('', Validators.required),
      'notes': new FormControl('', Validators.required)
    });

    this.breadcrumb = [
      {label:'Dashboard', routerLink:['/dashboard']},
      {label:'Employees', routerLink:['/employees']},
  ];
    //this.selectedEmployee = this.employeeList[0]; 
  }
  paginate(event) {
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
    this.globalService.getPagination<Pagination<Employee>>(this.serviceName, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
        this.pagination = pagination;
        this.employeeList = this.pagination.results;
        this.totalRecords = this.pagination.totalRecords;
        // for (var i = 0; i < this.contactList.length; i++) {
        //   this.contactList[i].dateActivated = this.contactList[i].dateActivated == null ? null :
        //     new Date(this.contactList[i].dateActivated).toLocaleDateString();
        // }
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
    //     first: ((n - 1) * this.dataTable.rows),
    //     rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }
  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new Employeeclass;
    this.displayDialog = true;
    this.employeeForm.enable();
  }
  editEmployee(Employee : Employee){
    this.isNewEmployee = false;
    this.selectedEmployee = Employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.displayDialog = true;
    this.employeeForm.enable();
  }

  saveEmployee() {
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee){
      this.globalService.addData<Employee>(this.serviceName,this.selectedEmployee).then(employees => {
        tmpEmployeeList.push(employees);
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee=null;
        });
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Success', detail:'Added Employee'});
    }else{
      this.globalService.updateData<Employee>(this.serviceName,this.selectedEmployee.employeeId, this.selectedEmployee).then(employees =>{
        tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
          this.employeeList=tmpEmployeeList;
          this.selectedEmployee=null;
        });
        // this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Employee Details'});
    }
    this.employeeForm.markAsPristine();
    this.employeeList = tmpEmployeeList;
    this.selectedEmployee = null;
    this.displayDialog = false;
  }

  saveAndNewEmployee(){

    this.employeeForm.markAsPristine();
    let tmpEmployeeList = [...this.employeeList];
    tmpEmployeeList.push(this.selectedEmployee);

    // this.submitted = true;
    this.msgs = [];
    this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Employee Details'});
    if(this.isNewEmployee){
      this.globalService.addData<Employee>(this.serviceName, this.selectedEmployee);
      this.employeeList=tmpEmployeeList;
      this.isNewEmployee = true;
      this.selectedEmployee = new Employeeclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Employee'});
    }
    
  }

  deleteEmployee(Employee : Employee){
    this.employeeForm.disable();
    this.selectedEmployee = Employee;
     this.displayDialog = true;
     this.delete = true;
  }
  delEmployee(){
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
          let index = this.findSelectedEmployeeIndex();
          this.employeeList = this.employeeList.filter((val,i) => i!=index);
          this.globalService.deleteData<Employee>(this.serviceName,this.selectedEmployee.employeeId);  
          this.submitted = true;
          this.msgs = [];
          this.msgs.push({severity:'error', summary:'Danger', detail:'Deleted Employee'});
          this.selectedEmployee = null;     
      }
  });
  }

  // onRowSelect(event) {
  //         this.isNewEmployee = false;
  //         this.selectedEmployee;
  //         this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
  //         // this.displayDialog = true;
  // } 

  cloneRecord(r: Employee): Employee {
      let employee = new Employeeclass();
      for(let prop in r) {
          employee[prop] = r[prop];
      }
      return employee;
  }

  cancelEmployee(){
    if(this.employeeForm.dirty){  
      this.confirmationService.confirm({
        message: 'Do you want to Discard this changes?',
        accept: () => {
          this.isNewEmployee = false;
          let tmpEmployeeList = [...this.employeeList];
          tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.cloneEmployee;
          this.employeeList = tmpEmployeeList;
          this.selectedEmployee = this.cloneEmployee;
          this.selectedEmployee = null;
        }
      });
    }
  }

  findSelectedEmployeeIndex(): number {
      return this.employeeList.indexOf(this.selectedEmployee);
  }
}
