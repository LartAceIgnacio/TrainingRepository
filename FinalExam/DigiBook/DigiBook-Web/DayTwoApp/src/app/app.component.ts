import { Component } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import {HttpClient} from '@angular/common/http';
import { AuthService } from './services/auth.service';
import { CanActivate, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'app';
  menuItems: MenuItem[];

  constructor(public auth: AuthService, private router: Router) {}

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit(): void {

    this.menuItems = [
      {label: 'Dashboard', icon: 'fa fa-home', routerLink: ['/dashboard']},
      {label: 'Employees', icon: 'fa fa-users', routerLink: ['/employees']},
      {label: 'Contacts', icon: 'fa fa-user-o', routerLink: ['/contacts']},
      {label: 'Venues', icon: 'fa fa-map-marker', routerLink: ['/venues']},
      {label: 'Appointments', icon: 'fa fa-map-marker', routerLink: ['/appointments']},
      { label: 'Login', icon: 'fa-lock', routerLink: ['/login'] },
    ];

  }
}

