import { Department } from "./Department";

export class DepartmentClass implements Department {
    constructor(
        public departmentId?,
        public departmentHeadId?,
        public departmentName?
    ){}
}