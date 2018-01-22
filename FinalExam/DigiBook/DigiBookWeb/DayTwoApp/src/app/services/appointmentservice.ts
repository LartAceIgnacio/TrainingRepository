import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Appointment } from '../domain/appointments/appointment';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';
import { API_URL } from './constants';

@Injectable()
export class AppointmentService {
    // tslint:disable-next-line:no-inferrable-types
    service: string = 'appointments';
    // tslint:disable-next-line:no-inferrable-types
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) { }

    getAppointments() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }

    postAppointments(AppointmentToPost: Appointment) {
        return this.http.post(this.serviceUrl, AppointmentToPost)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }

    putAppointments(appointmentId, appointmentToPut: Appointment) {
        return this.http.put(`${this.serviceUrl}?id=${appointmentId}`, appointmentToPut)
            .toPromise()
            // tslint:disable-next-line:arrow-return-shorthand
            .then(() => appointmentToPut)
            .catch(this.handleError);
    }

    deleteAppointments(appointmentId) {
        return this.http.delete(`${this.serviceUrl}?$id=${appointmentId}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}
