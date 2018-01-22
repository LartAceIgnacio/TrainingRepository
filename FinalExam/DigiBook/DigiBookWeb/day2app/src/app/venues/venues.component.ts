import { Component, OnInit } from '@angular/core';
import { VenueService } from '../services/venueservice';
import { Venue } from '../domain/venues/venue';
import { VenueClass } from '../domain/venues/venueclass';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [VenueService]
})
export class VenuesComponent implements OnInit {
  cloneVenue: Venue;

  isNewVenue: boolean;

  venueList: Venue[];
  selectedVenue: Venue;

  constructor(private venueService: VenueService) { }

  ngOnInit() {
    this.venueService.getVenues()
    .then(venues=>this.venueList = venues);
  }

  addVenue(){
    this.selectedVenue = new VenueClass();
    this.isNewVenue = true;
  }

  saveVenue(){
    let tmpVenueList = [...this.venueList];
    if(this.isNewVenue){
      this.venueService.postVenues(this.selectedVenue);
      tmpVenueList.push(this.selectedVenue);
      this.venueList = tmpVenueList;
      this.selectedVenue = null;
    }
    else{
      this.venueService.putVenues(this.selectedVenue.venueId, this.selectedVenue)
      .then(contact => { tmpVenueList[this.venueList.indexOf(this.selectedVenue)] 
      = this.selectedVenue;
        this.venueList = tmpVenueList;
        this.selectedVenue = null;
      });
    }

    this.isNewVenue = false;
  }

  deleteVenue(){
    this.venueService.deleteVenues(this.selectedVenue.venueId);
    let index = this.venueList.indexOf(this.selectedVenue);
    this.venueList=this.venueList.filter((val,i)=>i!=index);
    this.selectedVenue=null;
  }

  cancelVenue() {
    this.isNewVenue = false;
    let tmpVenueList = [...this.venueList];
    tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;

    this.venueList = tmpVenueList;
    this.selectedVenue = this.cloneVenue;
    this.selectedVenue = new VenueClass();
    // let index = this.venueList.indexOf(this.selectedVenue);
    // this.selectedVenue
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
}
