import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { AuthService } from './services/auth.service';
import { RouterLink } from '@angular/router/src/directives/router_link';
import { Router } from "@angular/router";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  menuItems: MenuItem[] = [];
  items: MenuItem[] = [];
  display : boolean = false;
  constructor(public auth : AuthService, private router: Router){ 

  }

  ngOnInit(): void{
    this.menuItems = [
      {label: 'Dashboard', icon: 'fa fa-home', routerLink: ['/dashboard']},
      {label: 'Employees', icon: 'fa fa-users', routerLink: ['/employees']},
      {label: 'Contacts', icon: 'fa fa-user-o', routerLink: ['/contacts']},
      {label: 'Appointments', icon: 'fa-handshake-o', routerLink: ['/appointments']},
      {label: 'Venues', icon: 'fa fa-university', routerLink: ['/venues']},
      {label: 'Google', icon: 'fa fa-tachometer', url: "http://google.com" },
    ];

    this.items = [
      {label: 'DigiBook', icon: 'fa fa-book fa-5x'}
    ];
    
    if(!this.auth.isLoggedIn()){
        this.router.navigate(["/login"]);
    }
  }

  
}
