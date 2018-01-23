import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Venue} from '../domain/venues/venue';
import 'rxjs/add/operator/toPromise';
import {HttpClient} from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class VenueService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'venues';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}
    getVenues() {
        return this.http.get(this.serviceUrl)
        .toPromise()
        .then(data => data as Venue[])
        .catch(this.handleError);
    }

    postVenues(objectVenue) {
        return this.http.post(this.serviceUrl, objectVenue)
        .toPromise()
        .then(data => data as Venue[])
        .catch(this.handleError);
    }

    putVenues(venueId, objectVenue) {
        return this.http.put(`${this.serviceUrl}?id=${venueId}`, objectVenue)
        .toPromise()
        .then(data => data as Venue[])
        .catch(this.handleError);
    }

    deleteVenues(venueId) {
        return this.http.delete(`${this.serviceUrl}?id=${venueId}`)
        .toPromise()
        .then(() => null)
        .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
