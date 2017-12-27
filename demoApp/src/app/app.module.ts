import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { PanelModule } from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';

// const appRoutes: Routes = [
//   // { path: "", redirectTo: "/dashboard", pathMatch: "full" },
//   { path: "employees", component: EmployeesComponent },
//   { path: "contacts", component: ContactsComponent },
// ];

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    ContactsComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    PanelModule,
    AppRoutingModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
