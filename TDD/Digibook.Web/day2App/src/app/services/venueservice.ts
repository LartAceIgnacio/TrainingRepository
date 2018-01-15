import { Injectable } from '@angular/core';
import { Venue } from '../domain/venue';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VenueService {
  constructor(private http: HttpClient){
  }

  getVenue(){
    return this.http.get('http://localhost:49784/api/venues')
                .toPromise()
                .then(result => { return result as Venue[]; });
  }

  postVenue(data){
     return this.http.post('http://localhost:49784/api/venues', data)
              .toPromise()
              .then(result => {return result as Venue});
  }

  putVenue(id, data){
    return this.http.put('http://localhost:49784/api/Venues?id='+id, data)
              .toPromise()
              .then(result => {return result as Venue});
  }

  deleteVenue(id){
    return this.http.delete('http://localhost:49784/api/Venues?id='+id)
              .toPromise()
              .then(result => {});
  }
}
