import { Component, OnInit } from '@angular/core';
import { IAirports } from '../domain/IAirports';
import { AirportClass } from '../domain/airports.class';
import { AirportService } from '../services/airport.service';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css'],
  providers: [ AirportService ]
})
export class AirportComponent implements OnInit {

  airportInfoList: IAirports[];

  constructor(private _httpClient: HttpClient,  private aiportService: AirportService) { }

  ngOnInit() {
    this.getAiportsInfo();
  }

  getAiportsInfo() {
    this.aiportService._getAirportsInfo()
      .then(info => this.airportInfoList = info);
  }

}
