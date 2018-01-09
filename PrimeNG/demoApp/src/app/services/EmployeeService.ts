import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Employee} from '../domain/Employee';
import 'rxjs/add/operator/toPromise';

import {HttpClient, HttpHeaders} from '@angular/common/http';
import { MessageService } from "primeng/components/common/messageservice";
import { Message } from "primeng/components/common/message";
import { Observable } from "rxjs/Observable";

@Injectable()
export class EmployeeService{

    constructor(private http:HttpClient){

    }

    getEmployees(){
        return this.http.get('http://localhost:56416/api/employees')
        .toPromise()
        .then(data => { return data as Employee[]; });
    }

    postEmployees(postEmployees){
        return this.http.post('http://localhost:56416/api/employees', postEmployees)
        .toPromise()
        .then(data => {return data as Employee; });
    }

    putEmployees(employee: Employee){
        return this.http.put('http://localhost:56416/api/employees/?id='+employee.employeeId ,employee)
        .toPromise()
        .then(()=> employee);
    }

    deleteEmployees(employeeId){
        return this.http.delete('http://localhost:56416/api/employees/?id=' + employeeId)
        .toPromise()
        .then(() => null);
    }

}