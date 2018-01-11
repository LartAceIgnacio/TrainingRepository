import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Appointment } from '../domain/appointment'
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class AppointmentService {
    service: string = 'appointments';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http: HttpClient) {}

    getAppointments() {
        return this.http.get(this.serviceUrl)
            .toPromise()
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }
    
    addAppointment(objAppointment: Appointment) {
        return this.http.post(this.serviceUrl, objAppointment)
            .toPromise()
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }
    
    deleteAppointment(id) {
        return this.http.delete(`${this.serviceUrl}?id=${id}`)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateAppointment(id, objAppointment: Appointment) {
        return this.http.put(`${this.serviceUrl}?id=${id}`, objAppointment)
            .toPromise()
            .then(() => objAppointment)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
