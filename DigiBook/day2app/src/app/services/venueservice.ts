import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Venue } from '../domain/venues/venue';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VenueService {
    constructor(private http: HttpClient) { }

    getVenues() {
        return this.http.get('http://localhost:52212/api/venues')
            .toPromise()
            .then(data => { return data as Venue[]; });
    }

    postVenues(venues) {
        return this.http.post('http://localhost:52212/api/venues', venues)
            .toPromise()
            .then(data => { return data as Venue[]; })
    }

    putVenues(id, venues) {
        return this.http.put('http://localhost:52212/api/venues/?id=' + id, venues)
            .toPromise()    
            .then(data => { return data as Venue[]; });
    }

    deleteVenues(id){
        return this.http.delete('http://localhost:52212/api/venues/?id='+id)
        .toPromise()
        .then();
    }
}