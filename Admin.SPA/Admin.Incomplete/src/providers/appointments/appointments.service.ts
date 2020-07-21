import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';



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
  
  private  appointmenturl:string = 'appointments/appointmentsfilter'
  data: any;
  constructor(private coreService:CoreService) { }

  public ListAppointments()
  {
    var url:string = this.coreService.gatewayurl+this.appointmenturl;
    this.coreService.httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Credentials': 'true',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Accept': 'application/json',
        "Content-Type": "application/json",
        'Ocp-Apim-Subscription-Key':'2e97c2daa919495786b9d89b948aefeb',
        'Ocp-Apim-Trace': 'true'
      })
    };
    var postdata={
      fromdate:"",
      todate:""
    };
    this.coreService.postData(url,postdata)
    .subscribe(resp => {
      return this.data.push(resp);
    });
  }
}
