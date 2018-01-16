import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { MessagesModule, TabMenuModule } from 'primeng/primeng';
import { MessageModule } from 'primeng/primeng';
import { GrowlModule } from 'primeng/primeng';
// validation
import { Validators, FormControl, FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';

// Primeng
import { FormsModule } from '@angular/forms';
import { MenuModule, DataTableModule, SharedModule, ButtonModule, InputTextModule, PanelModule, DropdownModule } from 'primeng/primeng';
import { CalendarModule } from 'primeng/primeng';
import { DialogModule } from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmployeeComponent } from './employee/employee.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AppRoutingModule } from './/app-routing.module';
import { ContactComponent } from './contact/contact.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { VenueComponent } from './venue/venue.component';
import { LoginComponent } from './login/login.component';
import { SampleValidationComponent } from './sample-validation/sample-validation.component';


//Authentication

import 'rxjs/add/operator/toPromise';

import { AuthService } from './services/common/Authentication/auth.service';
import { AuthInterceptor } from './services/common/Authentication/auth-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { StatisticComponent } from './statistic/statistic.component';
import { RegisterComponent } from './register/register.component';
import { InputMaskModule } from 'primeng/components/inputmask/inputmask';
import { SelectButtonModule } from 'primeng/components/selectbutton/selectbutton';
import { ConfirmDialogModule } from 'primeng/components/confirmdialog/confirmdialog';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/components/chart/chart';
import { AuthResponseInterceptor } from './services/common/Authentication/auth-response-interceptor';
import { AboutComponent } from './about/about.component';

@NgModule({
  declarations: [
    AppComponent,
    EmployeeComponent,
    DashboardComponent,
    ContactComponent,
    AppointmentComponent,
    VenueComponent,
    SampleValidationComponent,
    LoginComponent,
    StatisticComponent,
    RegisterComponent,
    AboutComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MenuModule,
    DataTableModule,
    SharedModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    HttpModule,
    PanelModule,
    HttpClientModule,
    DropdownModule,
    CalendarModule,
    MessagesModule,
    MessageModule,
    GrowlModule,
    ReactiveFormsModule,
    TabMenuModule,
    DialogModule,
    InputTextModule,
    InputMaskModule, 
    SelectButtonModule,
    DataTableModule, 
    SharedModule,
    DialogModule,
    HttpModule,
    MenuModule,
    ConfirmDialogModule,
    CommonModule,
    DropdownModule,
    ChartModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [AuthService,
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
export class AppModule { }
