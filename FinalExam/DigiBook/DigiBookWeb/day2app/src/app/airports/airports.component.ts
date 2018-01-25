import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../services/globalservice';
import { Airport } from '../domain/airports/airport';
import { LazyLoadEvent } from 'primeng/primeng';

@Component({
  selector: 'app-airports',
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.css'],
  providers: [GlobalService]
})
export class AirportsComponent implements OnInit {
  searchFilter: string = "";
  airportList: Airport[];

  url: string = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";
  constructor(private globalService: GlobalService) { }

  ngOnInit() {
    this.searchAirport();
  }

  searchAirport() {
    console.log("asd");
    this.globalService.getAirport<Airport>("Airports", this.searchFilter, this.url)
      .then(airport => { this.airportList = airport; });
  }

}
