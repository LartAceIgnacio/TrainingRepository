
import { Injectable } from '@angular/core';
import { Appointment } from '../domain/appointment';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import { API_URL} from './constants';
import { Http, Response } from '@angular/http';



@Injectable()
export class AppointmentService {
    service: string = 'appointments';
    serviceUrl: string = `${API_URL}/${this.service}/`;
    
constructor(private http: HttpClient) {}

    getAppointments() {
        return this.http.get('http://localhost:52369/api/appointments/')
                .toPromise()
                .then(data => { return data as Appointment[]; });
    }
    addAppointments(addAppointments) {
        return this.http.post('http://localhost:52369/api/appointments/',addAppointments)
                .toPromise()
                .then(data => { return data as Appointment; });
    }
    saveAppointments(appointmentId , appointment) {
        return this.http.put('http://localhost:52369/api/appointments/?id='+ appointment.appointmentId, appointment)
                .toPromise()
                .then(data => { return data as Appointment; });
    }
    deleteAppointments(appointmentId) {
        return this.http.delete('http://localhost:52369/api/appointments/?id='+ appointmentId)
                .toPromise()
                .then(()=>null);
    }
}
