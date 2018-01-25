import {Injectable} from '@angular/core';
import {Appointment} from '../domain/Appointment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class AppointmentService{

    service: string = 'appointments';
    serviceUrl: string = `${API_URL}/${this.service}/`;

    constructor(private http:HttpClient){

    }

    getAppointments(){
        return this.http.get(this.serviceUrl)
        .toPromise()
        .then(data => {return data as Appointment[];});
    }

    postAppointments(postAppointments){
        return this.http.post(this.serviceUrl, postAppointments)
        .toPromise()
        .then(data => {return data as Appointment;});
    }

    putAppointments(appointment: Appointment){
        console.log(appointment);
        return this.http.put(`${this.serviceUrl}?id=${appointment.appointmentId}`, appointment)
        .toPromise()
        .then(() => appointment);
    }

    deleteAppointments(appointmentId){
        return this.http.delete(`${this.serviceUrl}?id=${appointmentId}`)
        .toPromise()
        .then(() => null);
    }
}