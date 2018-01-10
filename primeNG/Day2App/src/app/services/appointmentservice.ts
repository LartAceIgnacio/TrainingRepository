import { Injectable } from  '@angular/core';
import { Http, Response } from '@angular/http';
import { Appointment } from '../domain/appointment'
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppointmentService {
    constructor(private http: HttpClient) {}

    getAppointments() {
        return this.http.get('http://localhost:16013/api/Appointments/')
            .toPromise()
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }
    
    addAppointment(objAppointment: Appointment) {
        return this.http.post('http://localhost:16013/api/Appointments/', objAppointment)
            .toPromise()
            .then(data => { return data as Appointment[]; })
            .catch(this.handleError);
    }
    
    deleteAppointment(id) {
        return this.http.delete('http://localhost:16013/api/Appointments/?id=' + id)
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
    
    updateAppointment(id, objAppointment: Appointment) {
        return this.http.put('http://localhost:16013/api/Appointments/?id=' + id, objAppointment)
            .toPromise()
            .then(() => objAppointment)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
      }
}
