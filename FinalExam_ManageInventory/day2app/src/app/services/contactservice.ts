import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Contact} from '../domain/contact';
import { HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ContactService
{
    constructor(private http: HttpClient)
    {

    }

    getContacts()
    {
        return this.http.get('http://localhost:65036/api/Contacts')
        .toPromise()
        .then(data => {return data as Contact[];});
    }
    
    createContact(createdContact)
    {
        return this.http.post('http://localhost:65036/api/Contacts',createdContact)
        .toPromise()
        .then(data => {return data as Contact;});
    }

    updateContact(updateContact, contactId)
    {
        return this.http.put('http://localhost:65036/api/Contacts/?id=' + contactId, updateContact)
        .toPromise()
        .then(data => {return data as Contact[];});
    }

    deleteContact(deletedContact)
    {
        return this.http.delete('http://localhost:65036/api/Contacts/?id=' + deletedContact)
        .toPromise()
        .then(() => null);
    }

}