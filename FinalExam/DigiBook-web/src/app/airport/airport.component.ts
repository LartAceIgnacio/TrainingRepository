import { Component, OnInit } from '@angular/core';
import { GlobalService } from "../services/globalService";
import { Airport } from "../domain/airport";
import { AirportClass } from "../domain/airportClass";

import { API_URL } from "../services/constants";

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css'],
  providers: [GlobalService]
})
export class AirportComponent implements OnInit {
  //airport variable
  airportList: Airport[];
  //url 
  service: string = 'airport';
  serviceUrl: string = `${API_URL}/${this.service}/`;
  apiUrl: string = 'https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1';
  //search
  searchFilter: string = "";
  //loading
  loading: boolean;

  constructor(private globalService : GlobalService) { }

  ngOnInit() {
    this.loading = true;    
    this.globalService.retrieveForAirport(this.serviceUrl, this.apiUrl).then(airport => { 
      this.airportList = airport;
      this.loading = false;
    });
  }
}
