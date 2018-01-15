import { BrowserModule } from '@angular/platform-browser';
import { NgModule} from '@angular/core';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeeComponent } from './employee/employee.component';
import { ContactComponent } from './contact/contact.component';
import { VenueComponent } from './venue/venue.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { StatisticComponent } from './statistic/statistic.component';

import { RouterModule, Routes} from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule, } from '@angular/forms';
import { HttpModule } from '@angular/http';

import {HttpClientModule} from '@angular/common/http';

import { AuthService } from './services/auth.service';
import { AuthInterceptor} from './services/auth-interceptor';
import { AuthResponseInterceptor} from './services/auth-response-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import {PanelModule, MenuModule, InputTextModule, 
        DataTableModule,SharedModule, ButtonModule,
        InputMaskModule, DropdownModule, CalendarModule,
        InputSwitchModule, GrowlModule, DialogModule, 
        BreadcrumbModule, ChartModule} from 'primeng/primeng';

const appRoutes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'employee', component: EmployeeComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'venue', component: VenueComponent },
  { path: 'appointment', component: AppointmentComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  
];

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    EmployeeComponent,
    ContactComponent,
    VenueComponent,
    AppointmentComponent,
    LoginComponent,
    RegisterComponent,
    StatisticComponent
  ],
  imports: [
    BrowserModule,

    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule,
    FormsModule, ReactiveFormsModule,
    HttpModule,
    HttpClientModule,

    PanelModule, MenuModule, InputTextModule,
    DataTableModule, SharedModule, ButtonModule,
    InputMaskModule, DropdownModule, CalendarModule,
    InputSwitchModule, GrowlModule, DialogModule,
    BreadcrumbModule, ChartModule
  ],
  providers: [
    AuthService,
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