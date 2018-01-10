import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Venue } from '../domain/venue'
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VenueService {
    constructor(private http: HttpClient) {}

    getVenues() {
        return this.http.get('http://localhost:16013/api/Venues/')
            .toPromise()
            .then(data => { return data as Venue[]; })
            .catch(this.handleError);
    }
    
    addVenue(objVenue: Venue) {
        return this.http.post('http://localhost:16013/api/Venues/', objVenue)
            .toPromise()
            .then(data => { return data as Venue[]; })
            .catch(this.handleError);
    }
    
    deleteVenue(id) {
        return this.http.delete('http://localhost:16013/api/Venues/?id=' + id)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateVenue(id, objVenue: Venue) {
        return this.http.put('http://localhost:16013/api/Venues/?id=' + id, objVenue)
            .toPromise()
            .then(() => objVenue)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
