
import { Injectable } from '@angular/core';
import { IContacts } from '../domain/IContacts';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class ContactsService {
    
constructor(private httpClient: HttpClient) {}

    _getContacts() {
        return this.httpClient.get('http://localhost:55168/api/contacts/')
                .toPromise()
                .then(data => {  return data as IContacts[]; });
    }

    _addContacts(contact) {
        return this.httpClient.post('http://localhost:55168/api/contacts/', contact)
                .toPromise()
                .then(data => { return data as IContacts[]; });
    }

    _saveContacts(contact) {
        return this.httpClient.put('http://localhost:55168/api/contacts/?id='+ contact.id, contact)
                .toPromise()
                .then(data => { return data as IContacts[]; });
    }

    _deleteContacts(contactId) {
        return this.httpClient.delete('http://localhost:55168/api/contacts/?id='+ contactId)
                .toPromise()
                .then(() => null);
    }
}
