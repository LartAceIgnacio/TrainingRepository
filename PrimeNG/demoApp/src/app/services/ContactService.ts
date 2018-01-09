import {Injectable} from '@angular/core';
import {Contact} from '../domain/Contact';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class ContactService{
    constructor(private http:HttpClient){
        
    }

    getContacts(){
        return this.http.get('http://localhost:56416/api/contacts')
        .toPromise()
        .then(data => {return data as Contact[];});
    }

    postContacts(postContacts){
        return this.http.post('http://localhost:56416/api/contacts', postContacts)
        .toPromise()
        .then(data => {return data as Contact;});
    }

    putContacts(contact: Contact){
        return this.http.put('http://localhost:56416/api/contacts/?id='+contact.contactId, contact, contact.contactId)
        .toPromise()
        .then(() => contact);
    }

    deleteContacts(contactId){
        return this.http.delete('http://localhost:56416/api/contacts/?id=' + contactId)
        .toPromise()
        .then(() => null);
    }
}