import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})



export class AppComponent implements OnInit{
  ngOnInit(): void {
    this.menuItems = [
      { label: 'Dashboard', icon: 'fa fa-home', routerLink:['/dashboard'] },
      { label: 'Employees', icon: 'fa fa-users', routerLink:['/employees'] },
      { label: 'Contacts', icon: 'fa fa-phone', routerLink:['/contacts'] },
      { label: 'Appointments', icon: 'fa fa-calendar-o', routerLink:['/appointments']}
    ]
  }

  title = 'Training PrimeNg';
  menuItems: MenuItem[];

}
