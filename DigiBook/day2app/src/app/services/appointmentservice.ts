import { Injectable } from '@angular/core';
import { Http, Response} from '@angular/http';
import { Appointment } from '../domain/appointments/appointment';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppointmentService{
    constructor(private http: HttpClient){}

    getAppointments(){
        return this.http.get('http://localhost:52212/api/appointments')
        .toPromise()
        .then(data=>{return data as Appointment[]});
    }

    postAppointment(appointments){
        return this.http.post('http://localhost:52212/api/appointments',appointments)
        .toPromise()
        .then(data=>{return data as Appointment});
    }
    deleteAppointment(id){
        return this.http.delete('http://localhost:52212/api/appointments?id='+id)
        .toPromise()
        .then();
    }

    putAppointments(id,appointments){
        return this.http.put('http://localhost:52212/api/appointments/?id='+id,appointments)
        .toPromise()
        .then(data=>{return data as Appointment});
    }
}