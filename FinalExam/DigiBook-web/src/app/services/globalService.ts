import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class GlobalService {
    constructor(private http: HttpClient){}

    retrieve<T>(path: string){
        return this.http.get(path)
        .toPromise()
        .then( data => {return data as T[]; });
    }

    retrieveWithPagination<T> (path: string, page: number, record: number, filter: string) {
        return this.http.get( path + page + '/' + record + '?filter=' + filter)
            .toPromise()
            .then(data => { return data as T; });
    }

    retrieveForAirport<T> (path: string, url: string) {
        return this.http.get( path + '?url=' + url)
            .toPromise()
            .then(data => { return data as T[]; });
    }

    add<T>(path: string, entity: any){
        return this.http.post(path,entity)
        .toPromise()
        .then( data => { return data as T; });
    }

    update<T>(path: string, entity: any, entityId: any){
        return this.http.put(path + entityId, entity, entityId)
        .toPromise()
        .then( () => entity );
    }

    delete<T>(path: string, entity: any, entityId: any){
        return this.http.delete(path + entityId, entityId)
        .toPromise()
        .then( () => entity );
    }
}