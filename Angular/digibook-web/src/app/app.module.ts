import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {ButtonModule,
  InputTextModule,
  RadioButtonModule,
  CheckboxModule,
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
  InputTextareaModule,
  CalendarModule,
  MenubarModule,
  BreadcrumbModule,
  DialogModule,
  ConfirmDialogModule,
  SidebarModule
  
} from 'primeng/primeng';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule, Routes} from '@angular/router';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { VenuesComponent } from './venues/venues.component';

import { HttpModule } from '@angular/http';
import { ContactsComponent } from './contacts/contacts.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const appRoutes: Routes = [
  {path: "", redirectTo: "/dashboard", pathMatch: "full"},
  {path: "dashboard", component: DashboardComponent},
  {path: "employees", component: EmployeesComponent},
  {path: "contacts", component: ContactsComponent},
  {path: "appointments", component: AppointmentsComponent},
  {path: "venues", component: VenuesComponent},
];

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    AppointmentsComponent,
    VenuesComponent,
    ContactsComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    CalendarModule,
    InputTextModule,
    RadioButtonModule,
    CheckboxModule,
    TabViewModule,
    GrowlModule,
    PanelModule,
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
    RouterModule.forRoot(appRoutes),
    DropdownModule,
    InputSwitchModule,
    ReactiveFormsModule,
    MenubarModule,
    BreadcrumbModule,
    DialogModule,
    ConfirmDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
