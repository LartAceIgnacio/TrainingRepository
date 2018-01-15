import { Component, OnInit } from '@angular/core';
import { VenueService } from '../services/venueservice';
import { Venue } from '../domain/venues/venue';
import { VenueClass } from '../domain/venues/venueclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [GlobalService, ConfirmationService, FormBuilder]
})
export class VenuesComponent implements OnInit {

  items: MenuItem[];
  home: MenuItem;
  venueList: Venue[];
  selectedVenue: Venue;
  isNewVenue: boolean;
  cloneVenue: Venue;
  display: boolean;
  userform: FormGroup;
  isDelete: boolean;
  isEdit: boolean;
  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;
  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';
  paginationResult: PaginationResult<Venue>;

  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {

    this.items = [
      { label: 'Venues' }
    ];
    this.home = { icon: 'fa fa-home', label: 'Home', routerLink: '/dashboard' };

    this.userform = this.fb.group({
      'venuename': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
      'description': new FormControl('', Validators.maxLength(100))
    });
  }

  showDialog() {
    this.isEdit = false;
    this.isDelete = false;
    this.userform.enable();
    this.userform.markAsPristine();
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
    this.display = true;
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Venue>>('Venues', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.venueList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
      });
  }

  searchVenue() {
    if (this.searchFilter.length !== 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    const paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }

  editVenue(venueToEdit: Venue) {
    this.isEdit = true;
    this.isDelete = false;
    this.userform.enable();
    this.isNewVenue = false;
    this.selectedVenue = this.cloneRecord(venueToEdit);
    this.cloneVenue = venueToEdit;
    this.display = true;
  }

  deleteVenue(venueToDelete: Venue) {
    this.isDelete = true;
    this.userform.disable();
    this.display = true;
    this.isNewVenue = false;
    this.selectedVenue = this.cloneRecord(venueToDelete);
    this.cloneVenue = venueToDelete;
    this.userform.markAsPristine();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        this.globalService.deleteSomething<Venue>('Venues', this.selectedVenue.venueId);
        const index = this.venueList.indexOf(this.cloneVenue);
        this.venueList = this.venueList.filter((val, i) => i !== index);
        this.selectedVenue = null;
        this.isNewVenue = false;
        this.display = false;

      }
    });
  }

  saveVenue() {
    const tmpVenueList = [...this.venueList];
    if (this.isNewVenue) {
      this.globalService.postSomething<Venue>('Venues', this.selectedVenue)
        .then(venue => {
          tmpVenueList.push(venue);
          this.venueList = tmpVenueList;
          this.selectedVenue = null;
          this.display = false;
        });
    } else {
      this.globalService.putSomething<Venue>('Venues', this.selectedVenue.venueId, this.selectedVenue)
        .then(venue => {
          tmpVenueList[this.venueList.indexOf(this.cloneVenue)] = this.selectedVenue;
          this.venueList = tmpVenueList;
          this.selectedVenue = null;
          this.display = false;
        });
    }
    this.isNewVenue = false;
  }

  newSaveVenue() {
    const tmpVenueList = [...this.venueList];
    this.globalService.postSomething<Venue>('Venues', this.selectedVenue)
      .then(venues => {
        tmpVenueList.push(venues);
        this.venueList = tmpVenueList;
        this.selectedVenue = new VenueClass;
        this.display = true;
      });
  }

  confirmCancel() {
    this.isNewVenue = false;
    const tmpVenueList = [...this.venueList];
    tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
    this.venueList = tmpVenueList;
    this.selectedVenue = Object.assign({}, this.cloneVenue);
    this.selectedVenue = new VenueClass();
    this.display = false;
    this.userform.markAsPristine();
  }

  cancelVenue() {
    if (this.userform.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        accept: () => {
          this.confirmCancel();
        }
      });
    } else {
      this.display = false;
    }
  }

  onRowSelect() {
    this.isNewVenue = false;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
  }

  cloneRecord(r: Venue): Venue {
    const venue = new VenueClass();
    // tslint:disable-next-line:forin
    for (const prop in r) {
      venue[prop] = r[prop];
    }
    return venue;
  }
}
