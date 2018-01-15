import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Contact } from '../domain/contacts/contact';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';
import { API_URL } from './constants';

@Injectable()
export class ContactService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'contacts';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) { }

    getContacts() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Contact[]; })
            .catch(this.handleError);
    }

    postContacts(contactToPost: Contact) {
        return this.http.post(this.serviceUrl, contactToPost)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Contact; });
    }

    putContacts(contactId, contactToPut: Contact) {
        return this.http.put(`{this.serviceUrl}?id={contactId}`, contactToPut)
            .toPromise()
            .then(() => contactToPut)
            .catch(this.handleError);
    }

    deleteContacts(contactId) {
        return this.http.delete(`${this.serviceUrl}?id=${contactId}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}
