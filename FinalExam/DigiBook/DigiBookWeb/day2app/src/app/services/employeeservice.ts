import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Employee } from '../domain/employee';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from "@angular/common/http";
import { API_URL } from './constants'; 

@Injectable()
export class EmployeeService {
    service:string = 'employees';
    serviceUrl: string = `${API_URL}/${this.service}`;

    constructor(private http: HttpClient) {}

    getEmployees() {
        return this.http.get(this.serviceUrl)
                .toPromise()
                .then(data => { return data as Employee[]; });
    }
    putEmployees(employeeId, employees) {
        return this.http.put(`${this.serviceUrl}?id=${employeeId}`, employees)
        .toPromise()
        .then(data => { return data as Employee[]; });
    }
    postEmployees(employees) {
        return this.http.post(`${this.serviceUrl}`, employees)
                .toPromise()
                .then(data => { return data as Employee; });
    }

    deleteEmployees(employeeId) {
        return this.http.delete(`${this.serviceUrl}?id=${employeeId}`)
        .toPromise()
        .then();
    }
}