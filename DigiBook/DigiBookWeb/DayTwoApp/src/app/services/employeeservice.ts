import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Employee } from '../domain/employees/employee';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';
import { API_URL } from './constants';

@Injectable()
export class EmployeeService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'employees';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) { }

    getEmployees() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Employee[]; })
            .catch(this.handleError);
    }

    postEmployees(employeeToPost: Employee) {
        return this.http.post(this.serviceUrl, employeeToPost)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Employee; })
            .catch(this.handleError);
    }

    putEmployees(employeeId, employeeToPut: Employee) {
        return this.http.put(`${this.serviceUrl}?id=${employeeId}`, employeeToPut)
            .toPromise()
            .then(() => employeeToPut)
            .catch(this.handleError);
    }

    deleteEmployees(employeeId) {
        return this.http.delete(`${this.serviceUrl}?id=${employeeId}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}
