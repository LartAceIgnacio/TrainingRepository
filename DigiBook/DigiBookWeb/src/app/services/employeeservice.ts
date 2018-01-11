import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Employee } from '../domain/employee'
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class EmployeeService {
    service: string = 'employees';
    serviceUrl: string = `${API_URL}/${this.service}/`;
    constructor(private http: HttpClient) {}

    getEmployees() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            .then(data => { return data as Employee[]; })
            .catch(this.handleError);
    }
    
    addEmployee(objEmployee: Employee) {
        return this.http.post(this.serviceUrl, objEmployee)
            .toPromise()
            .then(data => { return data as Employee; })
            .catch(this.handleError);
    }
    
    deleteEmployee(id) {
        return this.http.delete(`${this.serviceUrl}?id=${id}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateEmployee(id, objEmployee: Employee) {
        return this.http.put(`${this.serviceUrl}?id=${id}`, objEmployee)
            .toPromise()
            .then(() => objEmployee)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
