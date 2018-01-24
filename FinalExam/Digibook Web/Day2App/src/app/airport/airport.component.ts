import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../services/globalservice';
import { DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { PaginationResult } from "../domain/paginationresult";
import { AuthService } from "../services/authservice";
import { Airport } from "../domain/airport";

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css'],
  providers: [GlobalService]
})
export class AirportComponent implements OnInit {
  airportList: Airport[];
  airportDefaultList: Airport[];
  selectedAirport: Airport;
  paginationResult: PaginationResult<Airport>;
  loading: boolean;
  baseUrl = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

  constructor(private globalService: GlobalService, public auth: AuthService) { }

  ngOnInit() {
    this.loading = true;
    this.globalService.getSomethingWithUrl<Airport>("Airports", this.baseUrl).then(airportResult => {
      this.airportList = airportResult;
      this.airportDefaultList = Object.assign([], this.airportList);
      this.loading = false;
    });
  }
}
