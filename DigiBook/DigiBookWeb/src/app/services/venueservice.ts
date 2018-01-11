import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Venue } from '../domain/venue'
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class VenueService {
    service: string = 'venues';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}

    getVenues() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            .then(data => { return data as Venue[]; })
            .catch(this.handleError);
    }
    
    addVenue(objVenue: Venue) {
        return this.http.post(this.serviceUrl, objVenue)
            .toPromise()
            .then(data => { return data as Venue[]; })
            .catch(this.handleError);
    }
    
    deleteVenue(id) {
        return this.http.delete(`${this.serviceUrl}?id=${id}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateVenue(id, objVenue: Venue) {
        return this.http.put(`${this.serviceUrl}?id=${id}`, objVenue)
            .toPromise()
            .then(() => objVenue)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
