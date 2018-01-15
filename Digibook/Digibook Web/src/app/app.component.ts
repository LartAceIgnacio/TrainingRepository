import { Component } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { AuthService } from './services/authservice';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  menuItems: MenuItem[]

  constructor(public auth: AuthService) {  }

  ngOnInit(): void {
    this.menuItems = [
      { label: 'Statistics', icon: 'fa-tachometer', routerLink:['/statistic'] },
      { label: 'Calendar', icon: 'fa-calendar', routerLink:['/dashboard'] },
      { label: 'Appointments', icon: 'fa-calendar-check-o', routerLink:['/appointments'] },
      { label: 'Contacts', icon: 'fa-id-card', routerLink:['/contacts'] },
      { label: 'Employees', icon: 'fa-users', routerLink:['/employees'] },
      { label: 'Venues', icon: 'fa-map-marker', routerLink:['/venues'] },
      { label: 'Google', icon: 'fa-refresh', url: "https://www.google.com" }
    ];
  }
}
