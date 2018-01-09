import {Injectable} from '@angular/core';
import {Appointment} from '../domain/Appointment';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable()
export class AppointmentService{
    constructor(private http:HttpClient){

    }

    getAppointments(){
        return this.http.get('http://localhost:56416/api/appointments')
        .toPromise()
        .then(data => {return data as Appointment[];});
    }

    postAppointments(postAppointments){
        return this.http.post('http://localhost:56416/api/appointments', postAppointments)
        .toPromise()
        .then(data => {return data as Appointment;});
    }

    putAppointments(appointment: Appointment){
        return this.http.put('http://localhost:56416/api/appointments/?id='+ appointment.appointmentId, appointment, appointment.appointmentId)
        .toPromise()
        .then(() => appointment);
    }

    deleteAppointments(appointmentId){
        return this.http.delete('http://localhost:56416/api/appointments/?id='+ appointmentId)
        .toPromise()
        .then(() => null);
    }
}