import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem, ConfirmationService , DataTable} from 'primeng/primeng';
import { Flight } from '../domain/flight';
import { GlobalService } from '../services/globalservice';
import { FlightClass } from '../domain/FlightClass';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Pagination } from '../domain/pagination';

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
      'cityOfOrigin': new FormControl('', Validators.required),
      'cityOfDestination': new FormControl('', Validators.required),
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
      this.globalService.addSomething("flights" , this.selectedFlight).then(flights => {
      this.flight = flights;
      tmpFlightList.push(this.flight);
      this.flightList = tmpFlightList;
      });
    } else {
      this.globalService.updateSomething("flights", this.selectedFlight.flightId , this.selectedFlight);
      tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
    }
    this.selectedFlight = null;
    this.isNewFlight = false;
    this.userform.markAsPristine();
  }

  saveNewFlight(){
    this.userform.enable();
    let tmpflightList = [...this.flightList];
    if(this.isNewFlight){
      this.globalService.addSomething("flights" , this.selectedFlight).then(flights => {
      this.flight = flights;
      tmpflightList.push(this.flight);
      this.flightList = tmpflightList;
      console.log(this.flight);
      });
    } else {
      this.globalService.updateSomething("flights", this.selectedFlight.flightId , this.selectedFlight);
      tmpflightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
    }
    this.selectedFlight = new FlightClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(flight : Flight){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedFlight = flight;
  }

  findselectedFlightIndex(): number {
    return this.flightList.indexOf(this.selectedFlight);
  }
  deleteFlight(){
    let index = this.findselectedFlightIndex();
    this.flightList = this.flightList.filter((val, i) => i != index);
    this.globalService.deleteSomething("flights", this.selectedFlight.flightId);

  }

  cloneRecord(r: Flight): Flight {
    let flight = new FlightClass();
    for (let prop in r) {
      flight[prop] = r[prop];
    }
    return flight;
  }

  editFlight(flight: Flight){
    this.userform.enable();
    this.btnSave = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewFlight = false;
    this.selectedFlight = flight;
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.userform.markAsPristine();
  }

}
