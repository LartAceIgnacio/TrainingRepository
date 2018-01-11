import { Contact } from "../domain/contact"

export class ContactClass implements Contact {
    constructor (public contactId?, public firstName?, public lastName?, public fullName?, public mobilePhone?, 
        public emailAddress?,  public streetAddress?, public cityAddress?, public country?, public zipCode?, 
        public isActive?, public dateActivated?, public contactCount?) 
    {
        
    }
}