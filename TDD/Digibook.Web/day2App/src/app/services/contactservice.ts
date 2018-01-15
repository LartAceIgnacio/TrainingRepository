import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Contact } from '../domain/contact';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ContactService{
    constructor(private http: HttpClient){
    }

      getContact(page, record){
        return this.http.get('http://localhost:49784/api/contacts?pageNo='+page+'&recPerPage='+record)
                    .toPromise()
                    .then(result => { return result as Contact[]; });
      }
    
      postContact(data){
         return this.http.post('http://localhost:49784/api/contacts', data)
                  .toPromise()
                  .then(result => {return result as Contact});
      }
    
      putContact(id, data){
        return this.http.put('http://localhost:49784/api/contacts?id='+id, data)
                  .toPromise()
                  .then(result => {return result as Contact});
      }
    
      deleteContact(id){
        return this.http.delete('http://localhost:49784/api/contacts?id='+id)
                  .toPromise()
                  .then(result => {});
      }
}
