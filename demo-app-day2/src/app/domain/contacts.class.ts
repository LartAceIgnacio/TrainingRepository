import { IContacts } from '../domain/IContacts';

export class ContactsClass implements IContacts {

    constructor(public contactId?, public firstname?, public lastname?, public mobilePhone?, public officePhone?
                ,streetAddress? , public cityAddress?, public emailAddress?, public zipCode?, public isActive?
                , dateActivated?)
    {
        
    }
}
