import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// tslint:disable-next-line:max-line-length
import { MenuModule, DataTableModule, SharedModule, ButtonModule, InputTextModule, PanelModule, DialogModule, InputTextareaModule, CalendarModule, ToggleButtonModule, DropdownModule, BreadcrumbModule, ConfirmDialogModule, SelectButtonModule, ChartModule } from 'primeng/primeng';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { Route } from '@angular/compiler/src/core';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { AppRoutingModule } from './/app-routing.module';
import { LoginComponent } from './login/login.component';

import { AuthService } from './services/auth.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth-interceptor';
import { RegisterComponent } from './register/register.component';

import { AuthResponseInterceptor} from './services/auth-response-interceptor';
import { StatisticComponent } from './statistic/statistic.component';
import { PilotsComponent } from './pilots/pilots.component';



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
    PilotsComponent,
  ],
  imports: [
    BrowserModule,
    MenuModule,
    BrowserAnimationsModule,
    SharedModule,
    InputTextModule,
    ButtonModule,
    HttpModule,
    FormsModule,
    DataTableModule,
    PanelModule,
    HttpClientModule,
    DialogModule,
    InputTextareaModule,
    CalendarModule,
    ToggleButtonModule,
    DropdownModule,
    AppRoutingModule,
    BreadcrumbModule,
    ConfirmDialogModule,
    ReactiveFormsModule,
    SelectButtonModule,
    ChartModule

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
