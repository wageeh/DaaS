import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

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

    var url:string = this.coreService.gatewayurl+this.doctorurl;
    
    this.coreService.httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Credentials': 'true',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Accept': 'application/json',
        "Content-Type": "application/json",
        'Ocp-Apim-Trace': 'true'
      })
    };
    
    const data = await this.coreService.getData(url);
    return data;
  }

  public async GetDoctorById(doctorId:string)
  {
    
    var url:string = this.coreService.gatewayurl+this.doctorurl+"details/";
   
    this.coreService.httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Credentials': 'true',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Accept': 'application/json',
        "Content-Type": "application/json",
        "Id": doctorId,
        'Ocp-Apim-Trace': 'true'

      })
    };

    const data = this.coreService.getData(url);
    return data;
  }
}
