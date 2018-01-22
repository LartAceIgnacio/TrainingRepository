import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Contact } from '../domain/contact/contact';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ContactService {


    constructor(private http: Http) { }

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private url = 'http://localhost:7604/api/Contacts';

    getContacts(): Promise<Contact[]> {
        return this.http.get(this.url)
            .toPromise()
            .then(response => {
                return response.json() as Contact[]
            })
            .catch(this.handleError);
    }

    getContact(id: string): Promise<Contact> {
        const url = `${this.url}?id=${id}`;
        console.log(url);
        return this.http.get(url)
            .toPromise()
            .then(response => {
                return response.json() as Contact
            })
            .catch(this.handleError);
    }

    createContact(Contact: Contact): Promise<Contact> {
        console.log(JSON.stringify(Contact));
        return this.http
            .post(this.url, JSON.stringify(Contact), { headers: this.headers })
            .toPromise()
            .then(res => {
                console.log(res);
                return res.json() as Contact;
            })
            .catch(this.handleError);
    }

    deleteContact(Contact: Contact): Promise<number> {
        const url = `${this.url}?id=${Contact.contactId}`;
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(res => {
                    console.log(res);
                    return res.status;
            })
            .catch(this.handleError);
    }

    updateContact(Contact: Contact): Promise<Contact> {
        const url = `${this.url}?id=${Contact.contactId}`;
        return this.http
          .put(url, JSON.stringify(Contact), { headers: this.headers })
          .toPromise()
          .then(res => {
              console.log(res);
              return res.json() as Contact;
          })
          .catch(this.handleError);
      }
    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

}