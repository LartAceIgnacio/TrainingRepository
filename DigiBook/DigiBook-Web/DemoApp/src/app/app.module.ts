import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MenuModule, PanelModule, TabViewModule, DataTableModule, SharedModule, DialogModule, InputTextModule, ButtonModule,
         MenuItem, TreeTableModule, TreeNode, DropdownModule, ToggleButtonModule, GrowlModule, InputSwitchModule, CalendarModule,
         MenubarModule, BreadcrumbModule, ConfirmDialogModule, ConfirmationService, ChartModule, PaginatorModule } from "primeng/primeng";
import { FormsModule } from '@angular/forms';
import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule} from '@angular/forms';
import { RouterModule, Routes} from '@angular/router';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { AuthService } from './services/auth.service';
import { AuthInterceptor} from './services/auth-interceptor';
import { AuthResponseInterceptor} from './services/auth-response-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { EmployeesComponent } from './employees/employees.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TreeviewComponent } from './treeview/treeview.component';
import { ContacsComponent } from './contacs/contacs.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { VenuesComponent } from './venues/venues.component';
import { LoginComponent } from './login/login.component';
import { UserComponent } from './user/user.component';
import { RegisterComponent } from './register/register.component';
import { StatisticsComponent } from './statistics/statistics.component';

const appRoutes: Routes = [
  { path: 'dashboard', component: DashboardComponent},
  { path: 'employees', component: EmployeesComponent},
  { path: 'contacts', component: ContacsComponent},
  { path: 'appointments', component: AppointmentsComponent},
  { path: 'venues', component: VenuesComponent},
  { path: 'treeview', component: TreeviewComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent },
  { path: 'user', component: UserComponent },
  { path: '', redirectTo: '/dashboard', pathMatch:'full'},
  { path: '**', redirectTo: '/dashboard', pathMatch:'full'}
]

@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    DashboardComponent,
    TreeviewComponent,
    ContacsComponent,
    AppointmentsComponent,
    VenuesComponent,
    LoginComponent,
    RegisterComponent,
    UserComponent,
    StatisticsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    PanelModule,
    MenuModule,
    TabViewModule,
    DataTableModule,
    SharedModule,
    InputTextModule,
    ButtonModule,
    DialogModule,
    TreeTableModule,
    HttpClientModule,
    DropdownModule,
    ToggleButtonModule,
    GrowlModule,
    ReactiveFormsModule,
    InputSwitchModule,
    CalendarModule,
    MenubarModule,
    BreadcrumbModule,
    ConfirmDialogModule,
    ChartModule,
    PaginatorModule, 
    
    RouterModule.forRoot(appRoutes)
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
