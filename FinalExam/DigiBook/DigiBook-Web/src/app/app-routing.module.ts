import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContacsComponent } from './contacs/contacs.component';
import { VenuesComponent } from './venues/venues.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';
import { UserComponent } from './user/user.component';
import { PilotsComponent } from './pilots/pilots.component';

const appRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent},
    { path: 'employees', component: EmployeesComponent},
    { path: 'contacts', component: ContacsComponent},
    { path: 'appointments', component: AppointmentsComponent},
    { path: 'venues', component: VenuesComponent},
    { path: 'login', component: LoginComponent},
    { path: 'user', component: UserComponent },
    { path: 'pilot', component: PilotsComponent },
    { path: '', redirectTo: '/dashboard', pathMatch:'full'},
    { path: '**', redirectTo: '/dashboard', pathMatch:'full'}
  ]
 
@NgModule({
  imports: [ RouterModule.forRoot(appRoutes) ],
  exports: [ RouterModule ]
})

export class AppRoutingModule {}