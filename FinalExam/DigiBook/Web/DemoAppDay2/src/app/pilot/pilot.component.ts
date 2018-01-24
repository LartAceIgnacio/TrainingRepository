import { Component, OnInit } from '@angular/core';
import { Pilot } from '../domain/pilot/pilot';
import { PilotClass } from '../domain/pilot/pilot.class';

//validation
import { Message, SelectItem } from 'primeng/components/common/api';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { GenericService } from '../services/common/generic.service';
import { Pagination } from '../domain/common/pagination';

import { invoke } from 'q';

import { ConfirmationService, DataTable } from 'primeng/primeng';
import { ViewChild } from '@angular/core';
import { AuthService } from '../services/common/Authentication/auth.service';

@Component({
  selector: 'app-pilot',
  templateUrl: './pilot.component.html',
  styleUrls: ['./pilot.component.css'],
  providers: [GenericService]
})
export class PilotComponent implements OnInit {
  pilotList: Pilot[];
  selectedPilot: Pilot;
  clonePilot: Pilot;
  isNewPilot: boolean;

  // pagination
  entity: string = "Pilot";
  searchQuery: string = "";
  pageNumber: number = 0;
  rowCount: number = 5;
  totalRecords: number;
  @ViewChild('dt') public dataTable: DataTable;


  indexSelected: any; // for editing
  clonedSelectedPilot: Pilot; // for editing

  //validations
  msgs: Message[] = [];

  userform: FormGroup;


  submitted: boolean;

  description: string;

  constructor(
    private genericService: GenericService,
    private fb: FormBuilder,
    public authService: AuthService
  ) { }

  ngOnInit() {

    //validations
    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required),
      'middleName': new FormControl('',),
      'dateOfBirth': new FormControl('', Validators.required),
      'yearsOfExperience': new FormControl('', Validators.required),
      'dateActivated': new FormControl('', Validators.required),
    });

  }
  formSubmitted(message): void {
    this.submitted = true;
    this.msgs = [];
    this.msgs.push({ severity: 'info', summary: 'Success', detail: message });
  }

  clearContent(tmpPilotList) {
    this.pilotList = tmpPilotList;
    //this.selectedPilot = null;
    // this.clonedSelectedPilot = null;
    this.clonedSelectedPilot = new PilotClass();
    this.userform.markAsPristine();
    this.isNewPilot = false;
  }

  // v2
  cancelForEdit: boolean = true;
  displayModal: boolean = false;

  showAddAndNew: boolean = false;
  showAdd: boolean = false;
  showDelete: boolean = false;

  disableForm: boolean = false;

  checkIfEmpty = (event) => {
    console.log(event);
    if (event == "") {
      this.search();
    }
  }

  search = () => {

    if(this.searchQuery.length >= 2) {
      this.dataTable.reset();
    } 

    if(this.searchQuery.length == 0){
      this.dataTable.reset();
    }

    if (this.searchQuery.length == 1) {
      this.formSubmitted("Search key must be 2 or more characters!");
    }
  }


  hideModalBtn = () => {
    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;
  }


  add = () => {
    this.cancelForEdit = false;
    // this.toggleForm(false);
    this.userform.markAsPristine();
    this.userform.enable();
    this.isNewPilot = true;
    this.clonedSelectedPilot = new PilotClass();

    this.showAdd = true;
    this.showAddAndNew = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  cancel = () => {
    if (this.cancelForEdit) {

      if(JSON.stringify(this.clonedSelectedPilot) === JSON.stringify(this.selectedPilot)) {
        this.isNewPilot = false;
        this.clonedSelectedPilot = new PilotClass();
        this.userform.markAsPristine();
        this.displayModal = false;
        this.hideModalBtn();
      } else {
        this.clonedSelectedPilot = JSON.parse(JSON.stringify(this.selectedPilot));
      }
      // this.hideModalBtn();
    }
    else {
      this.isNewPilot = false;
      this.clonedSelectedPilot = new PilotClass();
      this.userform.markAsPristine();
      this.displayModal = false;
      this.hideModalBtn();
    }
  }

  save = (invoker) => {
    let tmpPilotList = [...this.pilotList];
    if (this.isNewPilot) {

      this.genericService.Create<Pilot>(this.entity, this.clonedSelectedPilot)
        .then(data => {
          tmpPilotList.push(data);
          this.clearContent(tmpPilotList);
          // alert('Success!');
          this.formSubmitted("Pilot Details Submitted!");

          this.displayModal = invoker == "Save" ? false : true;
          this.isNewPilot = invoker == "Save" ? false : true;
          this.dataTable.reset();
        });

    } else {
      this.genericService.Update<Pilot>(this.entity, this.clonedSelectedPilot.pilotId, this.clonedSelectedPilot)
        .then(data => {
          tmpPilotList[this.indexSelected] = this.clonedSelectedPilot;
          this.clearContent(tmpPilotList);
          // alert('Success!');
          this.formSubmitted("Updated Pilot Details!");
          this.displayModal = false;
          this.hideModalBtn();
          this.dataTable.reset();
        });
    }
  }

  edit = (rowData) => {
    this.cancelForEdit = true;

    // this.toggleForm(false);
    this.userform.enable();
    this.selectedPilot = rowData;
    this.indexSelected = this.pilotList.indexOf(this.selectedPilot); // value will be the index used for editing
    this.clonedSelectedPilot = JSON.parse(JSON.stringify(this.selectedPilot)); // cloned value of selected
    this.isNewPilot = false;

    this.showAddAndNew = false;
    this.showAdd = true;
    this.showDelete = false;

    this.displayModal = true;
  }

  setdeletion = (rowData) => {
    // this.toggleForm(true, rowData);
    this.cancelForEdit = false;

    this.userform.disable();
    this.selectedPilot = rowData;

    this.indexSelected = this.pilotList.indexOf(this.selectedPilot); // value will be the index used for editing

    this.clonedSelectedPilot = JSON.parse(JSON.stringify(this.selectedPilot)); // cloned value of selected
    this.isNewPilot = false;

    this.showAddAndNew = false;
    this.showAdd = false;
    this.showDelete = true;

    this.displayModal = true;
  }

  delete = () => {
    if (this.indexSelected > -1) {
      let tmpPilotList = [...this.pilotList];

      this.genericService
        .Delete(this.entity, this.clonedSelectedPilot.pilotId)
        .then(res => {
          console.log(res);
          if (res === 204) {
            tmpPilotList.splice(this.indexSelected, 1);
            this.pilotList = tmpPilotList;
            this.clonedSelectedPilot = null;
            this.indexSelected = -1;
            // alert('Success!');
            this.displayModal = false;
            // this.toggleForm(false, new PilotClass);
            this.disableForm = false;
            this.hideModalBtn();
            this.userform.markAsPristine();
            this.formSubmitted("Pilot Deleted!");
            this.dataTable.reset();
          } else {
            alert("Failed to Delete!");
          }
        });

    }
  }

  paginate = (event) => {
    this.genericService.Retrieve<Pagination<Pilot>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        console.log(result);
        
        for (var i = 0; i < result.result.length; i++) {
          result.result[i].dateOfBirth = new Date(result.result[i].dateOfBirth).toLocaleDateString();
          result.result[i].dateActivated = new Date(result.result[i].dateActivated).toLocaleDateString();
          result.result[i]["middleInitial"] = result.result[i].middleName.substring(0,1).toUpperCase();
        }

        this.pilotList = result.result;
        this.totalRecords = result.totalCount;
      });
  }
}
