import { Employee } from '../domain/employee';

export class EmployeeClass implements Employee {

    constructor(public id?, public firstname?, public lastname?, public employeeFullname?, public mobilePhone?, public officePhone?
                , public extension?, public emailAddress?)
    {
        
    }
}
