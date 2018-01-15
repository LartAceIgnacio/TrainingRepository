export interface PaginationResult<T> {
    results: Array<T>;
    pageNo: number;
    recordPage: number;
    totalCount: number;
}