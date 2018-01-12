import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './user/register.component';
 
const routes: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'appointments', component: AppointmentsComponent },
    { path: 'contacts', component: ContactsComponent },
    { path: 'employees', component: EmployeesComponent },
    { path: 'venues', component: VenuesComponent },
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent },
  ];
 
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})

export class AppRoutingModule {}