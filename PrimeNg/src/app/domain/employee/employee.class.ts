import { Employee } from '../employee/employee';
export class EmployeeClass implements Employee {

    constructor(
        public employeeId?, public firstName?, public lastName?, public mobilePhone?,
        public emailAddress?, public photo?, public photoUrl?, public officePhone?,
        public extension?
    ) { }

}