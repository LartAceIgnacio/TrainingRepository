import { Component, OnInit } from '@angular/core';
import {Venue} from '../domain/Venue';
import {VenueService} from '../services/VenueService';
import {HttpClient} from '@angular/common/http';
import {VenueClass} from '../domain/VenueClass';

import {BreadcrumbModule,MenuItem} from 'primeng/primeng';

import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import { AuthService } from "../services/auth.service";

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [VenueService]
})
export class VenuesComponent implements OnInit {

  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;

  venueform: FormGroup;

  brVenue: MenuItem[];
  home: MenuItem;

  constructor(
    private venueService:VenueService,
    private http:HttpClient,
    private fb: FormBuilder,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.venueService.getVenues().then(venues => this.venueList = venues);

    this.venueform=new FormGroup({
      'venuename': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
      'description': new FormControl('', Validators.compose([Validators.maxLength(100)]))
   });

   this.brVenue=[
    {label: 'Venues', url: '/venues'}
  ]
  this.home = {icon: 'fa fa-home', routerLink: '/dashboard'};
  }

  addVenue(){
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
  }

  saveVenue(){
    let tmpVenueList = [...this.venueList];
    if(this.isNewVenue){
      this.venueService.postVenues(this.selectedVenue);
      tmpVenueList.push(this.selectedVenue);
    }
    else{
      this.venueService.putVenues(this.selectedVenue);
      tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;
    }

    this.venueList=tmpVenueList;
    this.selectedVenue=null;
    this.isNewVenue = false;
  }

  cancelVenue(){
    let index = this.findSelectedVenueIndex();
    if (this.isNewVenue)
      this.selectedVenue = null;
    else {
      let tmpVenueList = [...this.venueList];
      tmpVenueList[index] = this.cloneVenue;
      this.venueList = tmpVenueList;
      this.selectedVenue = Object.assign({}, this.cloneVenue);
    }
  }

  onRowSelect(){
    this.isNewVenue = false;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
  }

  cloneRecord(r: Venue): Venue{
    let venue = new VenueClass();
    for(let prop in r){
      venue[prop]=r[prop];
    }
    console.log(venue);
    return venue;
  }

  findSelectedVenueIndex():number{
    return this.venueList.indexOf(this.selectedVenue);
  }

  deleteVenue(){
    let index = this.findSelectedVenueIndex();
    this.venueList = this.venueList.filter((val,i) => i!=index);
    this.venueService.deleteVenues(this.selectedVenue.venueId);
    this.selectedVenue = null;
  }

}
