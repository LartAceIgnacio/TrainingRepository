
import { Injectable } from '@angular/core';
import { Employee } from '../domain/employee';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class EmployeeService {
    
constructor(private http: HttpClient) {}

    getEmployees() {
        return this.http.get('http://localhost:55775/api/employees/')
                .toPromise()
                .then(data => { return data as Employee[]; });
    }
    addEmployees(addEmployees) {
        return this.http.post('http://localhost:55775/api/employees/',addEmployees)
                .toPromise()
                .then(data => { return data as Employee[]; });
    }
    saveEmployees(employee) {
        return this.http.put('http://localhost:55775/api/employees/?id='+ employee.employeeId, employee)
                .toPromise()
                .then(data => { return data as Employee[]; });
    }
    deleteEmployees(employeeId) {
        return this.http.delete('http://localhost:55775/api/employees/?id='+ employeeId)
                .toPromise()
                .then(()=>null);
    }
}
