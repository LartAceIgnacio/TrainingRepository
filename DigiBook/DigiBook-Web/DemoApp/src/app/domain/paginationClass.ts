import { Pagination } from "./pagination";
import { Contact } from "./contact";


export class PaginationClass implements Pagination<Contact>{
    constructor (public results, public pageNo, public recordPage, public totalRecords) 
    {
        
    }
}