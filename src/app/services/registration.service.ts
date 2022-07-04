import { Injectable } from '@angular/core';
import { UserRegisterRequest } from '../models/userRegisterRequest';
import {HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AppComponent } from '../app.component';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  controllerUrl="User";
  headers= new HttpHeaders()
  .set('content-type', 'application/json');
  constructor() { }

  public  registerUser(regReq:UserRegisterRequest, http:HttpClient):Observable<number | string>
  {
    let serviceUrl="register";
    let body = JSON.stringify(regReq)

    return http.post<string>(  `${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, 
                        body, 
                        {'headers':this.headers});   
  }
}
