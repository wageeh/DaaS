import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';

// Doctor Class
export interface Doctor {
  entityId: string;
  itemId:string;
  name: boolean;
  speciality:Speciality;
  description:string;
}

// Speciality Class
export interface Speciality{
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {

  private  doctorurl:string = 'doctors/'
  constructor(private coreService:CoreService) { }

  public async ListDoctors()
  {
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    var url:string = this.coreService.gatewayurl+this.doctorurl;
    
    const data = await this.coreService.http<Doctor[]>(
      new Request(
        url,
        {
          method: "get",
          headers:headers
        }
      )
    );

    return data;
  }

  public async GetDoctorById(doctorId:string)
  {
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    headers.append("Id", "patientId");
    
    var url:string = this.coreService.gatewayurl+this.doctorurl+"details/";
   
    const data = await this.coreService.http<Doctor[]>(
      new Request(
        url,
        {
          method: "get",
          headers:headers
        }
      )
    );

    return data;
  }
}
