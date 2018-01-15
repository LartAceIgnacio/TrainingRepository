import { Component, OnInit } from '@angular/core';
import { GlobalService } from "../services/globalService";
import { Venue } from "../domain/venue";
import { VenueClass } from "../domain/venueClass";
import { HttpClient } from '@angular/common/http';
import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { PaginationResult } from "../domain/paginationresult";
import { ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';
import { API_URL} from '../services/constants';

@Component({
  selector: 'app-venue',
  templateUrl: './venue.component.html',
  styleUrls: ['./venue.component.css'],
  providers: [GlobalService]
})

export class VenueComponent implements OnInit { 
  //contact variable 
  venueList: Venue[];
  selectedVenue: Venue;
  cloneVenue: Venue;
  isNewVenue: boolean;
  //url 
  service: string = 'venues';
  serviceUrl: string = `${API_URL}/${this.service}/`;
  //form
  venueForm: FormGroup;
  dialogTitle: String;
  //displayable
  displayInputBox: boolean = false;
  onEdit: boolean = false;
  onDelete: boolean = false;
  onAdd: boolean = false;
  //confirmation
  confirmEditCancel: boolean = false;
  confirmDeleteRecord: boolean = false;
  //pagination  
  totalRecords: number = 0;
  searchFilter: string = "";
  paginationResult: PaginationResult<Venue>;

  constructor(private globalService: GlobalService, private formbuilder: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {    
    const nameMaxLength: number = 50;
    const descMaxLength: number = 100;

    //this.globalService.retrieve(this.path).then(venues => this.venueList = venues);    
    this.venueForm = this.formbuilder.group({
      'venueName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(nameMaxLength)])),
      'description': new FormControl('', Validators.maxLength(descMaxLength)),
      });
  }

  paginate(event) {
    this.globalService.retrieveWithPagination<PaginationResult<Venue>>(this.serviceUrl, event.first, event.rows,
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

  //--------------------CREATE-----------------------
  addVenue() {    
    this.isNewVenue = true;
    this.selectedVenue = new VenueClass();
    //form
    this.displayForm("New Venue", true);
    //displayable      
    this.onEdit= false;
    this.onDelete = false;
    this.onAdd = true;
    //confirmation
    this.confirmEditCancel = false;
    this.confirmDeleteRecord = false;
  }
  //--------------------EDIT-----------------------
  editVenue(venue: Venue){
    this.isNewVenue = false;
    this.selectedVenue = venue;
    this.cloneVenue = this.cloneRecord(this.selectedVenue);
    //form
    this.displayForm("Edit Venue", true);
    //displayable      
    this.onEdit= true;
    this.onDelete = false;
    this.onAdd = false;
  }
  //--------------------DELETE-----------------------
  deleteVenue(venue: Venue, rowSelect: boolean){
    //displayable      
    this.onEdit= false;
    this.onDelete = true;
    this.onAdd = false;

    if(rowSelect){
      this.selectedVenue = venue;
      //form
      this.displayForm("Delete Venue", false);
    }
    else{
      //confirmation
      this.confirmEditCancel = false;
      this.confirmDeleteRecord = true;
    }
  }  
 //--------------------FORM-----------------------
  saveVenue(withNew:boolean){
    let tempVenueList = [...this.venueList];
    if(this.isNewVenue){
      this.globalService.add(this.serviceUrl, this.selectedVenue).then(venue => {
        tempVenueList.push(venue);
        this.venueList = tempVenueList;
        this.selectedVenue = null;
        this.displayInputBox = false;
        if(withNew)
          this.addVenue();
          this.setCurrentPage(1);
        })
    }
    else{      
      this.globalService.update(this.serviceUrl, this.selectedVenue, this.selectedVenue.venueId).then(venue => {
        tempVenueList[this.venueList.indexOf(this.selectedVenue)];
        this.venueList = tempVenueList;
        this.selectedVenue = null;  
        this.displayInputBox = false;
        })
    }
    this.isNewVenue = false;
  }

  cancelVenue() {
    if(this.onAdd){
      this.toCancel(true);
    }
    else if(this.onEdit){
      if(this.venueForm.dirty){  
        this.confirmEditCancel = true;
      }
      if(this.venueForm.pristine){
        this.displayInputBox = false;
      }
    }
    else if(this.onDelete){
      this.confirmDeleteRecord = false;
      this.displayInputBox = false;
    }    
    else{
      this.displayInputBox = false;
    }
  }
  //--------------------CONFIRMATION-----------------------
  toCancel(discard:boolean){  
    if(discard) {
      if(this.onEdit){
        this.isNewVenue = false;
        let tempVenueList = [...this.venueList];
        tempVenueList[this.venueList.indexOf(this.selectedVenue)] = this.cloneVenue;
        this.venueList = tempVenueList;
        this.selectedVenue = this.cloneVenue;
        this.cloneVenue = this.cloneRecord(this.selectedVenue);
        this.venueForm.markAsPristine();
      }
      else{
        //displayable
        this.displayInputBox = false;
      }      
    }
    this.confirmEditCancel = false;
  }

  toDelete(toDelete: boolean){
    if(toDelete){    
      this.displayInputBox = true;  
      this.globalService.delete(this.serviceUrl, this.selectedVenue, this.selectedVenue.venueId).then(venue => {
        let tempVenueList = [...this.venueList];
        tempVenueList.splice(this.venueList.indexOf(this.selectedVenue), 1);
        this.venueList = tempVenueList;
        this.selectedVenue = null;        
        this.setCurrentPage(1);
      })
    }   
    this.displayInputBox = false; 
    this.confirmDeleteRecord = false;
  } 
  //--------------------MISC-----------------------
  cloneRecord(r: Venue): Venue {
    let venue = new VenueClass();
    for (let prop in r) {
      venue[prop] = r[prop];
    }
    return venue;
  }

  displayForm(title: string, editable: boolean){
    this.dialogTitle = title;
    this.venueForm.markAsPristine();
    if(editable){
      this.venueForm.enable();
    }
    else{
      this.venueForm.disable();
    }
    this.displayInputBox = true;
  } 
}

class PaginationResultClass implements PaginationResult<Venue>{
  constructor (public results, public pageNo, public recordPage, public totalRecords) 
  {
      
  }
}