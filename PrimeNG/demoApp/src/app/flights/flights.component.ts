import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbModule, MenuItem, DataTable, ConfirmationService } from 'primeng/primeng';
import { Flight } from "../domain/Flight";
import { GlobalService } from "../services/globalservice";
import { HttpClient } from "@angular/common/http";
import { PaginationResult } from "../domain/paginationresult";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { FlightClass } from "../domain/FlightClass";

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class FlightsComponent implements OnInit {

  brFlight: MenuItem[];
  home: MenuItem;

  selectedFlight: Flight;
  cloneFlight: Flight;
  isNewFlight: boolean;

  flightForm: FormGroup
  display: boolean;
  isDelete: boolean;

  flightList: Flight[];
  paginationResult: PaginationResult<Flight>;
  searchFilter: string = "";
  totalRecords: number = 0;

  invalidDates: Date;

  constructor(
    private globalservice: GlobalService,
    private http: HttpClient,
    private fb:FormBuilder,
    private confirmationService: ConfirmationService
  ) { 
  }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.brFlight = [{label:'Flights', url:'/flights'}]
    this.home = {icon: 'fa fa-home', routerLink: '/dashboard'}

    this.flightForm = this.fb.group({
      'cityoforigin': new FormControl('', Validators.required),
      'cityofdestination': new FormControl('', Validators.required),
      'eta' : new FormControl('', Validators.required),
      'etd' : new FormControl('', Validators.required)
    });

    this.invalidDates = new Date();
    let yesterday = new Date();
    this.invalidDates.setDate(yesterday.getDate());
  }

  paginate(event){
    this.globalservice.getSomethingWithPagination<PaginationResult<Flight>>(
      "Flights", event.first, event.rows, this.searchFilter.length ==1? "" :
      this.searchFilter).then(paginationResult =>{
        this.paginationResult = paginationResult;
        this.flightList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for(var i=0; i < this.flightList.length; i++){
          this.flightList[i].eta = this.flightList[i].eta == null? null:
            new Date(this.flightList[i].eta).toLocaleString();
          this.flightList[i].etd = this.flightList[i].etd == null? null:
            new Date(this.flightList[i].etd).toLocaleString();
        }
      });
  }

  setCurrentPage(n: number){
    this.dataTable.reset();
  }

  searchFlight(){
    if(this.searchFilter.length != 1){
      this.setCurrentPage(1);
    }
  }

  addFlight(){
    this.isDelete = false;
    this.display = true;
    this.flightForm.markAsPristine();
    this.flightForm.enable();
    this.isNewFlight = true;
    this.selectedFlight = new FlightClass();
  }

  saveAndNewFlight(){
    this.flightForm.markAsPristine();

    let tmpFlightList = [...this.flightList];

    this.globalservice.addSomething<Flight>("Flights", this.selectedFlight).then(flights =>{
      flights.eta = new Date(flights.eta).toLocaleString();
      flights.etd = new Date(flights.etd).toLocaleString();
      tmpFlightList.push(flights);
      this.flightList = tmpFlightList;
    });

    this.isNewFlight = true;
    this.selectedFlight = new FlightClass();
  }

  saveFlight(){
    let tmpFlightList = [...this.flightList];
    if (this.isNewFlight) {
      this.globalservice.addSomething<Flight>("Flights", this.selectedFlight).then(flights =>{
        flights.eta = new Date(flights.eta).toLocaleString();
        flights.etd = new Date(flights.etd).toLocaleString();
        tmpFlightList.push(flights);
        this.flightList=tmpFlightList;
        this.selectedFlight = null;
      });
    } 
    else {
      this.globalservice.updateSomething<Flight>("Flights",this.selectedFlight.flightId,this.selectedFlight)
      .then(flights =>{
        this.selectedFlight.eta = new Date(this.selectedFlight.eta).toLocaleString();
        this.selectedFlight.etd = new Date(this.selectedFlight.etd).toLocaleString();
        tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
        this.flightList=tmpFlightList;
        this.selectedFlight=null;
      });
    }
    this.isNewFlight = false;
  }

  cloneRecord(r: Flight): Flight{
    let flight = new FlightClass();
    for(let prop in r){
      flight[prop] = r[prop];
    }
    return flight;
  }

  editFlight(Flight: Flight){
    this.flightForm.markAsPristine();
    this.flightForm.enable();
    this.isDelete = false;
    this.selectedFlight=Flight;
    //this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.display=true;
    this.isNewFlight = false;
    this.selectedFlight.eta = new Date(this.selectedFlight.eta);
    this.selectedFlight.etd = new Date(this.selectedFlight.etd);
  }

  findSelectedFlightIndex(): number{
    return this.flightList.indexOf(this.selectedFlight);
  }

  cancelFlight(){
    let index = this.findSelectedFlightIndex();
    if (this.isNewFlight) {
      this.selectedFlight = null;
    }
    else{
      this.confirmationService.confirm({
        message: 'Are you sure that you want discard changes?',
        header: 'Confirmation',
        icon: 'fa fa-question-circle',
        accept: () => {
          let tmpFlightList = [...this.flightList];
          tmpFlightList[index] = this.cloneFlight;
          this.flightList = tmpFlightList;
          this.selectedFlight = Object.assign({}, this.cloneFlight);
          this.display = false;
        }
      });
    } 
  }

}
