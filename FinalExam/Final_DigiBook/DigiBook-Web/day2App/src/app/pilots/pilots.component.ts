import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MenuItem, DataTable } from 'primeng/primeng';
import { GenericService } from '../services/genericservice';
import { Validators, FormControl, FormGroup, FormBuilder} from '@angular/forms';

import { Message, SelectItem } from 'primeng/components/common/api';
import { ViewChild } from '@angular/core';
import { Record } from '../domain/record';
import { Pilot } from '../domain/pilot';
import { PilotClass } from '../domain/pilotclass';


@Component({
  selector: 'app-pilots',
  templateUrl: './pilots.component.html',
  styleUrls: ['./pilots.component.css'],
  providers: [GenericService, ConfirmationService]
})
export class PilotsComponent implements OnInit {
  userform : FormGroup;
  items : MenuItem[] = [];

  btnSave : boolean = true;
  btnSaveNew : boolean = true;
  btnDelete : boolean = true;
  
  display: boolean;

  msgs: Message[] = [];
  pilotList: Pilot[];
  selectedPilot: Pilot;
  clonePilot: Pilot;
  isNewPilot: boolean;
  pilot : Pilot;
  isEdit: boolean;

   //datatable components
  loading: boolean;
  searchFilter: string = "";
  totalRecord: number = 0;
  searchButtonClickCtr: number = 0;
  retrieveRecordResult: Record<Pilot>;
 
  service: string = "Pilots";
  constructor(private genericService: GenericService, private confirmationService: ConfirmationService, private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {
    this.loading = true;
    this.items = [
      {label: 'Dashboard', icon: 'fa fa-book fa-5x', routerLink: ['/dashboard']},
      {label: 'Pilot', icon: 'fa fa-book fa-5x', routerLink: ['/Pilot']}
    ];

    this.userform = this.fb.group({
      'firstName' : new FormControl('', Validators.required),
      'lastName' : new FormControl('', Validators.required),
      'middleName': new FormControl(''),
      'dateActivated' : new FormControl('', Validators.required),
      'birthDate': new FormControl('', Validators.compose([Validators.required])),
      'yearsOfExperience': new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]*$')])),
    });
  }

  retrieveRecord(event){
    this.genericService.getPaginatedRecord<Record<Pilot>>(this.service, event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then( pilotRecord => {
          this.retrieveRecordResult = pilotRecord;
          this.pilotList = this.retrieveRecordResult.result;
          this.totalRecord = this.retrieveRecordResult.totalRecord;
          for(var i = 0; i < this.pilotList.length; i++)
          {
            this.pilotList[i].middleInitial = this.pilotList[i].middleName.slice(0, 1);
            this.pilotList[i].birthDate = new Date(this.pilotList[i].birthDate).toLocaleDateString();
            this.pilotList[i].dateActivated = new Date(this.pilotList[i].birthDate).toLocaleDateString();
          }
          this.loading = false;
          
    });
  }

  addPilot(){
    this.userform.markAsPristine();
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnSave = true;
    this.btnDelete = false;
    this.display = true;
    this.isNewPilot = true;
    this.selectedPilot = new PilotClass();
  }

  savePilot(saveNew: boolean){
    let tmtPilotList = [...this.pilotList];
    this.msgs = [];
    if(this.isNewPilot)
    {
      this.genericService.insertRecord(this.service, this.selectedPilot).then(pilot => 
        {
        this.pilot = pilot; 
        tmtPilotList.push(this.pilot);
        this.pilotList = tmtPilotList;
        this.selectedPilot = saveNew? new PilotClass(): null;
        this.isNewPilot = saveNew ? true : false;
        this.dataTable.reset();
        })
        .then( emp => this.msgs.push({severity:'success', summary:'Success Message', detail:'New Pilot: '+ this.pilot.lastName +' Added'}));
    }
    else{
      this.genericService.updateRecord(this.service, this.selectedPilot.pilotId, this.selectedPilot).then(pilot =>
        {this.pilot = pilot; 
        tmtPilotList[this.pilotList.indexOf(this.selectedPilot)] = pilot;
        this.pilotList = tmtPilotList;
        this.display = false;
        this.dataTable.reset();
        });
    }
    this.userform.markAsPristine();
  }

  search(){
    this.dataTable.reset();
  }

  cancelPilot(){
    if(this.isNewPilot ){
      this.selectedPilot = null;
    }
    else{
      if(this.isEdit)
        {
          if(this.userform.dirty){
            this.confirmationService.confirm({
              message: 'Are you sure you want to discard changes?',
              header: 'Discard Changes',
              icon: 'fa fa-pencil',
              accept: () => {
                let tmpPilotList = [...this.pilotList];
                tmpPilotList[this.pilotList.indexOf(this.selectedPilot)] = this.clonePilot;
                this.pilotList = tmpPilotList;
                this.selectedPilot = null;
              },
              reject: () => {

              }
            });
            this.userform.markAsPristine();
          }else{
            this.selectedPilot = null;
          }
        }else{
          this.selectedPilot = null;
        }
      }
      this.userform.markAsPristine();
    }

  cloneRecord(r: Pilot): Pilot{
    let pilot = new PilotClass();
    for(let prop in r){
      pilot[prop] = r[prop];
    }
    return pilot;
  }

  deletePilot(pilot: Pilot){
    this.userform.markAsPristine();
    this.isEdit = false;
    this.userform.disable();
    this.clonePilot = this.cloneRecord(pilot);
    this.selectedPilot = pilot;
    this.display = true;
    // hide the Saving button
    this.btnSave = false; 
    this.btnSaveNew = false;     
    this.btnDelete = true;
  } 
  
  deletePilotConfirmation(){
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this record?',
      header: 'Discard Changes',
      icon: 'fa fa-pencil',
      accept: () => {
        let tmpPilotList = [...this.pilotList];
        this.genericService.deleteRecord(this.service, this.selectedPilot.pilotId);
        tmpPilotList.splice(this.pilotList.indexOf(this.selectedPilot), 1);
    
        this.pilotList = tmpPilotList;
        this.selectedPilot   = null;
        this.msgs = [{severity:'info', summary:'Confirmed', detail:'Record deleted'}];
      },
      reject: () => {
      }
    });
  }
    
  editPilot(pilot: Pilot){
    this.isEdit = true;
    this.userform.enable();
    this.btnSave = true;
    this.btnDelete = false;
    this.btnSaveNew = false;
    this.isNewPilot = false;
    this.clonePilot = this.cloneRecord(pilot);
    this.selectedPilot = pilot;
    this.display = true;
  }
}
