import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Appointment} from '../domain/appointment';
import { HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class AppointmentService
{
    constructor(private http: HttpClient)
    {

    }

   getAppointment()
   {
    return this.http.get('http://localhost:65036/api/Appointments')
    .toPromise()
    .then(data => {return data as Appointment[];});
   }

   createAppointment(cAppointment)
   {
    return this.http.post('http://localhost:65036/api/Appointments', cAppointment)
    .toPromise()
    .then(data => {return data as Appointment;});
   }

   updateAppointment(cAppointment, appointmentId)
   {
    return this.http.put('http://localhost:65036/api/Appointments/?id=' + appointmentId, cAppointment)
    .toPromise()
    .then(data => {return data as Appointment[];});
   }

   deleteAppointment(appointmentId)
   {
    return this.http.delete('http://localhost:65036/api/Appointments/?id=' + appointmentId)
    .toPromise()
    .then(() =>  null);
   }

}