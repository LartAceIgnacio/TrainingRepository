import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { PanelModule, MenuModule, DataTableModule,
  SharedModule, InputTextModule, ButtonModule, ToggleButtonModule, ChartModule,
   CalendarModule, BreadcrumbModule, DialogModule, ConfirmDialogModule
} from 'primeng/primeng';

import { AppComponent } from './app.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { AppRoutingModule } from './app-routing.module';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth-interceptor';
import { AuthResponseInterceptor} from './services/auth-response-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './user/register.component';
import { StatisticComponent } from './statistic/statistic.component';
import { AirportComponent } from './airport/airport.component';


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
    DashboardComponent,
    AppointmentsComponent,
    LoginComponent,
    RegisterComponent,
    StatisticComponent,
    AirportComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    PanelModule,
    MenuModule,
    CommonModule,
    DataTableModule,SharedModule,
    InputTextModule, ButtonModule, ConfirmDialogModule, ChartModule,
    ToggleButtonModule, CalendarModule,BreadcrumbModule,DialogModule,
    AppRoutingModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
],
  bootstrap: [AppComponent]
})
export class AppModule { }
