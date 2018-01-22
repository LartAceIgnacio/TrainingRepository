import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ButtonModule, RadioButtonModule, SelectButtonModule, MenuModule, BreadcrumbModule } from 'primeng/primeng';
import { DataTableModule, SharedModule, DialogModule, ConfirmDialogModule } from 'primeng/primeng';
import { DropdownModule, ChartModule, InputTextModule, InputMaskModule } from 'primeng/primeng';
import { PanelModule, TabViewModule, CalendarModule, PaginatorModule } from 'primeng/primeng';
import { GrowlModule } from "primeng/components/growl/growl";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import 'rxjs/add/operator/toPromise';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';

import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth-interceptor';
import { AuthResponseInterceptor } from './services/auth-response-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegisterComponent } from './registers/register.component';

import { StatisticComponent } from './statistic/statistic.component';

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
    StatisticComponent
  ],
  imports: [
    BrowserModule,
    PaginatorModule,
    PanelModule,
    TabViewModule,
    BrowserAnimationsModule,
    CalendarModule,
    FormsModule,
    GrowlModule,
    RadioButtonModule,
    ButtonModule,
    InputTextModule,
    InputMaskModule,
    SelectButtonModule,
    DataTableModule,
    SharedModule,
    DialogModule,
    HttpModule,
    MenuModule,
    ConfirmDialogModule,
    CommonModule,
    DropdownModule,
    ChartModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    BreadcrumbModule

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