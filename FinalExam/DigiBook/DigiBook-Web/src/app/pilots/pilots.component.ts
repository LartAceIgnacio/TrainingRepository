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

@Component({
  selector: 'app-pilots',
  templateUrl: './pilots.component.html',
  styleUrls: ['./pilots.component.css'],
  providers:[GlobalService, ConfirmationService]
})
export class PilotsComponent implements OnInit {

  indexSelected: number;
  isNewPilot: boolean;
  displayDialog: boolean;
  loading: boolean;
  delete:boolean ;
  submitted: boolean;

  serviceName: string = "Pilots";

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

  constructor(private globalService: GlobalService, private fb: FormBuilder, private confirmationService: ConfirmationService) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.pilotForm = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'middleName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'dateOfBirth': new FormControl('', Validators.required),
      'yearsOfExperience': new FormControl('', Validators.required),
      'dateActivated': new FormControl('', Validators.required),
      'dateCreated': new FormControl('', Validators.required),
      'dateModified': new FormControl('', Validators.required),
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
    this.globalService.getPagination<Pagination<Pilot>>(this.serviceName, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(pagination => {
        this.pagination = pagination;
        this.pilotList = this.pagination.results;
        this.totalRecords = this.pagination.totalRecords;
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
    //this.clonePilot = this.cloneRecord(this.selectedPilot);
    this.displayDialog = true;
  }
  findSelectedPilotIndex(): number {
    return this.pilotList.indexOf(this.selectedPilot);
}
}

