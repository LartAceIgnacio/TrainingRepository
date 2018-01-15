import { Component, OnInit } from '@angular/core';
import { VenueService } from './../services/venueService';
import { Venue } from './../domain/venue';
import { VenueClass } from './../domain/venueclass';
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import {Message,SelectItem} from 'primeng/components/common/api';
import { GlobalService } from '../services/globalservice';
import { MenuItem, ConfirmationService , DataTable } from "primeng/primeng";
import { ViewChild } from '@angular/core';
import { Pagination } from "../domain/pagination";


@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [VenueService, ConfirmationService,
    GlobalService]
})
export class VenuesComponent implements OnInit {

  venuetItems: MenuItem[];
  
  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;

  clonedSelectedVenue: Venue;
  indexSelected: number;

  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;
  displayDialog: boolean;
  loading: boolean;
  venue: Venue = new VenueClass();
  indexOfVenue: number;
  totalRecords: number = 0;
  searchFilter: string = "";
  paginationResult: Pagination<Venue>;

  btnSaveNew: boolean;
  btnDelete: boolean;
  btnSave: boolean;

  display: boolean = false;
  
      showDialog() {
          this.display = true;
      }

  constructor(private venueService: VenueService , private fb: FormBuilder,
     private confirmationService: ConfirmationService, private globalService: GlobalService) { }
  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
      // this.venueService.getVenues().then(venues => this.venueList = venues);
      // this.loading = false;
    //this.selectedVenue = this.venueList[0];

    this.userform = this.fb.group({
      'venueName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
      'description': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(100)])),
      'searchFilter' : new FormControl('', )
  });
  this.venuetItems = [
    {label:'Dashboard', routerLink:['/dashboard'] },
    {label:'Venues', routerLink:['/venues']}
  ]
}
paginate(event) {
  this.globalService.getSomethingWithPagination<Pagination<Venue>>("Venues", event.first, event.rows,
    this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
      this.paginationResult = paginationResult;
      this.venueList = this.paginationResult.results;
      this.totalRecords = this.paginationResult.totalRecords;
    });
}

searchVenue() {
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

  addVenues() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass;
    this.displayDialog = true;
    this.userform.markAsPristine();
  }

  saveVenues(value: string) {
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
      
    // this.submitted = true;
    // this.msgs = [];
    // this.msgs.push({severity:'info', summary:'Success', detail:'Form Submitted'});
    let tmpVenueList = [...this.venueList];
    if(this.isNewVenue){
      this.venueService.addVenues(this.selectedVenue).then(venues => {this.venue = venues;
      tmpVenueList.push(this.venue);
      this.venueList = tmpVenueList;
      });
    }else{
        this.venueService.saveVenues(this.selectedVenue);
        tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;
    }
    //this.venueService.saveVenues(this.selectedVenue);
    this.selectedVenue = null;
    this.isNewVenue = false;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
this.userform.markAsPristine();
  }

  saveNewVenues(value: string) {
    this.userform.enable();
    let tmpVenueList = [...this.venueList];
    if(this.isNewVenue){
        this.venueService.addVenues(this.selectedVenue).then(venues => {this.venue = venues;
        tmpVenueList.push(this.venue);
        this.venueList = tmpVenueList;
        });
    }else{
        this.venueService.saveVenues(this.selectedVenue);
        tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;
    }
    //this.venueService.saveVenues(this.selectedVenue);
    this.selectedVenue = new VenueClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Venue: Venue){
    this.userform.disable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedVenue = Venue;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
  }


  deleteVenues(){
    this.userform.enable();
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
      this.selectedVenue;
    let index = this.findSelectedVenueIndex();
    this.venueList = this.venueList.filter((val,i) => i!=index);
    this.venueService.deleteVenues(this.selectedVenue.venueId);
    this.selectedVenue = null;
    this.displayDialog = false;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  editVenues(Venue: Venue){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.displayDialog = true;
    this.isNewVenue = false;     
    this.selectedVenue = Venue;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
    this.userform.markAsPristine();
  }

  // onRowSelect(event) {
  //         this.isNewVenue = false;
  //         this.selectedVenue;
  //         this.cloneVenue = this.cloneRecord(this.selectedVenue);
  //         this.displayDialog = true;
  // } 

  cloneRecord(r: Venue): Venue {
      let venue = new VenueClass();
      for(let prop in r) {
          venue[prop] = r[prop];
      }
      return venue;
  }

  cancelVenues(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      icon: 'fa fa-question-circle',
      accept: () => {
          this.msgs = [{severity:'info', summary:'Confirmed', detail:'You have accepted'}];
      
    this.isNewVenue = false;
    let tmpVenueList = [...this.venueList];
    tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
    this.venueList = tmpVenueList;
    this.selectedVenue = this.cloneVenue;
    this.selectedVenue = null;
  },
  reject: () => {
      this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
  }
});
  }

  findSelectedVenueIndex(): number {
      return this.venueList.indexOf(this.selectedVenue);
  }
}