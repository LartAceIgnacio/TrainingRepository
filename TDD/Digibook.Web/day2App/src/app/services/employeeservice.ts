import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../domain/employee';

@Injectable()
export class EmployeeService{
    constructor(private http: HttpClient){
    }

    getEmployees(){
         return this.http.get('http://localhost:49784/api/employees')
                    .toPromise()
                    .then(result =>{ return result as Employee[]; }); 
    }

    postEmployee(data){
        return this.http.post('http://localhost:49784/api/employees', data)
                 .toPromise()
                 .then(result => {return result as Employee});
     }
   
     putEmployee(id, data){
       return this.http.put('http://localhost:49784/api/employees?id='+id, data)
                 .toPromise()
                 .then(result => {return result as Employee});
     }
   
     deleteEmployee(id){
       return this.http.delete('http://localhost:49784/api/employees?id='+id)
                 .toPromise()
                 .then(result => {});
     }
}
