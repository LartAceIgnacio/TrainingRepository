import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Employee} from '../domain/employee';
import { HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class EmployeeService
{
    constructor(private http: HttpClient)
    {

    }

    getEmployees()
    {
        return this.http.get('http://localhost:65036/api/Employees')
        .toPromise()
        .then(data => {return data as Employee[];});
    }

    createEmployees(createdEmployee)
    {
        return this.http.post('http://localhost:65036/api/Employees',createdEmployee)
        .toPromise()
        .then(data => {return data as Employee;});
    }

    deleteEmployee(deletedEmployee)
    {
        return this.http.delete('http://localhost:65036/api/Employees/?id=' + deletedEmployee)
        .toPromise()
        .then(() => null);
    }

    updateEmployee(updateEmployee, employeeID)
    {
        return this.http.put('http://localhost:65036/api/Employees/?id='+ employeeID, updateEmployee)
        .toPromise()
        .then(data => {return data as Employee[];});
    }

}