import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from "./user/register.component";
import { ReservationsComponent } from "./reservations/reservations.component";
import { DepartmentsComponent } from "./departments/departments.component";
import { FlightsComponent } from "./flights/flights.component";
import { CanActivateRouteGuard } from "./services/can-activate-route.guard";

const routes: Routes = [
    { path: '', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'appointments', component: AppointmentsComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'contacts', component: ContactsComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'employees', component: EmployeesComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'venues', component: VenuesComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent },
    { path: 'reservations', component: ReservationsComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'departments', component: DepartmentsComponent, canActivate: [CanActivateRouteGuard] },
    { path: 'flights', component: FlightsComponent, canActivate: [CanActivateRouteGuard]}
  ];
 
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})

export class AppRoutingModule {}