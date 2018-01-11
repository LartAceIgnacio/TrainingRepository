import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Contact } from '../domain/contact'
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class ContactService {
    service: string = 'contacts';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}

    getContacts() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            .then(data => { return data as Contact[]; })
            .catch(this.handleError);
    }
    
    addContact(objContact: Contact) {
        return this.http.post(this.serviceUrl, objContact)
            .toPromise()
            .then(data => { return data as Contact; })
            .catch(this.handleError);
    }
    
    deleteContact(id) {
        return this.http.delete(`${this.serviceUrl}?id=${id}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateContact(id, objContact: Contact) {
        return this.http.put(`{this.serviceUrl}?id={id}`, objContact)
            .toPromise()
            .then(() => objContact)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
