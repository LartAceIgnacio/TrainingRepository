import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';
import { API_URL} from './constants';

@Injectable()
export class RegistrationService {

  constructor(private http: HttpClient) { }

  private url = API_URL;
 
  Register(tempUser : User, entity:string): any {
    const url = `${this.url}/${entity}`;
    return this.http
      .post(url, tempUser)
      .toPromise()
      .then(data => {
        console.log(data);
        return data;
      })
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }

}
