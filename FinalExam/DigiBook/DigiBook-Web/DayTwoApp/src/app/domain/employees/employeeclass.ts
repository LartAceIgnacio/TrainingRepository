import {Employee} from '../employees/employee';
export class EmployeeClass implements Employee {

    constructor(public employeeId?,
        public firstName?,
        public lastName?,
        public mobilePhone?,
        public emailAddress?,
        public photo?,
        public officePhone?,
        public extension?,
        public totalRecords?,
        public employeeCount?,
        public fullName?) {}
}
