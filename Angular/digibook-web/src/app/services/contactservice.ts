
import { Injectable } from '@angular/core';
import { Contact } from '../domain/contact';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class ContactService {
    
constructor(private http: HttpClient) {}

    getContacts() {
        return this.http.get('http://localhost:52369/api/contacts/')
                .toPromise()
                .then(data => { return data as Contact[]; });
    }
    addContacts(addContacts) {
        return this.http.post('http://localhost:52369/api/contacts/',addContacts)
                .toPromise()
                .then(data => { return data as Contact; });
    }
    saveContacts(contact) {
        return this.http.put('http://localhost:52369/api/contacts/?id='+ contact.contactId, contact)
                .toPromise()
                .then(data => { return data as Contact[]; });
    }
    deleteContacts(contactId) {
        return this.http.delete('http://localhost:52369/api/contacts/?id='+ contactId)
                .toPromise()
                .then(()=>null);
    }
}
