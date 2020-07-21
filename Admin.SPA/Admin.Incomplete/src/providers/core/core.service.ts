import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CoreService {
  public gatewayurl:string = "https://daas.azure-api.net/";
  constructor(private http: HttpClient) { }
  public httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      Accept: 'application/json'
    })
  };
 
  private result:any;
  async getData(url){
    // this.presentLoading();
    this.http.get<any>('https://daas-usermanagementapi.azurewebsites.net/api/patient/').subscribe(result => {
     return result;
    }, error => console.error(error));
    //return this.result;
   
  }

  // HttpClient API Post() data
  postData(url, data): Observable<any> {
    return this.http.post<any>(url, data, this.httpOptions).pipe(
      // catchError(this.handleError)
    );
  }

  async handleError(data) {
    console.log('will handel error');
    console.log(data);

    let errorMessage = '';
    if (data.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = data.error.message;
    } else  if (data.status === 422 && data.error.errors) {
      Object.keys(data.error.errors).forEach((key: any) => {
        errorMessage = `Input: <strong> ${key} </strong>
        <p> Message: <strong>${data.error.errors[key]}</strong></p>`;
      });
    } else if (data.status === 400  && data.error.errors) {
      Object.keys(data.error.errors).forEach((key: any) => {
        errorMessage = `Input: <strong> ${key} </strong>
        <p> Message: <strong>${data.error.errors[key]}</strong></p>`;
      });
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${data.status}\nMessage: ${data.message}`;
    }
    // this.dismissLoading();
    // window.alert(errorMessage);
  // await this.presentAlert('Error' , errorMessage)


    return throwError(errorMessage);
 }
}
