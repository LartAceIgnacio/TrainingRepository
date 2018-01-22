import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  item: MenuItem[];

  constructor(
    public auth: AuthService
  ) { }
  ngOnInit():void {
    this.item = [
      { label: 'Dashboard', icon: 'fa-home', routerLink: '/dashboard' }
      , { label: 'Employees', icon: 'fa-users', routerLink: '/employees' }
      , { label: 'Contacts', icon: 'fa-address-book', routerLink: '/contacts' }
     // , { label: 'Venues', icon: 'fa-map-marker', routerLink: '/venues' }
      , { label: 'Appointments', icon: 'fa-calendar', routerLink: '/appointments' }
     // , { label: 'Login', icon: 'fa-lock', routerLink:['/login'] }
    ];
  
    
  }

}
