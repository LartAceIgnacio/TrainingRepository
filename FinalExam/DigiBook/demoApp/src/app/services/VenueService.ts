import {Injectable} from '@angular/core';
import {Venue} from '../domain/Venue';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable()
export class VenueService{

    constructor(private http:HttpClient){

    }

    getVenues(){
        return this.http.get('http://localhost:56416/api/venues')
        .toPromise()
        .then(data => {return data as Venue[];});
    }

    postVenues(postVenues){
        return this.http.post('http://localhost:56416/api/venues', postVenues)
        .toPromise()
        .then(data => {return data as Venue[];});
    }

    putVenues(venue: Venue){
        return this.http.put('http://localhost:56416/api/venues/?id='+venue.venueId, venue, venue.venueId)
        .toPromise()
        .then(()=> venue);
    }

    deleteVenues(id){
        console.log(id);
        return this.http.delete('http://localhost:56416/api/venues/?id='+id)
        .toPromise()
        .then(() => null);
    }
}