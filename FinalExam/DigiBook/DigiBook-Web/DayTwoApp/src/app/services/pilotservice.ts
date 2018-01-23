import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Pilot} from '../domain/pilots/pilot';
import 'rxjs/add/operator/toPromise';
import {HttpClient} from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class VenueService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'pilots';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}
    getPilots() {
        return this.http.get(this.serviceUrl)
        .toPromise()
        .then(data => data as Pilot[])
        .catch(this.handleError);
    }

    postPilots(objectPilot) {
        return this.http.post(this.serviceUrl, objectPilot)
        .toPromise()
        .then(data => data as Pilot[])
        .catch(this.handleError);
    }

    putPilots(pilotId, objectPilot) {
        return this.http.put(`${this.serviceUrl}?id=${pilotId}`, objectPilot)
        .toPromise()
        .then(data => data as Pilot[])
        .catch(this.handleError);
    }

    deletePilots(pilotId) {
        return this.http.delete(`${this.serviceUrl}?id=${pilotId}`)
        .toPromise()
        .then(() => null)
        .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
