import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem, ConfirmationService , DataTable} from 'primeng/primeng';
import { Flight } from '../domain/flight';
import { GlobalService } from '../services/globalservice';
import { FlightClass } from '../domain/FlightClass';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Pagination } from '../domain/pagination';
import { MaxLengthValidator } from '@angular/forms/src/directives/validators';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class FlightsComponent implements OnInit {
  totalRecords: number = 0;
  paginationResult: Pagination<Flight>;
  searchFilter: string = "";
  cloneFlight: any;
  displayDialog: boolean;
  selectedFlight: Flight;
  isNewFlight: boolean;
  btnSave: boolean;
  btnDelete: boolean;
  btnSaveNew: boolean;
  flightItems: MenuItem[];
  clonedSeletedFLight: Flight;
  flight: Flight;
  flightList: Flight[];
  display: boolean = false;
  userform: FormGroup;

  showDialog() {
    this.display = true;
  }
  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
  private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.userform = this.fb.group({
      'cityOfOrigin': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(3), Validators.minLength(3)])),
      'cityOfDestination': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(3), Validators.minLength(3)])),
      'expectedTimeOfArrival': new FormControl('', Validators.required),
      'expectedTimeOfDeparture': new FormControl('', Validators.required)
    });
    this.flightItems = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Flight', routerLink: ['/flight'] }
    ]
  }
  addFlight() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewFlight = true;
    this.selectedFlight = new FlightClass;
    this.displayDialog = true;
    this.userform.markAsPristine();
  }
  
  saveFlight(){
    this.userform.enable();
    let tmpFlightList = [...this.flightList];
    if(this.isNewFlight){
      this.globalService.addSomething("Flights" , this.selectedFlight).then(flights => {
      this.flight = flights;
      tmpFlightList.push(this.flight);
      this.flightList = tmpFlightList;
      });
    } else {
      this.globalService.updateSomething("Flights", this.selectedFlight.flightId , this.selectedFlight);
      tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
    }
    this.selectedFlight = null;
    this.isNewFlight = false;
    this.userform.markAsPristine();
  }

  saveNewFlight(){
    this.userform.enable();
    let tmpFlightList = [...this.flightList];
    if(this.isNewFlight){
      this.globalService.addSomething("Flights" , this.selectedFlight).then(flights => {
      this.flight = flights;
      tmpFlightList.push(this.flight);
      this.flightList = tmpFlightList;
      });
    } else {
      this.globalService.updateSomething("Flights", this.selectedFlight.flightId , this.selectedFlight);
      tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
    }
    this.selectedFlight = new FlightClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Flight : Flight){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedFlight = Flight;
  }

  findselectedFlightIndex(): number {
    return this.flightList.indexOf(this.selectedFlight);
  }
  deleteFlight(){
    let index = this.findselectedFlightIndex();
    this.flightList = this.flightList.filter((val, i) => i != index);
    // this.globalService.deleteSomething("Flights", this.selectedFlight.flightId);
    this.selectedFlight = null;
    this.displayDialog = false;
  }

  cloneRecord(r: Flight): Flight {
    let flight = new FlightClass();
    for (let prop in r) {
      flight[prop] = r[prop];
    }
    return flight;
  }

  editFlight(Flight: Flight){
    this.userform.enable();
    this.btnSave = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.displayDialog = true;
    this.isNewFlight = false;
    this.selectedFlight = Flight;
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.userform.markAsPristine();
  }
  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Flight>>("Flights", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.flightList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
      });
  }

  searchFlight() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    let paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }
  
  cancelFlight(){
    this.isNewFlight = false;
    let tmpFlightList = [...this.flightList];
    tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.cloneFlight;
    this.flightList = tmpFlightList;
    this.selectedFlight = this.cloneFlight;
    this.selectedFlight = null;
  }
}
