import { Component } from '@angular/core';
import { MenuItem } from "primeng/primeng";
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  menuItems: MenuItem[]

  constructor(public auth: AuthService) {}

  ngOnInit() : void{
    this.menuItems = [
      {label: 'Dashboard', icon: 'fa-home', routerLink:['/dashboard']},
      {label: 'Employees', icon: 'fa-users', routerLink:['/employee']},
      {label: 'Contacts', icon: 'fa-id-card-o', routerLink:['/contact']},
      {label: 'Venue', icon: 'fa-building-o', routerLink:['/venue']},
      {label: 'Appointment', icon: 'fa-handshake-o', routerLink:['/appointment']},
    ];
  }
}
