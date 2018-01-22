import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})



export class AppComponent implements OnInit {

  constructor(public auth: AuthService) {

  }

  ngOnInit(): void {
    this.menuItems = [
      { label: 'Dashboard', icon: 'fa fa-home', routerLink: ['/dashboard'] },
      { label: 'Employees', icon: 'fa fa-users', routerLink: ['/employees'] },
      { label: 'Contacts', icon: 'fa fa-phone', routerLink: ['/contacts'] },
      { label: 'Appointments', icon: 'fa fa-calendar-o', routerLink: ['/appointments'] },
      { label: 'Login', icon: 'fa-lock', routerLink:['/login'] },
    ]
  }

  title = 'Training PrimeNg';
  menuItems: MenuItem[];

}
