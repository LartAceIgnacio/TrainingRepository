import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Venue} from '../domain/venue';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()

export class VenueService{

    constructor(private http: HttpClient) {}      

    getVenues(){
        return this.http.get('http://localhost:52675/api/venues')
        .toPromise()
        .then( data => { return data as Venue[]; });
    }

    addVenues(venue:Venue){
        return this.http.post('http://localhost:52675/api/venues',venue)
        .toPromise()
        .then( data => { return data as Venue; });
    }

    updateVenues(venue:Venue){
        return this.http.put('http://localhost:52675/api/venues?id='+venue.venueId ,venue, venue.venueId)
        .toPromise()
        .then( () => venue );
    }

    deleteVenues(venue:Venue){
        return this.http.delete('http://localhost:52675/api/venues?id='+venue.venueId ,venue.venueId)
        .toPromise()
        .then( () => venue );
    }
}