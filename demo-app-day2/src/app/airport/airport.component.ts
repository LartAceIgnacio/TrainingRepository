import { Component, OnInit, ViewChild } from '@angular/core';
import { IAirports } from '../domain/IAirports';
import { AirportClass } from '../domain/airports.class';
import { AirportService } from '../services/airport.service';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { DataTable } from 'primeng/primeng';
import { LazyLoadEvent } from 'primeng/components/common/api';

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css'],
  providers: [AirportService]
})
export class AirportComponent implements OnInit {

  airportInfoList: IAirports[];
  filterText: string;
  totalNumberOfRecords: number;
  loading: boolean;
  

  constructor(private _httpClient: HttpClient, private aiportService: AirportService) { }

  ngOnInit() {
    this.getAiportsInfo();
  }

  getAiportsInfo() {
    this.aiportService._getAirportsInfo()
      .then(info => {
        this.airportInfoList = info;
        this.totalNumberOfRecords = this.airportInfoList.length;
      });
  }

}
