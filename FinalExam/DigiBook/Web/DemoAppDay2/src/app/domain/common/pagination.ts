export interface Pagination<T> {
    
    totalCount: number,
    pageNumber: number,
    recordNumber: number,
    result: T[]

}