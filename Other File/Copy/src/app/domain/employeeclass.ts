import { Employee } from '../domain/employee';
export class EmployeeClass implements Employee{

    constructor(
        public firstName?,
        public lastName?,
        public mobilePhone?,
        public emailAddress?,
        public photo?,
        public officePhone?,
        public extension?,
        public employeeId?,){}
}