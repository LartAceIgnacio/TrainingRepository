import { Component, OnInit } from '@angular/core';
import { Venue } from '../domain/venue';
import { VenueService } from '../services/venueservice';
import { VenueClass } from '../domain/venueclass';

import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [VenueService, ConfirmationService]
})
export class VenuesComponent implements OnInit {

  venue: Venue = new VenueClass();
  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;

  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  description: string;

  display: boolean = false;

  items: MenuItem[];

  home: MenuItem;


  constructor(private venueservice: VenueService, private fb: FormBuilder, private conf: ConfirmationService) { }

  newVenue() {
    this.selectedVenue = new VenueClass();
    this.display = true;
    this.isNewVenue = true;
  }

  editVenue(venue: Venue) {
    this.display = true;
    this.selectedVenue = venue;
    this.isNewVenue = false;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
  }

  saveAndNewVenue() {
    let tmpEmployeeList = [...this.venueList];
    this.venueservice.createVenues(this.selectedVenue).then(emp => {
      tmpEmployeeList.push(emp);
      this.venueList = tmpEmployeeList;
      this.selectedVenue = new VenueClass();
    });
    this.userform.markAsPristine();
    this.msgs = [];
    this.msgs.push({ severity: 'success', summary: 'Venue Added!' });
  }

  deleteVen(venue: Venue) {
    this.selectedVenue = venue;
    this.conf.confirm({
      message: 'Are you sure that you want to delete this data?',
      accept: () => {
        if (this.selectedVenue.venueId == null)
          this.selectedVenue = new VenueClass();
        else {
          this.venueservice.deleteVenues(this.selectedVenue.venueId)
          let index = this.venueList.indexOf(this.selectedVenue);
          this.venueList = this.venueList.filter((val, i) => i != index);
          this.venue = null;
        }
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Venue Deleted!' });
        this.selectedVenue = new VenueClass();
      }
    });

  }


  ngOnInit() {
    this.venueservice.getVenues()
      .then(venues => this.venueList = venues);

    this.userform = this.fb.group({
      'venuename': new FormControl('', Validators.required),
      'venuedescription': new FormControl('', Validators.required)
    });

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Employee', routerLink: ['/employees'] },
      { label: 'Contact', routerLink: ['/contacts'] },
      { label: 'Venue', routerLink: ['/venues'] },
      { label: 'Appointment', routerLink: ['/appointments'] }
    ];

    this.home = { icon: 'fa fa-home' };
  }

  addVenue() {
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
  }

  saveVenue() {
    let tmpVenueList = [...this.venueList];
    if (this.isNewVenue) {
      this.venueservice.createVenues(this.selectedVenue).then(emp => {
        tmpVenueList.push(emp);
        this.venueList = tmpVenueList;
        this.selectedVenue = null;

        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Venue Added!' });
      });
    }

    else {
      this.venueservice.updateVenues(this.selectedVenue, this.selectedVenue.venueId)
      tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;

      this.msgs = [];
      this.msgs.push({ severity: 'success', summary: 'Venue Saved!' });
    }
    this.userform.markAsPristine();
    this.isNewVenue = false;
    this.display = false;
  }

  onRowSelect() {
    this.isNewVenue = false;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
  }

  cloneRecord(r: Venue): Venue {
    let venue = new VenueClass();
    for (let prop in r) {
      venue[prop] = r[prop];
    }
    return venue;
  }

  cancelVenue() {
    this.isNewVenue = false;
    let tmpVenueList = [...this.venueList];
    tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
    this.venueList = tmpVenueList;
    this.selectedVenue = this.cloneVenue;
    this.selectedVenue = new VenueClass();
    this.display = false;
    this.userform.markAsPristine();
  }
  cancelVen() {
    if (this.isNewVenue == true) {
      this.display = false;
    }
    else {
      if (this.userform.dirty) {
        this.conf.confirm({
          message: 'Do you want to discard changes?',
          accept: () => {
            this.cancelVenue()
          }
        });
      }
      else {
        this.display = false;
      }
    }
    this.userform.markAsPristine();
  }

  deleteVenue() {
    if (this.selectedVenue.venueId == null)
      this.selectedVenue = new VenueClass();
    else {
      this.venueservice.deleteVenues(this.selectedVenue.venueId)
      let index = this.venueList.indexOf(this.selectedVenue);
      this.venueList = this.venueList.filter((val, i) => i != index);
      this.venue = null;
    }
    this.selectedVenue = new VenueClass();
  }
}
