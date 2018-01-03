import { Component, Injectable, OnInit } from '@angular/core';

import { TreeNode } from "primeng/components/common/treenode";
import {MenuItem} from 'primeng/primeng';
import { Http } from "@angular/http/http";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  menuItems: MenuItem[];

  ngOnInit(): void{
    this.menuItems=[
      {label: 'Dashboard', icon: 'fa fa-home', routerLink:['/dashboard']},
      {label: 'Employees', icon: 'fa fa-users', routerLink:['/employees']},
      {label: 'Contacts', icon: 'fa fa-address-book-o', routerLink:['/contacts']},
      {label: 'Appointments', icon: 'fa fa-calendar-check-o', routerLink:['/appointments']},
      {label: 'Venues', icon: 'fa fa-building-o', routerLink:['/venues']},
      {label: 'Google', icon: 'fa fa-google', url: 'https://www.google.com.ph/'}
    ];
  }
}


