
import { Injectable } from '@angular/core';
import { IAirports } from '../domain/IAirports';
import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { IATA_URL } from './constants';
import 'rxjs/add/operator/toPromise';



@Injectable()
export class AirportService {
    
constructor(private httpClient: HttpClient) {}


    _getAirportsInfo() {
        return this.httpClient.get('http://localhost:55168/api/airport?url='+ IATA_URL)
                .toPromise()
                .then(data => { console.log("Get Employee:" + JSON.stringify(data as IAirports[])); return data as IAirports[]; });
    }

    _getAirportsInfoByCode(code){
      return this.httpClient.get('http://localhost:55168/api/airport?code='+code)
                .toPromise()
                .then(data => { console.log("Get Employee:" + JSON.stringify(data as IAirports)); return data as IAirports; });
    }

}
