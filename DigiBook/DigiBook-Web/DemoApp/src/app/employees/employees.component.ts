import { MenuItem, ConfirmationService} from 'primeng/primeng';
import { Component, OnInit } from '@angular/core';
import { EmployeeService } from './../services/employeeService';
import { Employee } from './../domain/employee';
import { Employeeclass } from './../domain/employeeclass';
import { Validators,FormControl,FormGroup,FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { RouterLink } from '@angular/router/src/directives/router_link';


@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [EmployeeService,ConfirmationService]
})
export class EmployeesComponent implements OnInit {

  clonedSelectedEmployee: Employee;
  indexSelected: number;

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

constructor(private employeeService: EmployeeService, private fb: FormBuilder, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.employeeService.getEmployees().then(employees => this.employeeList = employees);
      this.loading = false;
    }, 1000);

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

  addEmployee() {
    this.isNewEmployee = true;
    this.selectedEmployee = new Employeeclass;
    this.displayDialog = true;
  }
  editEmployee(Employee : Employee){
    this.isNewEmployee = false;
    this.selectedEmployee = Employee;
    this.cloneEmployee = this.cloneRecord(this.selectedEmployee);
    this.displayDialog = true;
  }

  saveEmployee() {
    let tmpEmployeeList = [...this.employeeList];
    if(this.isNewEmployee){
        this.employeeService.addEmployees(this.selectedEmployee);
        tmpEmployeeList.push(this.selectedEmployee);
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Success', detail:'Added Employee'});
    }else{
        this.employeeService.saveEmployees(this.selectedEmployee);
        tmpEmployeeList[this.employeeList.indexOf(this.selectedEmployee)] = this.selectedEmployee;
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
      this.employeeService.addEmployees(this.selectedEmployee);
      this.employeeList=tmpEmployeeList;
      this.isNewEmployee = true;
      this.selectedEmployee = new Employeeclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Employee'});
    }
    
  }

  deleteEmployee(Employee : Employee){
    this.selectedEmployee = Employee;
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
          let index = this.findSelectedEmployeeIndex();
          this.employeeList = this.employeeList.filter((val,i) => i!=index);
          this.employeeService.deleteEmployees(this.selectedEmployee.employeeId);  
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
