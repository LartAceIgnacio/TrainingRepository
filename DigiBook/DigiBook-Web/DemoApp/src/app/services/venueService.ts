import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Venue } from "../domain/venue";

@Injectable()
export class VenueService {
    
constructor(private http: HttpClient) {}

    getVenues() {
        return this.http.get('http://localhost:55775/api/venues/')
                .toPromise()
                .then(data => { return data as Venue[]; });
    }
    addVenues(addVenues) {
        return this.http.post('http://localhost:55775/api/venues/',addVenues)
                .toPromise()
                .then(data => { return data as Venue[]; });
    }
    saveVenues(venue) {
        return this.http.put('http://localhost:55775/api/venues/?id='+ venue.venueId, venue)
                .toPromise()
                .then(data => { return data as Venue[]; });
    }
    deleteVenues(venueId) {
        return this.http.delete('http://localhost:55775/api/venues/?id='+ venueId)
                .toPromise()
                .then(()=>null);
    }
}
