import { Component } from '@angular/core';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  menuItems: MenuItem[]

  ngOnInit(): void {
    this.menuItems = [
      { label: 'Dashboard', icon: 'fa-tachometer', routerLink:['/dashboard'] },
      { label: 'Appointments', icon: 'fa-calendar-check-o', routerLink:['/appointments'] },
      { label: 'Contacts', icon: 'fa-id-card', routerLink:['/contacts'] },
      { label: 'Employees', icon: 'fa-users', routerLink:['/employees'] },
      { label: 'Venues', icon: 'fa-map-marker', routerLink:['/venues'] },
      { label: 'Google', icon: 'fa-refresh', url: "https://www.google.com" }
    ];
  }
}
