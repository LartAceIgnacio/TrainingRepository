import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {RouterModule, Routes} from '@angular/router';
import { FormsModule }   from '@angular/forms';
import {HttpModule} from '@angular/http';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {ReactiveFormsModule} from '@angular/forms';

import {MenuModule, PanelModule} from 'primeng/primeng';
import {TabViewModule} from 'primeng/primeng';
import {DataTableModule,SharedModule,ButtonModule,InputTextModule} from 'primeng/primeng';
import {DropdownModule} from 'primeng/primeng';
import {InputTextareaModule} from 'primeng/primeng';
import {SplitButtonModule} from 'primeng/primeng';
import {CalendarModule} from 'primeng/primeng';
import {AccordionModule} from 'primeng/primeng';
import {InputSwitchModule} from 'primeng/primeng';
import {BreadcrumbModule,MenuItem} from 'primeng/primeng';
import {DialogModule} from 'primeng/primeng';
import {ConfirmDialogModule,ConfirmationService} from 'primeng/primeng';
import {ChartModule} from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { InputsComponent } from './inputs/inputs.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactsComponent } from './contacts/contacts.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { VenuesComponent } from './venues/venues.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from "./services/auth.service";
import { AuthInterceptor } from "./services/auth-interceptor";
import { StatisticComponent } from './statistic/statistic.component';
import { RegisterComponent } from './user/register.component';
import { AuthResponseInterceptor } from "./services/auth-response-interceptor";

const appRoutes: Routes =[
  {path:"", redirectTo:"/dashboard", pathMatch:"full"},
  {path:"dashboard", component:DashboardComponent},
  {path:"employees", component:EmployeesComponent},
  {path:"contacts", component:ContactsComponent},
  {path:"appointments", component:AppointmentsComponent},
  {path:"venues", component:VenuesComponent},
  {path:"login", component:LoginComponent},
  {path:"register", component:RegisterComponent}
]

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    InputsComponent,
    DashboardComponent,
    ContactsComponent,
    AppointmentsComponent,
    VenuesComponent,
    LoginComponent,
    StatisticComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    MenuModule,
    PanelModule,
    BrowserAnimationsModule,
    TabViewModule,
    DataTableModule,
    SharedModule,
    DropdownModule,
    FormsModule,
    InputTextareaModule,
    RouterModule.forRoot(appRoutes),
    ButtonModule,
    InputTextModule,
    HttpModule,
    FormsModule,
    SplitButtonModule,
    HttpClientModule,
    CalendarModule,
    ReactiveFormsModule,
    AccordionModule,
    InputSwitchModule,
    BreadcrumbModule,
    DialogModule,
    ConfirmDialogModule,
    ChartModule
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
export class AppModule {
 }

 