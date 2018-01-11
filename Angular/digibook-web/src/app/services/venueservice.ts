
import { Injectable } from '@angular/core';
import { Venue } from '../domain/venue';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class VenueService {
    
constructor(private http: HttpClient) {}

    getVenues() {
        return this.http.get('http://localhost:52369/api/venues/')
                .toPromise()
                .then(data => { return data as Venue[]; });
    }
    addVenues(addVenue) {
        return this.http.post('http://localhost:52369/api/venues/',addVenue)
                .toPromise()
                .then(data => { return data as Venue; });
    }
    saveVenues(venue) {
        return this.http.put('http://localhost:52369/api/venues/?id='+ venue.venueId, venue)
                .toPromise()
                .then(data => { return data as Venue[]; });
    }
    deleteVenues(venueId) {
        return this.http.delete('http://localhost:52369/api/venues/?id='+ venueId)
                .toPromise()
                .then(()=>null);
    }
}
