import { VenueService } from './../services/venueService';
import { Component, OnInit } from '@angular/core';
import { Venue } from '../domain/venue';
import { Venueclass } from '../domain/venueclass';
import { Message } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MenuItem, ConfirmationService } from 'primeng/primeng';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers:[VenueService, ConfirmationService]
})
export class VenuesComponent implements OnInit {

  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;
  displayDialog: boolean;
  loading: boolean;
  venue: Venue = new Venueclass();

  msgs: Message[] = [];
  venueForm: FormGroup;
  submitted: boolean;
  breadcrumb: MenuItem[];

  constructor(private venueService: VenueService, private fb: FormBuilder, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.loading = true;
    setTimeout(() => {
      this.venueService.getVenues().then(venues => this.venueList = venues);
      this.loading = false;
    }, 1000);
    
    this.venueForm = this.fb.group({
      'venuename': new FormControl('', Validators.required),
      'description': new FormControl('', Validators.required),
    });
    this.breadcrumb = [
      {label:'Dashboard', routerLink:['/dashboard']},
      {label:'Venues', routerLink:['/venues']},
  ];

  }
  addVenue() {
    this.isNewVenue = true;
     this.selectedVenue = new Venueclass;
    this.displayDialog = true;
  }
  editVenue(Venue : Venue){
    this.isNewVenue = false;
    this.selectedVenue = Venue;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
    this.displayDialog = true;
  }
  saveVenue() {
    let tmpVenueList = [...this.venueList];
    if(this.isNewVenue){
        this.venueService.addVenues(this.selectedVenue);
        tmpVenueList.push(this.selectedVenue);
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Success', detail:'Added Venue '});
    }else{
        this.venueService.saveVenues(this.selectedVenue);
        tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Venue Details'});
    }
    this.venueForm.markAsPristine();
    this.venueList = tmpVenueList;
    this.selectedVenue = null;
    this.displayDialog = false;
 
  }
  saveAndNewVenue(){
    
    this.venueForm.markAsPristine();

    let tmpVenueList = [...this.venueList];
    tmpVenueList.push(this.selectedVenue);

    // this.submitted = true;
    this.msgs = [];
    this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Venue Details'});
    if(this.isNewVenue){
      this.venueService.addVenues(this.selectedVenue);
      this.venueList=tmpVenueList;
      this.isNewVenue = true;
      this.selectedVenue = new Venueclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Venue'});
    }
    
  }

  deleteVenue(Venues : Venue){
    this.selectedVenue = Venues;
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        let index = this.findSelectedVenueIndex();
        this.venueList = this.venueList.filter((val,i) => i!=index);
        this.venueService.deleteVenues(this.selectedVenue.venueId);
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'error', summary:'Deleted', detail:'Deleted Venue'});
        this.selectedVenue = null;
        this.displayDialog = false;    
      }
  });
  }
  // onRowSelect(event) {
  //         this.isNewVenue = false;
  //         this.selectedVenue;
  //         this.cloneVenue = this.cloneRecord(this.selectedVenue);
  //         // this.displayDialog = true;
  // } 

  cloneRecord(r: Venue): Venue {
      let venue = new Venueclass();
      for(let prop in r) {
          venue[prop] = r[prop];
      }
      return venue;
  }

  cancelVenue(){
    if(this.venueForm.dirty){  
      this.confirmationService.confirm({
        message: 'Do you want to Discard this changes?',
        accept: () => {
          this.isNewVenue = false;
          let tmpVenueList = [...this.venueList];
          tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
          this.venueList = tmpVenueList;
          this.selectedVenue = this.cloneVenue;
          this.selectedVenue = null;
        }
      });
    }
  
  }

  findSelectedVenueIndex(): number {
      return this.venueList.indexOf(this.selectedVenue);
  }

}
