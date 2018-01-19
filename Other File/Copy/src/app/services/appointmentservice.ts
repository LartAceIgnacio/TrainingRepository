import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Appointment } from '../domain/appointment';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class AppointmentService{
    constructor(private http: HttpClient){
    }
    
      getAppointment(){
        return this.http.get('http://localhost:49784/api/appointments')
                    .toPromise()
                    .then(result => { return result as Appointment[]; });
      }
    
      postAppointment(data){
         return this.http.post('http://localhost:49784/api/appointments', data)
                  .toPromise()
                  .then(result => {return result as Appointment});
      }
    
      putAppointment(id, data){
        return this.http.put('http://localhost:49784/api/appointments?id='+id, data)
                  .toPromise()
                  .then(result => {return result as Appointment});
      }
    
      deleteAppointment(id){
        return this.http.delete('http://localhost:49784/api/appointments?id='+id)
                  .toPromise()
                  .then(result => {});
      }
}
