import {Employee} from '../domain/Employee';

export class EmployeeClass implements Employee{
    constructor(
        public employeeId?,
        public firstName?,
        public lastName?,
        public mobilePhone?,
        public emailAddress?,
        public officePhone?,
        public extension?){}
}