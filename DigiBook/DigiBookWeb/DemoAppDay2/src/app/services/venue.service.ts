import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Venue } from '../domain/venue/venue';
import 'rxjs/add/operator/toPromise';

import { HttpClient } from '@angular/common/http';

@Injectable()
export class VenueService {


    constructor(private http: Http) { }

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private url = 'http://localhost:7604/api/Venue';

    getVenues(): Promise<Venue[]> {
        return this.http.get(this.url)
            .toPromise()
            .then(response => {
                return response.json() as Venue[]
            })
            .catch(this.handleError);
    }

    getVenue(id: string): Promise<Venue> {
        const url = `${this.url}?id=${id}`;
        console.log(url);
        return this.http.get(url)
            .toPromise()
            .then(response => {
                return response.json() as Venue
            })
            .catch(this.handleError);
    }

    createVenue(venue: Venue): Promise<Venue> {
        console.log(JSON.stringify(venue));
        return this.http
            .post(this.url, JSON.stringify(venue), { headers: this.headers })
            .toPromise()
            .then(res => {
                console.log(res);
                return res.json() as Venue;
            })
            .catch(this.handleError);
    }

    deleteVenue(venue: Venue): Promise<number> {
        const url = `${this.url}?id=${venue.venueId}`;
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(res => {
                    console.log(res);
                    return res.status;
            })
            .catch(this.handleError);
    }

    updateVenue(venue: Venue): Promise<Venue> {
        const url = `${this.url}?id=${venue.venueId}`;
        // venue.venueId = "";
        return this.http
          .put(url, JSON.stringify(venue), { headers: this.headers })
          .toPromise()
          .then(res => {
              console.log(res);
              return res.json() as Venue;
          })
          .catch(this.handleError);
      }
    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

}