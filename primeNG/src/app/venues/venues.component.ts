import { Component, OnInit } from '@angular/core';
import { Venue } from '../domain/venue';
import { VenueClass } from '../domain/venueclass';
import { GlobalService } from '../services/globalservice';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [GlobalService]
})

export class VenuesComponent implements OnInit {
  venueList: Venue[];
  cloneVenue: Venue;
  selectedVenue: Venue;
  isNewVenue: boolean;
  indexOfVenue: number;
  userform: FormGroup;

  constructor(private globalService: GlobalService, private fb: FormBuilder) { }

  ngOnInit() {
    this.globalService.getSomething<Venue>("Venues").then(venues => this.venueList = venues);

    this.userform = this.fb.group({
      'venueName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
      'description': new FormControl('', Validators.maxLength(100))
    });
  }

  addVenue() {
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
  }

  onRowSelect(event) {
    this.isNewVenue = false;
    this.indexOfVenue = this.venueList.indexOf(this.selectedVenue);
    this.cloneVenue = Object.assign({}, this.selectedVenue);
    this.selectedVenue = Object.assign({}, this.selectedVenue);
  }

  btnSave() {
    let tmpVenueList = [...this.venueList];
    if (this.isNewVenue) {
      this.globalService.addSomething<Venue>("Venues", this.selectedVenue).then(venues => {
        tmpVenueList.push(venues);
        this.venueList = tmpVenueList;
        this.selectedVenue = null;
      });
    }
    else {
      this.globalService.updateSomething<Venue>("Venues", this.selectedVenue.venueId, this.selectedVenue).then(venues => {
        tmpVenueList[this.indexOfVenue] = this.selectedVenue;
        this.venueList = tmpVenueList;
        this.selectedVenue = null;
      });
    }
    this.isNewVenue = false;
  }

  btnCancel() {
    if (this.isNewVenue)
      this.selectedVenue = null;
    else {
      let tmpVenueList = [...this.venueList];
      tmpVenueList[this.indexOfVenue] = this.cloneVenue;
      this.venueList = tmpVenueList;
      this.selectedVenue = Object.assign({}, this.cloneVenue);
    }
  }

  btnDelete() {
    let tmpVenueList = [...this.venueList];
    tmpVenueList.splice(this.indexOfVenue, 1);
    this.globalService.deleteSomething<Venue>("Venues", this.selectedVenue.venueId).then(venues => {
      tmpVenueList.splice(this.indexOfVenue, 1);
      this.venueList = tmpVenueList;
      this.selectedVenue = null;
      this.isNewVenue = false;
    });
  }

  get diagnostic() { return JSON.stringify(this.userform.value); }
}
