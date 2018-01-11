export interface PaginationResult<T> {
    results: Array<T>;
    pageNo: number;
    recordPage: number;
    totalRecords: number;
}