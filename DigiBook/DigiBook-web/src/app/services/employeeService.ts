import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Employee} from '../domain/employee';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()

export class EmployeeService{

    constructor(private http: HttpClient) {}      

    getEmployees(){
        return this.http.get('http://localhost:52675/api/employees')
        .toPromise()
        .then( data => { return data as Employee[]; });
    }

    addEmployees(employee:Employee){
        return this.http.post('http://localhost:52675/api/employees',employee)
        .toPromise()
        .then( data => { return data as Employee; });
    }

    updateEmployees(employee:Employee){
        return this.http.put('http://localhost:52675/api/employees?id='+employee.employeeId ,employee, employee.employeeId)
        .toPromise()
        .then( () => employee );
    }

    deleteEmployees(employee:Employee){
        return this.http.delete('http://localhost:52675/api/employees?id='+employee.employeeId ,employee.employeeId)
        .toPromise()
        .then( () => employee );
    }
}