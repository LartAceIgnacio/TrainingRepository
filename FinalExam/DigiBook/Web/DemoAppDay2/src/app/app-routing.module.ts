import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

//Components
import { AppComponent }      from './app.component';
import { EmployeeComponent }      from './employee/employee.component';
import { DashboardComponent }      from './dashboard/dashboard.component';
import { ContactComponent } from './contact/contact.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { VenueComponent } from './venue/venue.component';
import { SampleValidationComponent } from './sample-validation/sample-validation.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AboutComponent } from './about/about.component';
import { PilotComponent } from './pilot/pilot.component';

//Routes Settings
const routes: Routes = [
  { path: 'home', component: AppComponent },
  { path: 'employee', component: EmployeeComponent },
	{ path: 'dashboard', component: DashboardComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'appointment', component: AppointmentComponent },
  { path: 'venue', component: VenueComponent },
  { path: 'validation', component: SampleValidationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component:  RegisterComponent},
  { path: 'about', component:  AboutComponent},
  { path: 'pilot', component: PilotComponent }
  
];
     
@NgModule({
 exports: [ RouterModule ],
 imports: [ RouterModule.forRoot(routes) ] // imports router module
})

export class AppRoutingModule {}
