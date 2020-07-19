import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// for importing services
import { CoreService } from '../providers/core/core.service';
import { PatientsService } from '../providers/patients/patients.service';
import { DoctorsService } from '../providers/doctors/doctors.service';
import { AppointmentsService } from '../providers/appointments/appointments.service';
import { DashboardComponent } from '../pages/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [
    CoreService,
    PatientsService, 
    DoctorsService,
    AppointmentsService
  ],
  bootstrap: [AppComponent],
  entryComponents: [DashboardComponent]
})
export class AppModule { }
