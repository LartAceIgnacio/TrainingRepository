import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Flight } from '../domain/flight';
import { GlobalService } from '../services/globalservice';
import { FlightClass } from '../domain/FlightClass';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Pagination } from '../domain/pagination';
import { MaxLengthValidator } from '@angular/forms/src/directives/validators';
import { DatePipe } from '@angular/common';
import { Message } from 'primeng/components/common/api';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.css'],
  providers: [GlobalService, ConfirmationService, DatePipe]
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
  today: Date;
  msgs: Message[] = [];
  UpperCaseDigitRejex: string = "([A-Z]{3})";

  showDialog() {
    this.display = true;
  }
  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder, private datePipe: DatePipe) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.userform = this.fb.group({
      'cityOfOrigin': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(3), Validators.minLength(3), Validators.pattern(this.UpperCaseDigitRejex)])),
      'cityOfDestination': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(3), Validators.minLength(3), Validators.minLength(3), Validators.pattern(this.UpperCaseDigitRejex)])),
      'expectedTimeOfArrival': new FormControl('', Validators.required),
      'expectedTimeOfDeparture': new FormControl('', Validators.required)
    });
    this.flightItems = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Flight', routerLink: ['/flight'] }
    ]
    this.today = new Date();
    let yesterday = new Date();
    this.today.setDate(yesterday.getDate());
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

  saveFlight() {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];
        let tmpFlightList = [...this.flightList];
        if (this.isNewFlight) {
          this.globalService.addSomething("Flights", this.selectedFlight).then(flights => {
            this.flight = flights;
            flights.expectedTimeOfArrival = new Date(flights.expectedTimeOfArrival).toLocaleString();
            flights.expectedTimeOfDeparture = new Date(flights.expectedTimeOfDeparture).toLocaleString();
            tmpFlightList.push(this.flight);
            this.flightList = tmpFlightList;
          });
        } else {
          this.globalService.updateSomething<Flight>("Flights", this.selectedFlight.flightId, this.selectedFlight)
            .then(flights => {
              this.selectedFlight.expectedTimeOfArrival = new Date(this.selectedFlight.expectedTimeOfArrival).toLocaleString();
              this.selectedFlight.expectedTimeOfDeparture = new Date(this.selectedFlight.expectedTimeOfDeparture).toLocaleString();
              tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
              this.flightList = tmpFlightList;
            });
        }
        this.selectedFlight = null;
        this.isNewFlight = false;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
    this.userform.markAsPristine();
  }

  saveNewFlight() {
    this.userform.enable();
    let tmpFlightList = [...this.flightList];
    if (this.isNewFlight) {
      this.globalService.addSomething("Flights", this.selectedFlight).then(flights => {
        this.flight = flights;
        flights.expectedTimeOfArrival = new Date(flights.expectedTimeOfArrival).toLocaleString();
        flights.expectedTimeOfDeparture = new Date(flights.expectedTimeOfDeparture).toLocaleString();
        tmpFlightList.push(this.flight);
        this.flightList = tmpFlightList;
      });
    } else {
      this.globalService.updateSomething<Flight>("Flights", this.selectedFlight.flightId, this.selectedFlight)
        .then(flights => {
          this.selectedFlight.expectedTimeOfArrival = new Date(this.selectedFlight.expectedTimeOfArrival).toLocaleString();
          this.selectedFlight.expectedTimeOfDeparture = new Date(this.selectedFlight.expectedTimeOfDeparture).toLocaleString();
          tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
          this.flightList = tmpFlightList;
        });
    }
    this.selectedFlight = new FlightClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Flight: Flight) {
    this.userform.disable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedFlight = Flight;
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.selectedFlight.expectedTimeOfArrival = new Date(this.selectedFlight.expectedTimeOfArrival);
    this.selectedFlight.expectedTimeOfDeparture = new Date(this.selectedFlight.expectedTimeOfDeparture);
  }

  findselectedFlightIndex(): number {
    return this.flightList.indexOf(this.selectedFlight);
  }
  deleteFlight() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];
        let index = this.findselectedFlightIndex();
        this.flightList = this.flightList.filter((val, i) => i != index);
        this.globalService.deleteSomething("Flights", this.selectedFlight.flightId);
        this.selectedFlight = null;
        this.displayDialog = false;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
  }

  cloneRecord(r: Flight): Flight {
    let flight = new FlightClass();
    for (let prop in r) {
      flight[prop] = r[prop];
    }
    return flight;
  }

  editFlight(Flight: Flight) {
    this.userform.enable();
    this.btnSave = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.btnSaveNew = false;
    this.displayDialog = true;
    this.isNewFlight = false;
    this.selectedFlight = Flight;
    console.log(Flight);
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.selectedFlight.expectedTimeOfArrival = new Date(this.selectedFlight.expectedTimeOfArrival);
    this.selectedFlight.expectedTimeOfDeparture = new Date(this.selectedFlight.expectedTimeOfDeparture);
    this.userform.markAsPristine();
  }
  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Flight>>("Flights", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.flightList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (let i = 0; i < this.flightList.length; i++) {
          this.flightList[i].expectedTimeOfArrival = new Date(this.flightList[i].expectedTimeOfArrival).toLocaleString();
          this.flightList[i].expectedTimeOfDeparture = new Date(this.flightList[i].expectedTimeOfDeparture).toLocaleString();
          // this.flightList[i].expectedTimeOfArrival = this.datePipe.transform(this.flightList[i].expectedTimeOfArrival, 'MM-dd-yyyy HH:mm:ss');
          // this.flightList[i].expectedTimeOfDeparture = this.datePipe.transform(this.flightList[i].expectedTimeOfDeparture, 'MM-dd-yyyy HH:mm:ss');
        }
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

  cancelFlight() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
        this.msgs = [{ severity: 'info', summary: 'Confirmed', detail: 'You have accepted' }];
        this.isNewFlight = false;
        let tmpFlightList = [...this.flightList];
        tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.cloneFlight;
        this.flightList = tmpFlightList;
        this.selectedFlight = this.cloneFlight;
        this.selectedFlight = null;
      },
      reject: () => {
        this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
      }
    });
  }
}
