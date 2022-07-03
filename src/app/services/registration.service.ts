import { Injectable } from '@angular/core';
import { UserRegisterRequest } from '../models/userRegisterRequest';
import {HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ReturnStatement } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  controllerUrl="User";
  headers= new HttpHeaders()
  .set('content-type', 'application/json');
  constructor() { }

  public async registerUser(regReq:UserRegisterRequest, http:HttpClient):Promise<any>
  {
    let serviceUrl="register";
    let body = JSON.stringify(regReq)
    const t = await http.post<string>(  `${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, 
                        body, 
                        {'headers':this.headers}).toPromise();
    
    return t;
  }
}
