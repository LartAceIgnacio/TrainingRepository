import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class GlobalService {



    constructor(private http: HttpClient) {}

    getSomethingWithPagination<T> (serviceName: string, page: number, 
        record: number, filter: string) {
        return this.http.get(`${API_URL}/${serviceName}/${page}/${record}?query=${filter}`)
            .toPromise()
            .then(data => { return data as T; })
            .catch(this.handleError);
    }

    getSomething<T> (serviceName: string) {
        return this.http.get(`${API_URL}/${serviceName}`)
            .toPromise()
            .then(data => { return data as T[]; })
            .catch(this.handleError);
    }
    
    addSomething<T>(serviceName: string, objEntity: T) {
        return this.http.post(`${API_URL}/${serviceName}`, objEntity)
            .toPromise()
            .then(data => { return data as T; })
            .catch(this.handleError);
    }
    
    deleteSomething<T>(serviceName: string, id) {
        return this.http.delete(`${API_URL}/${serviceName}?id=${id}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateSomething<T>(serviceName: string, id, objEntity: T) {
        return this.http.put(`${API_URL}/${serviceName}?id=${id}`, objEntity)
            .toPromise()
            .then(() => objEntity)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
