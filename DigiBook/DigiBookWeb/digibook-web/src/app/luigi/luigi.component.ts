import { Component, OnInit, transition, ViewChild } from '@angular/core';
import { MenuItem, ConfirmationService , DataTable} from 'primeng/primeng';
import { Luigi } from '../domain/luigi';
import { GlobalService } from '../services/globalservice';
import { LuigiClass } from '../domain/luigiclass';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Pagination } from '../domain/pagination';

@Component({
  selector: 'app-luigi',
  templateUrl: './luigi.component.html',
  styleUrls: ['./luigi.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class LuigiComponent implements OnInit {
  totalRecords: number = 0;
  venueList: any;
  paginationResult: Pagination<Luigi>;
  searchFilter: string = "";
  cloneLuigi: any;
  displayDialog: boolean;
  selectedLuigi: Luigi;
  isNewLuigi: boolean;
  btnSave: boolean;
  btnDelete: boolean;
  btnSaveNew: boolean;
  luigiItems: MenuItem[];
  clonedSeletedLuigi: Luigi;
  luigi: Luigi;
  luigiList: Luigi[];
  display: boolean = false;
  userform: FormGroup;
  showDialog() {
    this.display = true;
  }
  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
  private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.userform = this.fb.group({
      'firstName': new FormControl('', Validators.required),
      'lastName': new FormControl('', Validators.required)
    });
    this.luigiItems = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Luigi', routerLink: ['/luigi'] }
    ]
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Luigi>>("Luigis", event.first, event.rows,
      this.searchFilter.length == 1 ? "" : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.luigiList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
      });
  }

  searchLuigis() {
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

  addLuigis() {
    this.userform.enable();
    this.btnSaveNew = true;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewLuigi = true;
    this.selectedLuigi = new LuigiClass;
    this.displayDialog = true;
    this.userform.markAsPristine();
  }
  
  saveLuigis(){
    this.userform.enable();
    let tmpLuigiList = [...this.luigiList];
    if(this.isNewLuigi){
      this.globalService.addSomething("luigis" , this.selectedLuigi).then(luigis => {
      this.luigi = luigis;
      tmpLuigiList.push(this.luigi);
      this.luigiList = tmpLuigiList;
      console.log(this.luigi);
      });
    } else {
      this.globalService.updateSomething("luigis", this.selectedLuigi.luigiId , this.selectedLuigi);
      tmpLuigiList[this.luigiList.indexOf(this.selectedLuigi)] = this.selectedLuigi;
    }
    this.selectedLuigi = null;
    this.isNewLuigi = false;
    this.userform.markAsPristine();
  }

  saveNewLuigis(){
    this.userform.enable();
    let tmpLuigiList = [...this.luigiList];
    if(this.isNewLuigi){
      this.globalService.addSomething("luigis" , this.selectedLuigi).then(luigis => {
      this.luigi = luigis;
      tmpLuigiList.push(this.luigi);
      this.luigiList = tmpLuigiList;
      console.log(this.luigi);
      });
    } else {
      this.globalService.updateSomething("luigis", this.selectedLuigi.luigiId , this.selectedLuigi);
      tmpLuigiList[this.luigiList.indexOf(this.selectedLuigi)] = this.selectedLuigi;
    }
    this.selectedLuigi = new LuigiClass;
    this.userform.markAsPristine();
  }

  deleteConfirmation(Luigi : Luigi){
    this.userform.enable();
    this.btnSaveNew = false;
    this.btnDelete = true;
    this.btnSave = false;
    this.displayDialog = true;
    this.selectedLuigi = Luigi;
  }

  findSelectedLuigiIndex(): number {
    return this.luigiList.indexOf(this.selectedLuigi);
  }
  deleteLuigis(){
    let index = this.findSelectedLuigiIndex();
    this.luigiList = this.luigiList.filter((val, i) => i != index);
    this.globalService.deleteSomething("luigis", this.selectedLuigi.luigiId);

  }

  cloneRecord(r: Luigi): Luigi {
    let luigi = new LuigiClass();
    for (let prop in r) {
      luigi[prop] = r[prop];
    }
    return luigi;
  }

  editLuigis(Luigi: Luigi){
    this.userform.enable();
    this.btnSave = false;
    this.btnDelete = false;
    this.btnSave = true;
    this.isNewLuigi = false;
    this.selectedLuigi = Luigi;
    this.cloneLuigi = this.cloneRecord(this.selectedLuigi);
    this.userform.markAsPristine();
  }

}