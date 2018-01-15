import { Pagination } from "./pagination";

export class PaginationClass<T> implements Pagination<T> {
    constructor(

        public totalCount,
        public pageNumber,
        public recordNumber,
        public result
        
    ) { }
}