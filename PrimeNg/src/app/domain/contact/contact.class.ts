import { Contact } from '../contact/contact';
export class ContactClass implements Contact {

    constructor(
        public firstName?, public lastName?, public mobilePhone?,
        public streetAddress?, public cityAddress?, public zipCode?,
        public country?, public emailAddress?, public isActive?,
        public dateActivated?, public contactId?
        
    ) {}

}