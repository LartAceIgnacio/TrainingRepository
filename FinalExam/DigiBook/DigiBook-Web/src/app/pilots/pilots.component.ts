import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SelectItem, ConfirmationService, LazyLoadEvent } from 'primeng/primeng';
import { Message } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MenuItem } from 'primeng/components/common/menuitem';
import { Pagination } from '../domain/pagination';
import { PaginationClass } from '../domain/paginationClass';

import { DataTable } from 'primeng/components/datatable/datatable';
import { GlobalService } from '../services/global.service';
import { Pilot } from '../domain/pilot';
import { Pilotclass } from '../domain/pilotclass';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-pilots',
  templateUrl: './pilots.component.html',
  styleUrls: ['./pilots.component.css'],
  providers:[GlobalService, ConfirmationService,DatePipe]
})
export class PilotsComponent implements OnInit {
  [x: string]: any;

  indexSelected: number;
  isNewPilot: boolean;
  displayDialog: boolean;
  loading: boolean;
  delete:boolean ;
  submitted: boolean;

  serviceName: string = "Pilot";
  selectedDateOfBirth: Date;
  selectedDateActivated: Date;

  countryList: SelectItem[];
  clonedSelectedPilot: Pilot;
  pilotList: Pilot[];
  selectedPilot: Pilot;
  clonePilot: Pilot;

  Pilot: Pilot = new Pilotclass();

  pagination: Pagination<Pilot>;
  totalRecords: number = 0;
  searchFilter: string = "";

  msgs: Message[] = [];
  pilotForm: FormGroup;
  breadcrumb: MenuItem[];

  constructor(private globalService: GlobalService, private fb: FormBuilder, private confirmationService: ConfirmationService,
              private datePipe: DatePipe) { }

  @ViewChild('dt') public dataTable: DataTable;
  
  ngOnInit() {
    this.pilotForm = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'middleName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'dateOfBirth': new FormControl('', Validators.required),
      'yearsOfExperience': new FormControl('', Validators.required),
      'dateActivated': new FormControl('', Validators.required)
    });

    this.breadcrumb = [
      {label:'Dashboard', routerLink:['/dashboard']},
      {label:'Pilots', routerLink:['/pilots']},
  ];
  }
  paginate(event) {
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
    this.globalService.getPagination<Pagination<Pilot>>("Pilots", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
        this.pagination = pagination;
        this.pilotList = this.pagination.results;
        this.totalRecords = this.pagination.totalRecords;
        for(let i=0;i < this.pilotList.length;i++){
        this.pilotList[i].dateOfBirth = new Date(this.pilotList[i].dateOfBirth).toLocaleDateString();
        this.pilotList[i].dateActivated = new Date(this.pilotList[i].dateActivated).toLocaleDateString();
        this.pilotList[i].dateCreated = new Date(this.pilotList[i].dateCreated).toLocaleDateString();
        this.pilotList[i].dateModified = new Date(this.pilotList[i].dateModified).toLocaleDateString();
        }
        // for (var i = 0; i < this.pilotList.length; i++) {
        //   this.pilotList[i].dateActivated = this.pilotList[i].dateActivated == null ? null :
        //     new Date(this.pilotList[i].dateActivated).toLocaleDateString();
        // }
      });
  }
  searchPilot() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }
  setCurrentPage(n: number) {
    this.dataTable.reset();
  }
  addPilot(){
    this.isNewPilot = true;
    this.selectedPilot = new Pilotclass;
    this.displayDialog = true;
    this.pilotForm.enable();
  }
  editPilot(pilots : Pilot){
    this.pilotForm.enable();
    this.isNewPilot = false;
    this.delete = false;
    this.selectedPilot = pilots;
    this.selectedDateOfBirth = this.selectedPilot.dateOfBirth;
    this.selectedDateActivated = this.selectedPilot.dateActivated;
    this.displayDialog = true;
  }
  savePilot() {
    let tmpPilotList = [...this.pilotList];
    this.selectedPilot.dateOfBirth = this.selectedDateOfBirth;
    this.selectedPilot.dateActivated  = this.selectedDateActivated;
    this.selectedPilot.dateOfBirth = this.datePipe.transform(this.selectedDateOfBirth, 'MM/dd/yyyy');
    this.selectedPilot.dateActivated = this.datePipe.transform(this.selectedDateActivated, 'MM/dd/yyyy');

    if(this.isNewPilot){
        this.globalService.addData<Pilot>(this.serviceName,this.selectedPilot).then(pilots => {
          tmpPilotList.push(pilots);
          this.pilotList = tmpPilotList;
          this.selectedPilot=null;
        });
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'info', summary:'Success', detail:'Added Pilot Details'});
        this.dataTable.reset();
    }else{
        this.globalService.updateData<Pilot>(this.serviceName,this.selectedPilot.pilotId, this.selectedPilot).then(pilots =>{
        tmpPilotList[this.pilotList.indexOf(this.selectedPilot)] = this.selectedPilot;
          this.pilotList=tmpPilotList;
          this.selectedPilot=null;
        });
        this.submitted = true;
        this.msgs = [];
        this.msgs.push({severity:'warn', summary:'Modified', detail:'Modified Pilot Details'});

    }
    this.pilotForm.markAsPristine();
    this.pilotList = tmpPilotList;
    this.selectedPilot = null;
    this.displayDialog = false;

  }
  saveAndNewPilot(){
    this.pilotForm.markAsPristine();
    let tmpPilotList = [...this.pilotList];
    tmpPilotList.push(this.selectedPilot);

    if(this.isNewPilot){
      this.globalService.addData<Pilot>(this.serviceName,this.selectedPilot);
      this.pilotList=tmpPilotList;
      this.isNewPilot = true;
      this.selectedPilot = new Pilotclass();
      this.msgs = [];
      this.msgs.push({severity:'info', summary:'Success', detail:'Added Pilot'});
    }
  }
  deletePilot(pilots : Pilot){
    this.pilotForm.disable();
    this.selectedPilot = pilots;
    this.displayDialog = true;
    this.delete = true;
 }

 delPilot(){
   this.confirmationService.confirm({
     message: 'Do you want to delete this record?',
     accept: () => {
     let index = this.findSelectedPilotIndex();
     this.pilotList = this.pilotList.filter((val,i) => i!=index);
     this.globalService.deleteData<Pilot>(this.serviceName,this.selectedPilot.pilotId);
     this.submitted = true;
     this.msgs = [];
     this.msgs.push({severity:'error', summary:'Deleted', detail:'Deleted Pilot Details'});
     this.selectedPilot = null;
     this.displayDialog = false;
     }
   });
 }
 
 cancelPilot(){
  if(this.pilotForm.dirty){
    this.confirmationService.confirm({
      message: 'Do you want to Discard this changes?',
      accept: () => {
        this.isNewPilot = false;
        let tmpPilotList = [...this.pilotList];
        tmpPilotList[this.pilotList.indexOf(this.selectedPilot)] = this.clonePilot;
        this.pilotList = tmpPilotList;
        this.selectedPilot = this.clonePilot;
        this.selectedPilot = null;

      }
    });
  }
}
  findSelectedPilotIndex(): number {
    return this.pilotList.indexOf(this.selectedPilot);
}
}

