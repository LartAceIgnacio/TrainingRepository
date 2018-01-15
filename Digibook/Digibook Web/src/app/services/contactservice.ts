import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Contact } from '../domain/contact'
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ContactService {
    constructor(private http: HttpClient) {}

    getContacts() {
        return this.http.get('http://localhost:16013/api/Contacts/')
            .toPromise()
            .then(data => { return data as Contact[]; })
            .catch(this.handleError);
    }
    
    addContact(objContact: Contact) {
        return this.http.post('http://localhost:16013/api/Contacts/', objContact)
            .toPromise()
            .then(data => { return data as Contact; })
            .catch(this.handleError);
    }
    
    deleteContact(id) {
        return this.http.delete('http://localhost:16013/api/Contacts/?id=' + id)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateContact(id, objContact: Contact) {
        return this.http.put('http://localhost:16013/api/Contacts/?id=' + id, objContact)
            .toPromise()
            .then(() => objContact)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
