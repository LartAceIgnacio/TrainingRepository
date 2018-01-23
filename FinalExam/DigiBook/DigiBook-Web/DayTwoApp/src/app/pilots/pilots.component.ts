import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../services/globalservice';
import { ConfirmationService } from 'primeng/primeng';
import { Pilot } from '../domain/pilots/pilot';
import { PilotClass } from '../domain/pilots/pilotclass';


@Component({
  selector: 'app-pilots',
  templateUrl: './pilots.component.html',
  styleUrls: ['./pilots.component.css'],
  providers: [GlobalService, ConfirmationService]
})
export class PilotsComponent implements OnInit {

  displayDialog: boolean;

  pilot: Pilot = new PilotClass();

  selectedPilot: Pilot;

  newPilot: boolean;

  pilots: Pilot[];

  constructor(private globalService: GlobalService) { }

  ngOnInit() {
    this.globalService.getSomething('pilots').then(pilots => this.pilots = pilots);
  }

  showDialogToAdd() {
    this.newPilot = true;
    this.pilot = new PilotClass();
    this.displayDialog = true;
  }

  save() {
    // tslint:disable-next-line:prefer-const
    let pilots = [...this.pilots];
    if (this.newPilot) {
      pilots.push(this.pilot);
    }else {
      pilots[this.findSelectedPilotIndex()] = this.pilot;
    }


    this.pilots = pilots;
    this.pilot = null;
    this.displayDialog = false;
  }

  delete() {
    // tslint:disable-next-line:prefer-const
    let index = this.findSelectedPilotIndex();
    this.pilots = this.pilots.filter((val, i) => i !== index);
    this.pilot = null;
    this.displayDialog = false;
  }

  onRowSelect(event) {
    this.newPilot = false;
    this.pilot = this.clonePilot(event.data);
    this.displayDialog = true;
  }

  clonePilot(p: Pilot): Pilot {
    // tslint:disable-next-line:prefer-const
    let pilot = new PilotClass();
    // tslint:disable-next-line:forin
    for (const prop in p) {
      pilot[prop] = p[prop];
    }
    return pilot;
  }

  findSelectedPilotIndex(): number {
    return this.pilots.indexOf(this.selectedPilot);
  }

}
