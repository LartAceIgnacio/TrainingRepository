import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Employee } from '../domain/employee'
import 'rxjs/add/operator/toPromise';

@Injectable()
export class EmployeeService{
    /**
     *
     */
    constructor(private http: Http) {}

    getEmployees(){
        return this.http.get('assets/data/employees.json')
            .toPromise()
            .then(res => <Employee[]>res.json().data)
            .then(data => {console.log("daaata: " + data); return data;} );
    }
}
