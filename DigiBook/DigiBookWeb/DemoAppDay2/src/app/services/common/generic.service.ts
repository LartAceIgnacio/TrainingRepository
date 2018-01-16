import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';
import { API_URL} from './Authentication/constants';


@Injectable()
export class GenericService {

  constructor(private http: HttpClient) { }

  // private headers = new Headers({ 'Content-Type': 'application/json' });
  private url = API_URL;

  // Data With Pagination
  Retrieve<T>(entity: string, pageNumber: number, recordNumber: number, T): Promise<T> {
    const url = `${this.url}/${entity}/${pageNumber}/${recordNumber}/?query=${T}`;
    console.log(url);
    return this.http.get(url)
      .toPromise()
      .then(data => {
        return data as T
      })
      .catch(this.handleError)
  }

  // Data by id 
  RetrieveById<T>(entity: string, id: string): Promise<T> {
    const url = `${this.url}/${entity}?id=${id}`;
    console.log(url);
    return this.http.get(url)
      .toPromise()
      .then(data => {
        return data as T
      })
      .catch(this.handleError);
  }


  Create<T>(entity: string, T): Promise<T> {
    const url = `${this.url}/${entity}`;
    return this.http
      .post(url, T)
      .toPromise()
      .then(data => {
        console.log(data);
        return data as T;
      })
      .catch(this.handleError);
  }

  Delete<T>(entity: string, id: number): Promise<T> {
    const url = `${this.url}/${entity}?id=${id}`;
    return this.http.delete(url)
      .toPromise()
      .then(data => {
        return 204;
      })
      .catch(this.handleError);
  }

  Update<T>(entity: string, id: number, T): Promise<T> {
    const url = `${this.url}/${entity}?id=${id}`;
    return this.http
      .put(url, T)
      .toPromise()
      .then(data => {
        console.log(data);
        return data as T;
      })
      .catch(this.handleError);
  }


  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}
