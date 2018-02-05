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
  providers: [GlobalService, ConfirmationService]
})
export class VenuesComponent implements OnInit {

  items: MenuItem[];

  home: MenuItem;

  displayDialog: boolean;

  venue: Venue = new VenueClass();

  selectedVenue: Venue;

  newVenue: boolean;

  venues: Venue[];

  isDelete: boolean;

  isEdit: boolean;

  isNewVenue: boolean;

  loading: boolean;

  userform: FormGroup;

  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';

  paginationResult: PaginationResult<Venue>;

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService, private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      // this.venueService.getVenues().then(venues => this.venues = venues);
      this.loading = false;
    }, 1000);

    this.items = [
      { label: 'Home', routerLink: ['/dashboard'] },
      { label: 'Venues' }
    ];

    this.home = { icon: 'fa fa-home' };

    this.userform = this.fb.group({
      'venuename': new FormControl('', Validators.required),
      'description': new FormControl('', Validators.maxLength(100))
    });
  }

  showDialogToAdd() {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = true;
    this.isDelete = false;
    this.newVenue = true;
    this.selectedVenue = null;
    this.venue = new VenueClass();
    this.displayDialog = true;
  }

  onRowSelect(clickVenue: Venue) {
    this.newVenue = false;
    this.venue = this.cloneContact(clickVenue);
    this.selectedVenue = clickVenue;
    this.displayDialog = true;
  }

  cloneContact(v: Venue): Venue {
    const venue = new VenueClass();
    // tslint:disable-next-line:forin
    for (const prop in v) {
      venue[prop] = v[prop];
    }
    return venue;
  }
  save(number) {
    this.isNewVenue = number;

    const venues = [...this.venues];
    if (this.newVenue) {
      this.globalService.addSomething('Venues', this.venue).then(
        data => {
          this.venue = data;
          venues.push(this.venue);
          this.venues = venues;
          this.dataTable.reset();
        }
      );

    } else {
      this.globalService.updateSomething('Venues', this.venue.venueId, this.venue).then(
        data => {
          this.venue = data;
          venues[this.findSelectedVenueIndex()] = this.venue;
          this.venues = venues;
        }
      );
    }

    if (this.isNewVenue) {
      this.userform.markAsPristine();
      // this.venues = venues;
      this.newVenue = true;
      this.selectedVenue = null;
      this.venue = new VenueClass();

    }else {
      this.userform.markAsPristine();
      // this.venues = venues;
      this.venue = null;
      this.displayDialog = false;
      // this.setCurrentPage(1);
    }

  }

  findSelectedVenueIndex(): number {
    return this.venues.indexOf(this.selectedVenue);
  }

  delete(clickVenue: Venue) {
    this.userform.markAsPristine();
    this.userform.disable();
    this.isDelete = true;
    this.onRowSelect(clickVenue);
  }

  deleteVenue() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        this.globalService.deleteSomething('Venues', this.venue.venueId);
        const index = this.findSelectedVenueIndex();
        this.venues = this.venues.filter((val, i) => i !== index);
        this.venue = null;
        this.displayDialog = false;
        this.dataTable.reset();
      }
    });
  }
  edit(clickVenue: Venue) {
    this.userform.enable();
    this.isEdit = false;
    this.isDelete = false;
    this.onRowSelect(clickVenue);
  }

  cancel() {
    if (this.isDelete) {
      this.displayDialog = false;
    }else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to cancel?',
        accept: () => {
          this.venue = Object.assign({}, this.selectedVenue);
        }
      });
    }
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Venue>>('Venues', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.venues = this.paginationResult.results;
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
    // tslint:disable-next-line:prefer-const
    let paging = {
      first: ((n - 1) * this.dataTable.rows),
      rows: this.dataTable.rows
    };
    this.dataTable.paginate();
  }
}

class PaginationResultClass implements PaginationResult<Venue> {
  constructor (public results, public pageNo, public recordPage, public totalRecords) {

  }
}
