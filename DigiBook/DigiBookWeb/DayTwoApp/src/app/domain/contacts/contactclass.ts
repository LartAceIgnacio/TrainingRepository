import { Contact } from '../contacts/contact';
export class ContactClass implements Contact {

    constructor(public contactId?, public firstName?, public lastName?, public fullName?, public mobilePhone?, public streetAddress?,
        public cityAddress?, public zipCode?, public country?, public emailAddress?, public isActive?,
        public dateActivated?, public contactCount?) { }
}
