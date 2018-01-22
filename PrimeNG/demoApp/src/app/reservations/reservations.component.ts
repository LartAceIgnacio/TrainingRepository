import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbModule, MenuItem } from 'primeng/primeng';
import { GlobalService } from "../services/globalservice";
import { HttpClient } from "@angular/common/http";
import { Reservation } from "../domain/Reservation";
import { DataTable } from "primeng/components/datatable/datatable";
import { PaginationResult } from "../domain/paginationresult";
import {Validators,FormControl,FormGroup,FormBuilder} from '@angular/forms';
import { ReservationClass } from "../domain/ReservationClass";
import { ConfirmDialogModule, ConfirmationService, Message } from 'primeng/primeng';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class ReservationsComponent implements OnInit {

  brReservation: MenuItem[];
  home: MenuItem;

  selectedReservation: Reservation;
  cloneReservation: Reservation;
  isNewReservation: boolean;
  display: boolean;

  reservationForm: FormGroup;
  isDelete: boolean;

  reservationList: Reservation[];
  paginationResult: PaginationResult<Reservation>;
  searchFilter: string ="";
  totalRecords: number = 0;

  invalidDates: Date;

  constructor(
    private globalservice : GlobalService,
    private http: HttpClient,
    private fb: FormBuilder,
    private confirmationService: ConfirmationService) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    this.reservationForm = this.fb.group({
      'venuename': new FormControl('', Validators.required),
      'description': new FormControl(''),
      'startdate': new FormControl('', Validators.required),
      'enddate': new FormControl('', Validators.required)
    });
    this.brReservation = [{label:'Reservation', url:'/reservations'}]
    this.home = {icon: 'fa fa-home', routerLink: '/dashboard'}
  }

  paginate(event){
    this.globalservice.getSomethingWithPagination<PaginationResult<Reservation>>(
      "Reservations", event.first, event.rows, this.searchFilter.length == 1 ? "" :
      this.searchFilter).then(paginationResult =>{
        this.paginationResult = paginationResult;
        this.reservationList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (var i = 0; i < this.reservationList.length; i++) {
          this.reservationList[i].startDate = this.reservationList[i].startDate == null ? null :
            new Date(this.reservationList[i].startDate).toLocaleDateString();
          this.reservationList[i].endDate = this.reservationList[i].endDate == null ? null :
            new Date(this.reservationList[i].endDate).toLocaleDateString();
        }
      }); 

      this.invalidDates = new Date();
      let yesterday = new Date();
      this.invalidDates.setDate(yesterday.getDate());
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
  }

  searchReservation() {
    if (this.searchFilter.length != 1) {
      this.setCurrentPage(1);
    }
  }

  addReservation(){
    this.isDelete = false;
    this.display = true;
    this.reservationForm.markAsPristine();
    this.reservationForm.enable();
    this.isNewReservation = true;
    this.selectedReservation = new ReservationClass();
  }

  saveAndNewReservation(){
    this.reservationForm.markAsPristine();

    let tmpReservationList = [...this.reservationList];

    this.globalservice.addSomething<Reservation>("Reservations", this.selectedReservation).then(reservations =>{
      tmpReservationList.push(reservations);
      this.reservationList = tmpReservationList;
    });

    this.isNewReservation = true;
    this.selectedReservation = new ReservationClass();
  }

  cloneRecord(r: Reservation): Reservation{
    let reservation = new ReservationClass();
    for(let prop in r){
      reservation[prop] = r[prop];
    }
    return reservation;
  }


  editReservation(Reservation: Reservation){
    this.reservationForm.markAsPristine();
    this.reservationForm.enable();
    this.isDelete = false;
    this.selectedReservation=Reservation;
    this.cloneReservation = this.cloneRecord(this.selectedReservation);
    this.display=true;
    this.isNewReservation = false;
  }

  saveReservation(){
    let tmpReservationList = [...this.reservationList];
    if (this.isNewReservation) {
      this.globalservice.addSomething<Reservation>("Reservations", this.selectedReservation).then(reservations =>{
        tmpReservationList.push(reservations);
        this.reservationList=tmpReservationList;
        this.selectedReservation = null;
      });
    } 
    else {
      this.globalservice.updateSomething<Reservation>("Reservations",this.selectedReservation.reservationId,this.selectedReservation)
      .then(reservations =>{
        tmpReservationList[this.reservationList.indexOf(this.selectedReservation)] = this.selectedReservation;
        this.reservationList=tmpReservationList;
        this.selectedReservation=null;
      });
    }
    this.isNewReservation = false;
  }

  findSelectedReservationIndex(): number{
    return this.reservationList.indexOf(this.selectedReservation);
  }

  cancelReservation(){
    let index = this.findSelectedReservationIndex();
    if (this.isNewReservation) {
      this.selectedReservation = null;
    }
    else{
      this.confirmationService.confirm({
        message: 'Are you sure that you want discard changes?',
        header: 'Confirmation',
        icon: 'fa fa-question-circle',
        accept: () => {
          let tmpReservationList = [...this.reservationList];
          tmpReservationList[index] = this.cloneReservation;
          this.reservationList = tmpReservationList;
          this.selectedReservation = Object.assign({}, this.cloneReservation);
          this.display = false;
        }
      });
    } 
  }

  confirmDelete(Reservation: Reservation){
    this.reservationForm.markAsPristine();
    this.cloneReservation = this.cloneRecord(this.selectedReservation);
    this.selectedReservation = Reservation;
    this.isDelete = true;
    this.display = true;
    this.reservationForm.disable();
    this.isNewReservation = false;
  }

  deleteReservation(){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'fa fa-trash',
      accept: () => {
        let index = this.findSelectedReservationIndex();
        this.reservationList = this.reservationList.filter((val,i) => i=index);
        this.globalservice.deleteSomething<Reservation>("Reservations", this.selectedReservation.reservationId);
        this.selectedReservation = null;
      }
    });
  }

}
