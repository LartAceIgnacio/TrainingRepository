import { Component, OnInit } from '@angular/core';
import { Venue } from '../domain/venue';
import { VenueClass } from '../domain/venueclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { PaginationResult } from "../domain/paginationresult";

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [GlobalService, ConfirmationService]
})

export class VenuesComponent implements OnInit {
  venueList: Venue[];
  cloneVenue: Venue;
  selectedVenue: Venue;
  isNewVenue: boolean;
  isDeleteVenue: boolean = false;
  indexOfVenue: number;
  userform: FormGroup;
  totalRecords: number = 0;
  searchFilter: string = "";
  paginationResult: PaginationResult<Venue>;

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.userform = this.fb.group({
      'venueName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
      'description': new FormControl('', Validators.maxLength(100))
    });
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Venue>>("Venues", event.first, event.rows,
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

  addVenue() {
    this.userform.enable();
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
  }

  onRowSelect(venue) {
    this.userform.enable();
    this.selectedVenue = venue;
    this.isNewVenue = false;
    this.indexOfVenue = this.venueList.indexOf(this.selectedVenue);
    this.cloneVenue = Object.assign({}, this.selectedVenue);
    this.selectedVenue = Object.assign({}, this.selectedVenue);
  }

  btnSave(isSaveAndNew: boolean) {
    this.userform.markAsPristine();
    let tmpVenueList = [...this.venueList];
    if (this.isNewVenue) {
      this.globalService.addSomething<Venue>("Venues", this.selectedVenue).then(venues => {
        tmpVenueList.push(venues);
        this.venueList = tmpVenueList;
        this.selectedVenue = isSaveAndNew ? new VenueClass() : null;
        this.isNewVenue = isSaveAndNew ? true : false;
        if (!isSaveAndNew) {
          this.setCurrentPage(1);
        }
      });
    }
    else {
      this.globalService.updateSomething<Venue>("Venues", this.selectedVenue.venueId, this.selectedVenue).then(venues => {
        tmpVenueList[this.indexOfVenue] = this.selectedVenue;
        this.venueList = tmpVenueList;
        this.selectedVenue = null;
        this.isNewVenue = false;
      });
    }
  }

  btnCancel() {
    if (this.isNewVenue || this.isDeleteVenue || !this.userform.dirty) {
      this.selectedVenue = null;
      this.isDeleteVenue = false;
      this.isNewVenue = false
      this.userform.markAsPristine();
    }
    else {
      this.confirmationService.confirm({
        message: 'Are you sure you want to discard the changes?',
        header: 'Cancel Confirmation',
        icon: 'fa fa-ban',
        accept: () => {
          let tmpVenueList = [...this.venueList];
          tmpVenueList[this.indexOfVenue] = this.cloneVenue;
          this.venueList = tmpVenueList;
          this.selectedVenue = Object.assign({}, this.cloneVenue);
          this.userform.markAsPristine();
        }
      });
    }
  }

  btnDelete(venue) {
    this.userform.disable();
    this.selectedVenue = venue;
    this.indexOfVenue = this.venueList.indexOf(venue);
    this.isDeleteVenue = true;
  }

  btnOk() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let tmpVenueList = [...this.venueList];
        this.globalService.deleteSomething<Venue>("Venues", this.selectedVenue.venueId).then(venues => {
          tmpVenueList.splice(this.indexOfVenue, 1);
          this.venueList = tmpVenueList;
          this.selectedVenue = null;
          this.isNewVenue = false;
          this.isDeleteVenue = false;
          this.setCurrentPage(1);
        });
      }
    });
  }
}

class PaginationResultClass implements PaginationResult<Venue>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}
