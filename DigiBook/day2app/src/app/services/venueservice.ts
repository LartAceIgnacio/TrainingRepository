import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Venue} from '../domain/venue';
import { HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class VenueService
{
    constructor(private http: HttpClient)
    {

    }

   getVenues()
   {
    return this.http.get('http://localhost:65036/api/Venues')
    .toPromise()
    .then(data => {return data as Venue[];});
   }

   createVenues(createdVenue)
   {
    return this.http.post('http://localhost:65036/api/Venues', createdVenue)
    .toPromise()
    .then(data => {return data as Venue;});
   }

   updateVenues(cVenue,venueId)
   {
    return this.http.put('http://localhost:65036/api/Venues/?id=' + venueId, cVenue)
    .toPromise()
    .then(data => {return data as Venue[];});
   }

   deleteVenues(venueId)
   {
    return this.http.delete('http://localhost:65036/api/Venues/?id=' + venueId)
    .toPromise()
    .then(() => null);
   }

}