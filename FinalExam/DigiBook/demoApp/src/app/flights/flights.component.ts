import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbModule, MenuItem, DataTable, ConfirmationService, SelectItem } from 'primeng/primeng';
import { Flight } from "../domain/Flight";
import { GlobalService } from "../services/globalservice";
import { HttpClient } from "@angular/common/http";
import { PaginationResult } from "../domain/paginationresult";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { FlightClass } from "../domain/FlightClass";
import {Message} from 'primeng/components/common/api';
import {MessageService} from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.css'],
  providers: [GlobalService, ConfirmationService, MessageService]
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
  cities: SelectItem[];
  msgs: Message[] = [];

  constructor(
    private globalservice: GlobalService,
    private http: HttpClient,
    private fb:FormBuilder,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
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

    this.cities = [
      {label: 'AAV - Surallah', value: 'AAV'},
      {label: 'CBO - Cotabato', value: 'CBO'},
      {label: 'BCD - Bacolod', value: 'BCD'},
      {label: 'BNQ - Baganga', value: 'BNQ'},
      {label: 'BQA - Baler', value: 'BQA'},
      {label: 'BSO - Basco', value: 'BSO'},
      {label: 'BPH - Bislig', value: 'BPH'},
      {label: 'BXU - Butuan', value: 'BXU'},
      {label: 'CGY - Cagayan De Oro', value: 'CGY'},
      {label: 'CDY - Cagayan De Sulu', value: 'CDY'},
      {label: 'CYP - Calbayog', value: 'CYP'},
      {label: 'CYZ - Cauayan', value: 'CYZ'},
      {label: 'RZP - Taytay Sandoval', value: 'RZP'},
      {label: 'XCN - Coron', value: 'XCN'},
      {label: 'NCP - Luzon Is', value: 'NCP'},
      {label: 'CUJ - Culion', value: 'CUJ'},
      {label: 'CYU - Cuyo', value: 'CYU'},
      {label: 'MNL - Manila', value: 'MNL'}
    ];
    
  }

  paginate(event){
    this.globalservice.getRecordWithPagination<PaginationResult<Flight>>(
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

    this.globalservice.addRecord<Flight>("Flights", this.selectedFlight).then(flights =>{
      flights.eta = new Date(flights.eta).toLocaleString();
      flights.etd = new Date(flights.etd).toLocaleString();
      tmpFlightList.push(flights);
      this.flightList = tmpFlightList;
    });

    this.msgs = [];
    this.msgs.push({severity:'success', summary:'Success Message', detail:'Flight record added.'});
    this.isNewFlight = true;
    this.selectedFlight = new FlightClass();
  }

  saveFlight(){
    let tmpFlightList = [...this.flightList];
    if (this.isNewFlight) {
      this.globalservice.addRecord<Flight>("Flights", this.selectedFlight).then(flights =>{
        flights.eta = new Date(flights.eta).toLocaleString();
        flights.etd = new Date(flights.etd).toLocaleString();
        tmpFlightList.push(flights);
        this.flightList=tmpFlightList;
        this.msgs = [];
        this.msgs.push({severity:'success', summary:'Success!', detail:'Flight record added.'});
        this.selectedFlight = null;
      });
    } 
    else {
      this.globalservice.updateRecord<Flight>("Flights",this.selectedFlight.flightId,this.selectedFlight)
      .then(flights =>{
        this.selectedFlight.eta = new Date(this.selectedFlight.eta).toLocaleString();
        this.selectedFlight.etd = new Date(this.selectedFlight.etd).toLocaleString();
        tmpFlightList[this.flightList.indexOf(this.selectedFlight)] = this.selectedFlight;
        this.flightList=tmpFlightList;
        this.msgs = [];
        this.msgs.push({severity:'success', summary:'Success!', detail:'Flight record updated.'});
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
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
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
          this.display = false;
        },
        reject: () => {
          this.msgs = [{severity:'info', summary:'Rejected', detail:'Cancelled changes.'}];
          this.display = false;
        }
      });
    } 
  }

  confirmDelete(Flight: Flight){
    this.flightForm.markAsPristine();
    this.cloneFlight = this.cloneRecord(this.selectedFlight);
    this.selectedFlight=Flight;
    this.isDelete = true;
    this.display=true;
    this.flightForm.disable();
    this.isNewFlight = false;
    this.selectedFlight.eta = new Date(this.selectedFlight.eta);
    this.selectedFlight.etd = new Date(this.selectedFlight.etd);
  }

  deleteFlight(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let tmpFlightList = [...this.flightList];
        let index = this.findSelectedFlightIndex();
        this.flightList = this.flightList.filter((val,i) => i!=index);
        this.globalservice.deleteRecord<Flight>("Flights", this.selectedFlight.flightId);
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Record Deleted', detail:'Flight record deleted.'});
        this.selectedFlight = null;
      }
    });
  }

}
