
import { Injectable } from '@angular/core';
import { IAppointments } from '../domain/IAppointments';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';


@Injectable()
export class AppointmentService {
    
constructor(private httpClient: HttpClient) {}

    _getAppointments() {
        return this.httpClient.get('http://localhost:55168/api/appointment/')
                .toPromise()
                .then(data => {  return data as IAppointments[]; });
    }

    _addAppointment(appointment) {
        return this.httpClient.post('http://localhost:55168/api/appointment/', appointment)
                .toPromise()
                .then(data => { console.log("post contact: "+ JSON.stringify(data)); return data as IAppointments; });
    }

    _updateAppointment(appointment) {
        return this.httpClient.put('http://localhost:55168/api/appointment/?id='+ appointment.appointmentId, appointment)
                .toPromise()
                .then(data => { return data as IAppointments; });
    }

    _deleteAppointment(appointmendId) {
        return this.httpClient.delete('http://localhost:55168/api/appointment/?id='+ appointmendId)
                .toPromise()
                .then(() => null);
    }
}
