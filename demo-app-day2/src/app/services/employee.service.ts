
import { Injectable } from '@angular/core';
import { Employee } from '../domain/employee';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class EmployeeService {
    
constructor(private httpClient: HttpClient) {}


    _getEmployees() {
        return this.httpClient.get('http://localhost:55168/api/employee/')
                .toPromise()
                .then(data => { console.log("Get Employee:" + JSON.stringify(data as Employee[])); return data as Employee[]; });
    }

    _addEmployee(employee) {
        return this.httpClient.post('http://localhost:55168/api/employee/', employee)
                .toPromise()
                .then(data => { console.log("add data:" + JSON.stringify(data as Employee)); return data as Employee;  });
    }

    _saveEmployee(employee) {
        return this.httpClient.put('http://localhost:55168/api/employee/?id='+ employee.id,employee)
                .toPromise()
                .then(data => { return data as Employee; });
    }

    _deleteEmployee(employeeId) {
        return this.httpClient.delete('http://localhost:55168/api/employee/?id='+ employeeId)
                .toPromise()
                .then(() => null);
    }
}
