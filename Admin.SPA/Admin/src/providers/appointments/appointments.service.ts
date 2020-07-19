import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';



// Vehicle Status Class
export interface Appointment {
  PatientId: string;
  DoctorId: string;
  AppointmentDate: string;
  appointmentTimeSlot:number;
  HasConflict: Boolean;
  CreatedDate: string;
  id: string;
}

@Injectable({
  providedIn: 'root'
})

export class AppointmentsService {
  
  private  appointmenturl:string = 'appointmentsfilter'
  constructor(private coreService:CoreService) { }

  public async ListAppointments()
  {
    const headers = new Headers();
    
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");

    const data = await this.coreService.http<Appointment[]>(
      new Request(
        this.coreService.gatewayurl+this.appointmenturl,
        {
          method: "post",
          body: JSON.stringify({
            Minutes: 1
          }),
          headers:headers
        }
      )
    );

    return data;
  }
}
