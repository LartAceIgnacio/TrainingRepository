import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { PanelModule, MenuModule, DataTableModule,
  SharedModule, InputTextModule, ButtonModule
} from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

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
    MenuModule,
    DataTableModule,
    SharedModule, InputTextModule, ButtonModule,
    AppRoutingModule,
    HttpModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
