import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Employee } from '../domain/employee/employee';
import 'rxjs/add/operator/toPromise';

import { HttpClient } from '@angular/common/http';

@Injectable()
export class EmployeeService {


    constructor(private http: Http) { }

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private url = 'http://localhost:7604/api/Employee';

    getEmployees(): Promise<Employee[]> {
        return this.http.get(this.url)
            .toPromise()
            .then(response => {
                return response.json() as Employee[]
            })
            .catch(this.handleError);
    }

    getEmployee(id: string): Promise<Employee> {
        const url = `${this.url}?id=${id}`;
        console.log(url);
        return this.http.get(url)
            .toPromise()
            .then(response => {
                return response.json() as Employee
            })
            .catch(this.handleError);
    }

    createEmployee(employee: Employee): Promise<Employee> {
        console.log(JSON.stringify(employee));
        return this.http
            .post(this.url, JSON.stringify(employee), { headers: this.headers })
            .toPromise()
            .then(res => {
                console.log(res);
                return res.json() as Employee;
            })
            .catch(this.handleError);
    }

    deleteEmployee(employee: Employee): Promise<number> {
        const url = `${this.url}?id=${employee.employeeId}`;
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(res => {
                    console.log(res);
                    return res.status;
            })
            .catch(this.handleError);
    }

    updateEmployee(employee: Employee): Promise<Employee> {
        const url = `${this.url}?id=${employee.employeeId}`;
        return this.http
          .put(url, JSON.stringify(employee), { headers: this.headers })
          .toPromise()
          .then(res => {
              console.log(res);
              return res.json() as Employee;
          })
          .catch(this.handleError);
      }
    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

    // getEmployees() {
    //     return this.http.get('assets/data/employees.json')
    //                             .toPromise()
    //                             .then( res => <Employee[]> res.json().data)
    //                             .then( data => { return data; });
    // }
    // getEmployees() {
    //     return this.http.get('http://localhost:7604/api/Employee')
    //         .toPromise()
    //         .then(data => { 
    //             console.log(data); 
    //             return data as Employee[]; 
    //         });
    // }
}