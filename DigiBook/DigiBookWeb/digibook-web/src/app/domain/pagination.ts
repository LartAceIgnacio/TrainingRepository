export interface Pagination<T> {
    results: Array<T>;
    pageNo: number;
    recordPage: number;
    totalRecords: number;
}