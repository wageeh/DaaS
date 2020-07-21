import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';



// Patient Class
export interface Patient {
  email: string;
  name: string;
  description: string;
  address: {
    zipCode: string;
    entityId: string;
    itemId: string;
    createdDate: string;
  },
  entityId: string;
  itemId: string;
  createdDate: string;
}

@Injectable({
  providedIn: 'root'
})

export class PatientsService {

  private  patienturl:string = 'patients/';
  result: any;
  constructor(private coreService:CoreService) { }

  public async ListPatients()
  {
    
    var url = this.coreService.gatewayurl+this.patienturl;

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
    
    this.result = await this.coreService.getData(url);
    return this.result;

  }

  public async GetPatientById(patientId:string)
  {
    /*const headers = new Headers();
    headers.append('Access-Control-Allow-Credentials', 'true');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    headers.append("Id", patientId);*/
    
    var url = this.coreService.gatewayurl+this.patienturl+"details/";

    this.coreService.httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Credentials': 'true',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Accept': 'application/json',
        "Content-Type": "application/json",
        "Id": patientId,
        'Ocp-Apim-Trace': 'true'

      })
    };

    const data = await this.coreService.getData(url);
    return data;
  }
}
