import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../services/globalservice';
import { Airport } from '../domain/airports/airport';

@Component({
  selector: 'app-airports',
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.css'],
  providers: [GlobalService]
})
export class AirportsComponent implements OnInit {
  searchFilter: string = "";
  airportList: Airport[];

  constructor(private globalService: GlobalService) { }

  ngOnInit() {
    this.searchAirport();
  }

  searchAirport() {
    console.log("asd");
    this.globalService.getAirport<Airport>("Airports",this.searchFilter)
      .then(airport => { this.airportList = airport; });
  }

}
