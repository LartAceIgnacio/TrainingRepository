import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Contact } from '../domain/contacts/contact';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from "@angular/common/http";

@Injectable()
export class ContactService {

    constructor(private http: HttpClient) {}

    getContacts() {
        return this.http.get('http://localhost:52212/api/contacts')
                .toPromise()
                .then(data => { return data as Contact[]; });
    }
    postContacts(contact) {
        return this.http.post('http://localhost:52212/api/contacts', contact)
                .toPromise()
                .then(data => { return data as Contact; });
    }

    putContacts(contactId, contactToPut) {
        return this.http.put('http://localhost:52212/api/contacts/?id=' + contactId, contactToPut)
        .toPromise()
        .then(data => { return data as Contact[]; });
    }

    deleteContacts(contactId) {
        return this.http.delete('http://localhost:52212/api/contacts/?id=' + contactId)
        .toPromise()
        .then();
    }
}