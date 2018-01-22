import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  ButtonModule,
  InputTextModule,
  RadioButtonModule,
  CheckboxModule,
  ChartModule,
  PanelModule,
  TabViewModule,
  GrowlModule,
  FileUploadModule,
  TabMenuModule,
  MenuItem,
  InputMaskModule,
  MenuModule,
  GMapModule,
  DataTableModule,
  SharedModule,
  ListboxModule,
  InputSwitchModule,
  DropdownModule,
  SelectButtonModule,
  InputTextareaModule,
  CalendarModule,
  PaginatorModule,
  MenubarModule,
  BreadcrumbModule,
  DialogModule,
  ConfirmDialogModule,
  SidebarModule

} from 'primeng/primeng';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { VenuesComponent } from './venues/venues.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth-interceptor';
import { AuthResponseInterceptor } from './services/auth-response-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { HttpModule } from '@angular/http';
import { ContactsComponent } from './contacts/contacts.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RegisterComponent } from './user/register.component';
import { StatisticComponent } from './statistic/statistic.component';
import { LuigiComponent } from './luigi/luigi.component';
import { FlightsComponent } from './flights/flights.component';

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    AppointmentsComponent,
    VenuesComponent,
    ContactsComponent,
    DashboardComponent,
    LoginComponent,
    RegisterComponent,
    StatisticComponent,
    LuigiComponent,
    FlightsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    CalendarModule,
    InputTextModule,
    RadioButtonModule,
    CheckboxModule,
    AppRoutingModule,
    TabViewModule,
    GrowlModule,
    CommonModule,
    ChartModule,
    PaginatorModule,
    PanelModule,
    SelectButtonModule,
    InputTextareaModule,
    FileUploadModule,
    TabMenuModule,
    InputMaskModule,
    HttpModule,
    GMapModule,
    ButtonModule,
    MenuModule,
    DataTableModule,
    SharedModule,
    HttpClientModule,
    ListboxModule,
    DropdownModule,
    InputSwitchModule,
    ReactiveFormsModule,
    MenubarModule,
    BreadcrumbModule,
    DialogModule,
    ConfirmDialogModule
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
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
