import { Employee } from '../domain/employee';

export class EmployeeClass implements Employee {

    constructor(public id?, public firstName?, public lastName?, public mobilePhone?, public officePhone?
                , public extension?, public emailAddress?)
    {
        
    }
}
