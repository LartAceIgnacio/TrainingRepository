import { Component, Injectable, OnInit } from '@angular/core';

import { TreeNode } from "primeng/components/common/treenode";
import {MenuItem} from 'primeng/primeng';
import { Http } from "@angular/http/http";
import { AuthService } from "./services/auth.service";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  menuItems: MenuItem[];

  constructor(public auth: AuthService) {
    
  }

  ngOnInit(): void{
    this.menuItems=[
      {label: 'Dashboard', icon: 'fa fa-home', routerLink:['/dashboard']},
      {label: 'Employees', icon: 'fa fa-users', routerLink:['/employees']},
      {label: 'Contacts', icon: 'fa fa-address-book-o', routerLink:['/contacts']},
      {label: 'Appointments', icon: 'fa fa-calendar-check-o', routerLink:['/appointments']},
      {label: 'Departments', icon: 'fa fa-address-book', routerLink:['/departments']},
      {label: 'Flights', icon: 'fa fa-plane', routerLink:['/flights']}
    ];
  }
}


