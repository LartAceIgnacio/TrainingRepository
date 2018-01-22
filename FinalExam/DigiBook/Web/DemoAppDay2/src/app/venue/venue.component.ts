import { Component, OnInit } from '@angular/core';
// imports
import { VenueService } from '../services/venue.service';
import { Venue } from '../domain/venue/venue';
import { VenueClass } from '../domain/venue/venue.class';

import { HttpClient } from '@angular/common/http';
//validation
import { Message, SelectItem } from 'primeng/components/common/api';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-venue',
  templateUrl: './venue.component.html',
  styleUrls: ['./venue.component.css'],
  providers: [VenueService]

})
export class VenueComponent implements OnInit {


  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;

  indexSelected: any; // for editing
  clonedSelectedVenue: Venue; // for editing

  //validations
  msgs: Message[] = [];

  userform: FormGroup;

  submitted: boolean;

  constructor(private venueService: VenueService, private http: HttpClient, private fb: FormBuilder) { }

  ngOnInit() {
    //validations
    this.userform = this.fb.group({
      'venueName': new FormControl('', Validators.required),
      'description': new FormControl('', Validators.required),
      //'gender': new FormControl('', Validators.required)
    });

    this.venueService.getVenues()
      .then(data => {
        console.log(data);
        this.venueList = data;
      });
  }
  // validations
  get diagnostic() { return JSON.stringify(this.userform.value); }

  formSubmitted(message): void {
    this.submitted = true;
    this.msgs = [];
    this.msgs.push({ severity: 'info', summary: 'Success', detail: message });
  }

  addVenue() {
    this.isNewVenue = true;
    //this.selectedVenue = new VenueClass();
    this.clonedSelectedVenue = new VenueClass();
  }

  saveVenue() {
    let tmpVenueList = [...this.venueList];
    if (this.isNewVenue) {

      this.venueService.createVenue(this.clonedSelectedVenue)
        .then(data => {
          tmpVenueList.push(data);
          this.clearContent(tmpVenueList);
          // alert('Success!');
          this.formSubmitted("Venue Details Submitted!");
        });

    } else {
      this.venueService.updateVenue(this.clonedSelectedVenue)
        .then(data => {
          tmpVenueList[this.indexSelected] = this.clonedSelectedVenue;
          this.clearContent(tmpVenueList);
          // alert('Success!');
          this.formSubmitted("Updated Venue Details!");
        });
    }
  }

  clearContent(tmpVenueList) {
    this.venueList = tmpVenueList;
    //this.selectedVenue = null;
    this.clonedSelectedVenue = null;
    this.isNewVenue = false;
  }

  deleteVenue() {
    if (this.indexSelected > -1) {
      let tmpVenueList = [...this.venueList];

      this.venueService
        .deleteVenue(this.clonedSelectedVenue)
        .then(res => {
          console.log(res);
          if (res === 204) {
            tmpVenueList.splice(this.indexSelected, 1);
            this.venueList = tmpVenueList;
            this.clonedSelectedVenue = null;
            this.indexSelected = -1;
            this.formSubmitted("Deleted Venue!");

          } else {
            alert("Failed to Delete!");
          }
        });

    }
  }
  cancelProcess() {
    this.isNewVenue = false;
    this.clonedSelectedVenue = JSON.parse(JSON.stringify(this.selectedVenue));

  }

  onRowSelect() {
    console.log(this.selectedVenue);
    this.indexSelected = this.venueList.indexOf(this.selectedVenue); // value will be the index used for editing
    this.clonedSelectedVenue = JSON.parse(JSON.stringify(this.selectedVenue)); // cloned value of selected
    this.isNewVenue = false;
  }


}
