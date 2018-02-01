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
import { CanActivateRouteGuard } from './services/can-activate-route.guard';

const appRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent, canActivate: [CanActivateRouteGuard]},
    { path: 'employees', component: EmployeesComponent, canActivate: [CanActivateRouteGuard]},
    { path: 'contacts', component: ContacsComponent, canActivate: [CanActivateRouteGuard]},
    { path: 'appointments', component: AppointmentsComponent, canActivate: [CanActivateRouteGuard]},
    { path: 'venues', component: VenuesComponent, canActivate: [CanActivateRouteGuard]},
    { path: 'login', component: LoginComponent},
    { path: 'register', component: UserComponent },
    { path: 'pilot', component: PilotsComponent , canActivate: [CanActivateRouteGuard]},
    { path: '', redirectTo: '/login', pathMatch:'full'},
    { path: '**', redirectTo: '/login', pathMatch:'full'}
  ]
 
@NgModule({
  imports: [ RouterModule.forRoot(appRoutes) ],
  exports: [ RouterModule ]
})

export class AppRoutingModule {}