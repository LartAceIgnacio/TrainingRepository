import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Appointment} from '../domain/appointments/appointment';
import 'rxjs/add/operator/toPromise';
import {HttpClient} from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()

export class AppointmentService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'appointments';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}

    getAppointment() {
        return this.http.get(this.serviceUrl)
        .toPromise()
        // tslint:disable-next-line:arrow-return-shorthand
        .then(data => { return data as Appointment[]; })
        .catch(this.handleError);

    }
    postAppointment(objectAppointment) {
        return this.http.post(this.serviceUrl, objectAppointment)
        .toPromise()
        // tslint:disable-next-line:arrow-return-shorthand
        .then(data => { return data as Appointment[]; })
        .catch(this.handleError);
    }
    putAppointment(appointmentId, objectAppointment) {
        return this.http.put(`${this.serviceUrl}?id=${appointmentId}`, objectAppointment)
        .toPromise()
        // .then(res => <Employee[]> res.json().data)
        // tslint:disable-next-line:arrow-return-shorthand
        .then(data => { return data as Appointment[]; })
        .catch(this.handleError);
    }
    deleteAppointment(appointmentId) {
        return this.http.delete(`${this.serviceUrl}?id=${appointmentId}`)
        .toPromise()
        .then(() => null)
        .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}
