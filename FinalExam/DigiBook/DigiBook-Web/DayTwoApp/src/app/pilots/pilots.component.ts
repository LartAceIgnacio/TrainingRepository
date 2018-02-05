import { Component, OnInit } from '@angular/core';
import { Pilot } from '../domain/pilots/pilot';
import { PilotClass } from '../domain/pilots/pilotclass';
import { MenuItem, ConfirmationService, DataTable } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-pilots',
  templateUrl: './pilots.component.html',
  styleUrls: ['./pilots.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class PilotsComponent implements OnInit {

  items: MenuItem[];

  home: MenuItem;

  displayDialog: boolean;

  pilot: Pilot = new PilotClass();

  selectedPilot: Pilot;

  newPilot: boolean;

  pilots: Pilot[];

  isDelete: boolean;

  isEdit: boolean;

  isNewPilot: boolean;

  loading: boolean;

  userform: FormGroup;

  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';

  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  paginationResult: PaginationResult<Pilot>;

  today: Date = new Date();

  // tslint:disable-next-line:no-inferrable-types
  minAge: number = 21;

  maxYear: Date = new Date(this.today.getFullYear() - this.minAge, this.today.getMonth(), this.today.getDate());

  // tslint:disable-next-line:no-inferrable-types
  yearRange = '1950 :' + this.maxYear.getFullYear();

  constructor(private globalService: GlobalService, private confirmationService: ConfirmationService, private fb: FormBuilder) { }

  @ViewChild('dt') public dataTable: DataTable;
  ngOnInit() {
    // this.contactService.getContacts().then(pilots => this.pilots = pilots);
    // this.contactService.getContacts()
    // .then(pilots => this.pilots = pilots);

    this.loading = true;
    setTimeout(() => {
      // this.contactService.getContacts().then(pilots => this.pilots = pilots);
      this.loading = false;
    }, 1000);


    this.items = [
      { label: 'Home', routerLink: ['/dashboard'] },
      { label: 'Pilot' }
    ];
    this.home = { icon: 'fa fa-home' };

    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(60)])),
      'middlename': new FormControl(''),
      'lastname': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(60)])),
      'dateofbirth': new FormControl('', Validators.required),
      'yearsofexperience': new FormControl('', Validators.compose([Validators.required, Validators.min(10)])),
      'dateactivated': new FormControl('', Validators.required)
    });

  }

  showDialogToAdd() {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = true;
    this.isDelete = false;
    this.newPilot = true;
    this.selectedPilot = null;
    this.pilot = new PilotClass();
    this.displayDialog = true;
  }

  onRowSelect(clickPilot: Pilot) {
    this.newPilot = false;
    this.pilot = this.clonePilot(clickPilot);
    this.selectedPilot = clickPilot;
    this.displayDialog = true;
    this.pilot.dateOfBirth = new Date(this.selectedPilot.dateOfBirth).toLocaleDateString();
    this.pilot.dateActivated = new Date(this.selectedPilot.dateActivated).toLocaleDateString();

  }

  clonePilot(c: Pilot): Pilot {
    const pilot = new PilotClass();
    // tslint:disable-next-line:forin
    for (const prop in c) {
      pilot[prop] = c[prop];
    }
    return pilot;
  }

  save(number: boolean) {
    this.isNewPilot = number;

    const pilots = [...this.pilots];
    if (this.newPilot) {
      this.globalService.addSomething('Pilots', this.pilot).then(data => {
        this.pilot = data;
        this.pilot.dateOfBirth = new Date(this.pilot.dateOfBirth).toLocaleDateString();
        this.pilot.dateActivated = new Date(this.pilot.dateActivated).toLocaleDateString();
        pilots.push(this.pilot);
        // this.totalRecords = this.totalRecords + 1;
        this.pilots = pilots;
        this.dataTable.reset();

      });
    } else {
      this.globalService.updateSomething('Pilots', this.pilot.pilotId, this.pilot).then(
        data => {
          this.pilot = data;
          pilots[this.findSelectedContactIndex()] = this.pilot;
          this.pilots = pilots;
          this.pilot = null;
          this.dataTable.reset();
        }
      );
    }

    if (this.isNewPilot) {
      this.userform.markAsPristine();
      this.pilot = null;
      // this.pilots = pilots;
      this.newPilot = true;
      this.selectedPilot = null;
      this.pilot = new PilotClass();
      // this.dataTable.reset();

    } else {
      this.pilot = null;
      this.userform.markAsPristine();
      this.displayDialog = false;

    }

  }

  findSelectedContactIndex(): number {
    return this.pilots.indexOf(this.selectedPilot);
  }

  delete(clickPilot: Pilot) {
    this.isDelete = true;
    this.userform.disable();
    this.onRowSelect(clickPilot);

  }

  deletePilot() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to perform this action?',
      accept: () => {
        this.globalService.deleteSomething('Pilots', this.pilot.pilotId);
        const index = this.findSelectedContactIndex();
        this.pilots = this.pilots.filter((val, i) => i !== index);
        this.pilot = null;
        this.displayDialog = false;
        this.dataTable.reset();
      }
    });
  }

  edit(clickPilot: Pilot) {
    this.userform.enable();
    this.userform.markAsPristine();
    this.isEdit = false;
    this.isDelete = false;
    this.onRowSelect(clickPilot);
  }

  cancel() {
    if (this.isDelete) {
      this.displayDialog = false;
    } else {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to cancel?',
        accept: () => {
          this.pilot = Object.assign({}, this.selectedPilot);
        }
      });
    }
  }

  paginate(event) {
    // event.first = Index of the first record
    // event.rows = Number of rows to display in new page
    // event.page = Index of the new page
    // event.pageCount = Total number of pages
    // tslint:disable-next-line:max-line-length
    this.globalService.getSomethingWithPagination<PaginationResult<Pilot>>('Pilots', event.first, event.rows, this.searchFilter.length === 1 ? '' : this.searchFilter)
      .then(paginationResult =>
      // tslint:disable-next-line:one-line
      {
        console.log(paginationResult);
        this.paginationResult = paginationResult;
        this.pilots = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (let i = 0; i < this.pilots.length; i++) {
          this.pilots[i].dateCreated = this.pilots[i].dateCreated == null ? null :
            new Date(this.pilots[i].dateCreated).toLocaleDateString();

          this.pilots[i].dateOfBirth = new Date(this.pilots[i].dateOfBirth).toLocaleDateString();
          this.pilots[i].dateActivated = new Date(this.pilots[i].dateActivated).toLocaleDateString();
        }
      });
  }

  searchPilot() {
    if (this.searchFilter.length !== 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
    // let paging = {
    //     first: ((n - 1) * this.dataTable.rows),
    //     rows: this.dataTable.rows
    // };
    // this.dataTable.paginate();
  }
}


class PaginationResultClass implements PaginationResult<Pilot> {
  constructor(public results, public pageNo, public recordPage, public totalRecords) {

  }
}

