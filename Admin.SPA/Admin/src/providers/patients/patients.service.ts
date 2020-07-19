import { CoreService } from '../core/core.service';
import { Injectable } from '@angular/core';



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

  private  patienturl:string = 'patients/'
  constructor(private coreService:CoreService) { }

  public async ListPatients()
  {
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    
    
    var url = this.coreService.gatewayurl+this.patienturl;
    const data = await this.coreService.http<Patient[]>(
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

  public async GetPatientById(patientId:string)
  {
    const headers = new Headers();
    headers.append('Access-Control-Allow-Credentials', 'true');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    headers.append("Id", "patientId");
    
    var url = this.coreService.gatewayurl+this.patienturl+"details/";
    const data = await this.coreService.http<Patient[]>(
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
