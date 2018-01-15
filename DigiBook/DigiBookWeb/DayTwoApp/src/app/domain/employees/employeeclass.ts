import { Employee } from '../employees/employee';
export class EmployeeClass implements Employee {

    constructor(public employeeId?, public lastName?, public firstName?, public fullName?, public mobilePhone?, public emailAddress?,
        public officePhone?, public extension?, public employeeCount?) { }
}
