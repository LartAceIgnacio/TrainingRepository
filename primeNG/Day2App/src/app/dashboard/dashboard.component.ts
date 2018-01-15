import { Component, OnInit } from '@angular/core';
import { GlobalService } from "../services/globalservice";
import { Appointment } from "../domain/appointment";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers: [GlobalService, DatePipe]
})

export class DashboardComponent implements OnInit {
  events: any[];
  event: any;
  header: any;
  appointmentList: Appointment[];

  constructor(private globalService: GlobalService, private datePipe: DatePipe) { }

  ngOnInit() {
    this.events = [];
    this.globalService.getSomething<Appointment>("Appointments").then(appointments => {
      this.appointmentList = appointments;
      for (var i = 0; i < this.appointmentList.length; i++) {
        var dateFormatStart = this.datePipe.transform(this.appointmentList[i].appointmentDate, 'yyyy-MM-dd');
        this.events[i] = 
          {
            "id": this.appointmentList[i].appointmentId,
            "title": this.appointmentList[i].notes,
            "start": `${dateFormatStart}T${this.appointmentList[i].startTime}`,
            "end": `${this.appointmentList[i].appointmentDate}`
          };
        //this.events[i] = this.event;
      }
    });

    this.header = {
      left: 'prev,next today',
      center: 'title',
      right: 'month,agendaWeek,agendaDay'
    };
  }
}

export class ContactEvent {
  id: any;
  title: string;
  start: string;
  end: string;
}
