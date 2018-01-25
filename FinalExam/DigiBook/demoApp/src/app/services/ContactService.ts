import {Injectable} from '@angular/core';
import {Contact} from '../domain/Contact';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { API_URL} from './constants';

@Injectable()
export class ContactService{

    service: string = 'contacts';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http:HttpClient){
        
    }

    getContacts(){
        return this.http.get(this.serviceUrl)
        .toPromise()
        .then(data => {return data as Contact[];});
    }

    postContacts(postContacts){
        return this.http.post(this.serviceUrl, postContacts)
        .toPromise()
        .then(data => {return data as Contact;});
    }

    putContacts(contact: Contact){
        return this.http.put(`${this.serviceUrl}?id=${contact.contactId}`, contact,)
        .toPromise()
        .then(() => contact);
    }

    deleteContacts(contactId){
        return this.http.delete(`${this.serviceUrl}?id=${contactId}`)
        .toPromise()
        .then(() => null);
    }
}