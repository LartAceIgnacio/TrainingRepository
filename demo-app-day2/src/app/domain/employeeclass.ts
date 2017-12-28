import { Employee } from '../domain/employee';

export class EmployeeClass implements Employee {

    constructor(public Name?, public Address?,
         public Phone?, public Email?)
    {
        
    }
}