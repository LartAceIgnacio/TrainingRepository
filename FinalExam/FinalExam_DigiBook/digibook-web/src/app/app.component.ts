import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { HttpClient } from "@angular/common/http";
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  menuItems: MenuItem[] = []
  items: MenuItem[];
  visibleSidebar1;

  constructor(private http: HttpClient, public auth: AuthService) {

  }
  ngOnInit(): void {
    this.menuItems = [
      { label: 'Dashboard', icon: 'fa fa-bookmark-o', routerLink: ['/dashboard'] },
      { label: 'Contacts', icon: 'fa-address-card', routerLink: ['/contacts'] },
      { label: 'Employees', icon: 'fa-download', routerLink: ['/employees'] },
      { label: 'Venues', icon: 'fa fa-building', routerLink: ['/venues'] },
      { label: 'Appointments', icon: 'fa fa-calendar', routerLink: ['/appointments'] },
      { label: 'Luigi', icon: 'fa-address-card', routerLink: ['/luigi']},
      { label: 'Flight', icon: 'fa fa-calendar', routerLink: ['/flight']}
    ];
    this.items = [
      {
        icon: "fa fa-book fa-5x", label: 'DigiBook', routerLink: ['/dashboard']
      }
    ];
  }
}
