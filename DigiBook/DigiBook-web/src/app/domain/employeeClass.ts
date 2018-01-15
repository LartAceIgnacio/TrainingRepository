import {Employee} from '../domain/employee';
export class EmployeeClass implements Employee{
    constructor(
        public firstName?,
        public lastName?,
        public fullName?,
        public mobilePhone?,
        public emailAddressFirstName?,
        public extension?,
        public officePhone?,
        public employeeId?,
        public appointments?,
    ){}
}
