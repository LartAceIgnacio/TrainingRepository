import { Contact } from '../contacts/contact';

export class ContactClass implements Contact{
    constructor(   
        public contactId?,
        public firstName?,
        public lastName?,
        public mobilePhone?,
        public streetAddress?,
        public cityAddress?,
        public zipCode?,
        public country?,
        public emailAddress?
    ) {}
}
