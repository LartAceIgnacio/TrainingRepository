import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Contact} from '../domain/contact';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';


import {Appointment} from '../domain/appointment';

@Injectable()

export class ContactService{

    constructor(private http: HttpClient) {}      

    getContacts(){
        return this.http.get('http://localhost:52675/api/contacts')
        .toPromise()
        .then(data=> { return data as Contact[]; });
    }

    addContacts(contact:Contact){
        return this.http.post('http://localhost:52675/api/contacts',contact)
        .toPromise()
        .then(data=> { return data as Contact});
    }

    updateContacts(contact:Contact){
        return this.http.put('http://localhost:52675/api/contacts?id='+contact.contactId ,contact, contact.contactId)
        .toPromise()
        .then( ()=> contact );
    }

    deleteContacts(contact:Contact){
        return this.http.delete('http://localhost:52675/api/contacts?id='+contact.contactId ,contact.contactId)
        .toPromise()
        .then( ()=> contact );
    }
}