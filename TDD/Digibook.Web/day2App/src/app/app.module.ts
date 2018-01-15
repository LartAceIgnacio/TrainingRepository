import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { RouterModule, Routes} from '@angular/router';
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

const appRoutes: Routes = [
  {path: "", redirectTo: "/dashboard", pathMatch: "full"},
  {path: "dashboard", component: DashboardComponent},
  {path: "employees", component: EmployeesComponent},
  {path: "contacts", component: ContactsComponent},
  {path: "appointments", component: AppointmentsComponent},
  {path: "venues", component: VenuesComponent},
  {path: "login", component: LoginComponent},
  {path: "register", component: RegisterComponent}
];

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
    RouterModule.forRoot(appRoutes)
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
