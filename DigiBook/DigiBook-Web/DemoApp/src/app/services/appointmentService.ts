import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Appointment } from "../domain/appointment";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class AppointmentService {
    
constructor(private http: HttpClient) {}

    getAppointments() {
        return this.http.get('http://localhost:55775/api/appointments/')
                .toPromise()
                .then(data => { return data as Appointment[]; });
    }
    addAppointments(addAppointments) {
        return this.http.post('http://localhost:55775/api/appointments/',addAppointments)
                .toPromise()
                .then(data => { return data as Appointment; });
    }
    saveAppointments(id,appointment) {
        return this.http.put('http://localhost:55775/api/appointments/?id='+ id, appointment)
                .toPromise()
                .then(data => { return data as Appointment[]; });
    }
    deleteAppointments(appointmentId) {
        return this.http.delete('http://localhost:55775/api/appointments/?id='+ appointmentId)
                .toPromise()
                .then(()=>null);
    }
}
