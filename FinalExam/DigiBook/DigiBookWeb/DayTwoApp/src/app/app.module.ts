import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';

import { MenuModule } from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';
import {
  DataTableModule, SharedModule, ButtonModule, InputTextModule, PanelModule,
  DropdownModule, InputTextareaModule, CalendarModule, InputSwitchModule, ConfirmDialogModule,
  ConfirmationService, DialogModule, BreadcrumbModule, ChartModule, CheckboxModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';
import { AppRoutingModule } from './/app-routing.module';


import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { AuthResponseInterceptor} from './services/auth-response-interceptor';
import { StatisticComponent } from './statistic/statistic.component';
import { InventoriesComponent } from './inventories/inventories.component';




@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    EmployeesComponent,
    ContactsComponent,
    VenuesComponent,
    AppointmentsComponent,
    LoginComponent,
    RegisterComponent,
    StatisticComponent,
    InventoriesComponent

  ],
  imports: [
    BrowserModule, BrowserAnimationsModule,
    MenuModule, DataTableModule, SharedModule,
    ButtonModule, InputTextModule,
    FormsModule, InputTextareaModule,
    HttpModule, CalendarModule,
    PanelModule, InputSwitchModule,
    HttpClientModule, ConfirmDialogModule,
    DropdownModule, DialogModule,
    BreadcrumbModule, ReactiveFormsModule, AppRoutingModule,
    ChartModule, CheckboxModule

  ],
  providers: [AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthResponseInterceptor,
      multi: true
    }

  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
