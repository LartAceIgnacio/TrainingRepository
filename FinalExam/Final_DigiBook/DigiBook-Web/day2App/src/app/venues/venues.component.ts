import { Component, OnInit } from '@angular/core';
import { VenueService } from '../services/venueservice';
import { Venue } from '../domain/venue';
import { VenueClass } from '../domain/venueclass';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { ConfirmationService } from 'primeng/primeng';

@Component({
  selector: 'app-venues',
  templateUrl: './venues.component.html',
  styleUrls: ['./venues.component.css'],
  providers: [VenueService, ConfirmationService]
})
export class VenuesComponent implements OnInit {

  msgs: Message[] = [];
  userform: FormGroup;
  submitted: boolean;
  description: string;
  
  venueList: Venue[];
  selectedVenue: Venue;
  isNewVenue: Boolean;
  venue: Venue;
  cloneVenue: Venue;

  constructor(private venueService: VenueService, private fb: FormBuilder, private confirmationService: ConfirmationService) { }
  
    ngOnInit() {
      this.venueService.getVenue()
      .then(venue => this.venueList = venue); 

      this.userform = this.fb.group({
        'venueName': new FormControl('', Validators.required),
        'description': new FormControl('', Validators.required),
      });
    }

    addVenue(){
      this.isNewVenue = true;
      this.selectedVenue = new VenueClass();
    }
  
    saveVenue(){
      this.submitted = true;
      this.msgs = [];
      this.msgs.push({severity: 'info', summary: 'Success', detail:'New Added Venue'});
      let tmpVenueList = [...this.venueList];
      if(this.isNewVenue)
      {
        this.venueService.postVenue(this.selectedVenue).then(venue => 
          {this.venue = venue; 
          tmpVenueList.push(this.venue);
          this.venueList = tmpVenueList;
        });
      }
      else{
        this.venueService.putVenue(this.selectedVenue.venueId, this.selectedVenue).then(venue =>
          {this.venue = venue; 
          });
        tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.selectedVenue;
        this.venueList = tmpVenueList;
      }
      this.selectedVenue   = null;
      this.isNewVenue = false;  
    }
  
    cancelVenue(){
      if (this.isNewVenue)
        this.selectedVenue = null;
      else {
        this.isNewVenue = false;
        let tmpVenueList = [...this.venueList];
        tmpVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
        this.venueList = tmpVenueList ;
        this.selectedVenue = this.cloneVenue;
        this.selectedVenue = null;
      }
    }
  
    onRowSelect(){
      this.isNewVenue = false;
      this.cloneVenue = this.cloneRecord(this.selectedVenue);
    }
  
    cloneRecord(r: Venue): Venue{
      let venue = new VenueClass();
      for(let prop in r){
        venue[prop] = r[prop];
      }
      return venue;
    }

    deleteVenue(){
      if(this.selectedVenue != null && !this.isNewVenue){
        this.confirmationService.confirm({
          message: 'Do you want to delete this record?',
          header: 'Delete Confirmation',
          icon: 'fa fa-trash',
          accept: () => {
            let tmpVenueList = [...this.venueList];
            this.venueService.deleteVenue(this.selectedVenue.venueId);
            tmpVenueList.splice(this.venueList.indexOf(this.selectedVenue), 1);
  
            this.venueList = tmpVenueList;
            this.selectedVenue   = null;
            this.isNewVenue = false;  
              this.msgs = [{severity:'info', summary:'Confirmed', detail:'Record deleted'}];
          },
          reject: () => {
              this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
          }
        });
      } 
    } 
}