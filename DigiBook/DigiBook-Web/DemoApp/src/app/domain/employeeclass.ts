import { Employee } from './employee';

export class Employeeclass implements Employee{
    
        constructor(public employeeId?, public firstName?, public lastName?, public mobilePhone?,
                    public emailAddress?, public officePhone?, public extension?,
                    public notes?
                    ) { }
}