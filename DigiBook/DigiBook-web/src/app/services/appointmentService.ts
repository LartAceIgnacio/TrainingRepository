import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Appointment} from '../domain/appointment';
import {Contact} from '../domain/contact';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()

export class AppointmentService{

    constructor(private http: HttpClient) {}  

    getAppointments(){
        return this.http.get('http://localhost:52675/api/appointments')
        .toPromise()
        .then( data => {return data as Appointment[]; });
    }

    addAppointments(appointment:Appointment){
        return this.http.post('http://localhost:52675/api/appointments', appointment) 
        .toPromise()
        .then( data => {return data as Appointment });
    }

    updateAppointments(appointment:Appointment){
        return this.http.put('http://localhost:52675/api/appointments?id='+appointment.appointmentId, appointment, appointment.appointmentId)
        .toPromise()
        .then( () => appointment );
    }

    deleteAppointments(appointment:Appointment){
        return this.http.delete('http://localhost:52675/api/appointments?id='+appointment.appointmentId)
        .toPromise()
        .then( () => appointment );
    }
}