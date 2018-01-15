import {Contact} from '../domain/contact';

export class ContactClass implements Contact
{
    
    constructor(public firstName?, public lastName?
        , public mobilePhone?, public streetAddress?
        , public  cityAddress?, public emailAddress?
        , public zipCode?, public country?, public contactId?)
    {
        
    }

}