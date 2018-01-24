import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { ContactsComponent } from './contacts/contacts.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './user/register.component';
import { AirportComponent } from './airport/airport.component';



const routes: Routes = [
  // { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  // { path: 'dashboard', component: DashboardComponent },
  // { path: 'employees', component: EmployeesComponent },
  // { path: 'contacts', component: ContactsComponent },
  // { path: 'appointments', component: AppointmentsComponent },
  //{ path: 'login', component: LoginComponent},
  //{ path: 'register', component: RegisterComponent },
  // { path: 'airport', component: AirportComponent }

  //activity may removed this
  { path: '', redirectTo: '/airport', pathMatch: 'full' },
  { path: 'airport', component: AirportComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }