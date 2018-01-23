import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { RouterModule, Routes} from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenubarModule, 
         DropdownModule, 
         GrowlModule, 
         DataTableModule, 
         SharedModule, 
         ButtonModule, 
         InputTextModule, 
         MenuModule, 
         MenuItem, 
         PanelModule, 
         CalendarModule, 
         InputSwitchModule, 
         Dropdown, 
         Sidebar, 
         Dialog, 
         BreadcrumbModule} from 'primeng/primeng';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ConfirmDialogModule, SidebarModule, DialogModule } from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { VenuesComponent } from './venues/venues.component';
import { ContactsComponent } from './contacts/contacts.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';

import { AuthService } from './services/auth.service';
import { AuthInterceptor} from './services/auth-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { StatisticComponent } from './statistic/statistic.component';
import { RegisterComponent } from './register/register.component';

import { RadioButtonModule, SelectButtonModule } from 'primeng/primeng';
import { ChartModule, InputMaskModule } from 'primeng/primeng';
import { TabViewModule, PaginatorModule } from 'primeng/primeng';
import { AppRoutingModule } from './app-routing.module';
import { AuthResponseInterceptor } from './services/auth-response-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    DashboardComponent,
    VenuesComponent,
    ContactsComponent,
    AppointmentsComponent,
    LoginComponent,
    StatisticComponent,
    RegisterComponent,
    StatisticComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ConfirmDialogModule,
    MenuModule,
    DataTableModule,
    DropdownModule,
    DialogModule,
    SharedModule,
    SidebarModule,
    ButtonModule,
    InputTextModule,
    BreadcrumbModule,
    CalendarModule,
    GrowlModule,
    HttpModule,
    FormsModule,
    InputSwitchModule,
    ReactiveFormsModule,
    PanelModule,
    HttpClientModule,
    MenubarModule,
    RadioButtonModule,
    SelectButtonModule,
    ChartModule,
    InputMaskModule,
    TabViewModule,
    PaginatorModule,
    AppRoutingModule
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
