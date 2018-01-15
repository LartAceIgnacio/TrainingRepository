import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Employee } from '../domain/employee'
import { HttpClient } from '@angular/common/http';

@Injectable()
export class EmployeeService {
    constructor(private http: HttpClient) {}

    getEmployees() {
        return this.http.get('http://localhost:16013/api/Employees/')
            .toPromise()
            .then(data => { return data as Employee[]; })
            .catch(this.handleError);
    }
    
    addEmployee(objEmployee: Employee) {
        return this.http.post('http://localhost:16013/api/Employees/', objEmployee)
            .toPromise()
            .then(data => { return data as Employee; })
            .catch(this.handleError);
    }
    
    deleteEmployee(id) {
        return this.http.delete('http://localhost:16013/api/Employees/?id=' + id)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateEmployee(id, objEmployee: Employee) {
        return this.http.put('http://localhost:16013/api/Employees/?id=' + id, objEmployee)
            .toPromise()
            .then(() => objEmployee)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
