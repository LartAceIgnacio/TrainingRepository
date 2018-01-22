import { Component } from '@angular/core';
// added
import { MenuItem } from 'primeng/primeng';
import { AuthService } from './services/common/Authentication/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  test : number = 4;
  test1 : string = 'String btn';
  menuItems: MenuItem[];
  
  constructor(public auth: AuthService) {
    
  }
  
  ngOnInit(): void {
    this.menuItems = [
      {label: 'DashBoard', icon: 'fa-code', routerLink: ['/dashboard']},
      {label: 'Employee', icon: 'fa-user-circle', routerLink: ['/employee']},
      {label: 'Contact', icon: 'fa-user-plus', routerLink: ['/contact']},
      {label: 'Appointment', icon: 'fa-handshake-o ', routerLink: ['/appointment']},
      
  ];
  };

}
