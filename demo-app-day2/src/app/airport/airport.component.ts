import { Component, OnInit, ViewChild } from '@angular/core';
import { IAirports } from '../domain/IAirports';
import { AirportClass } from '../domain/airports.class';
import { AirportService } from '../services/airport.service';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { DataTable } from 'primeng/primeng';

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css'],
  providers: [AirportService]
})
export class AirportComponent implements OnInit {

  airportInfoList: IAirports[];
  filterText: string;

  constructor(private _httpClient: HttpClient, private aiportService: AirportService) { }
  
  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.getAiportsInfo();
  }

  getAiportsInfo() {
    this.aiportService._getAirportsInfo()
      .then(info => this.airportInfoList = info);
  }

  filterAirports(params) {
    var item = this.airportInfoList.find(params);
  }


  setCurrentPage(n : number){
    this.dataTable.reset();
  }

  searchAirport(){
    if(this.filterText.length != 1){
      this.setCurrentPage(1);
    }
  }

}
