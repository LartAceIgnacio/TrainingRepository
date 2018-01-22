import { Component } from '@angular/core';
import { MenuItem, SelectItem } from 'primeng/primeng';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'app';
  menuItems: MenuItem[];

  constructor(private http: HttpClient, public auth: AuthService) { }


  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit(): void {
    this.menuItems = [
      { label: 'Dashboard', icon: 'fa fa-home', routerLink: ['/dashboard'] },
      { label: 'Employees', icon: 'fa fa-users', routerLink: ['/employees'] },
      { label: 'Contacts', icon: 'fa fa-address-book', routerLink: ['/contacts'] },
      { label: 'Venues', icon: 'fa fa-map-marker', routerLink: ['/venues'] },
      { label: 'Appointments', icon: 'fa fa-calendar', routerLink: ['/appointments'] },
      { label: 'Inventories', icon: 'fa fa-shopping-bag', routerLink: ['/inventories'] },
      // { label: 'Login', icon: 'fa-lock', routerLink: ['/login'] },
      // { label: 'Google', icon: 'fa fa-google', url: 'https://www.google.com' }
    ];
  }
}
