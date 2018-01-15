import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Employee} from '../domain/Employee';
import 'rxjs/add/operator/toPromise';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { MessageService } from "primeng/components/common/messageservice";
import { Message } from "primeng/components/common/message";
import { Observable } from "rxjs/Observable";
import { API_URL} from './constants';

@Injectable()
export class EmployeeService{

    service: string = 'employees';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http:HttpClient){

    }

    getEmployees(){
        return this.http.get(this.serviceUrl)
        .toPromise()
        .then(data => { return data as Employee[]; });
    }

    postEmployees(postEmployees){
        return this.http.post(this.serviceUrl, postEmployees)
        .toPromise()
        .then(data => {return data as Employee; });
    }

    putEmployees(employee: Employee){
        return this.http.put(`${this.serviceUrl}?id=${employee.employeeId}` ,employee)
        .toPromise()
        .then(()=> employee);
    }

    deleteEmployees(employeeId){
        return this.http.delete(`${this.serviceUrl}?id=${employeeId}`)
        .toPromise()
        .then(() => null);
    }

}