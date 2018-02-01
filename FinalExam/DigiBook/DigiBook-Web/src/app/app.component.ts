import { Component, OnInit, Injectable } from '@angular/core';

import { MenuItem } from 'primeng/primeng';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  {
  title = 'app';
  results = '';

  items: MenuItem[];
  header:MenuItem[];

  constructor(public auth: AuthService) { }

  ngOnInit():void  {
    
      this.items = [
        {label: 'Dashboard', icon: 'fa-home', routerLink:['/dashboard'] },
        {label: 'Employees', icon: 'fa-users', routerLink:['/employees']},
        {label: 'Contacts', icon: 'fa-phone', routerLink:['/contacts']},
        {label: 'Appointments', icon: 'fa-address-book', routerLink:['/appointments']},
        {label: 'Venues', icon: 'fa-address-book', routerLink:['/venues']},
        {label: 'Pilot', icon: 'fa-address-book', routerLink:['/pilot']}
        // {label: 'Google', icon: 'fa-google' , url:"https://www.google.com"}
    ];
    this.header = [
      { 
        icon: 'fa-book fa-3x',
        label: 'DigiBook'
      }
    ]

  }
  
}
