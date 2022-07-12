import { Injectable } from '@angular/core';
import { UserRegisterRequest } from '../models/userRegisterRequest';
import { UserLoginRequest } from '../models/userLoginRequest';
import {PrimitiveResponse}from '../models/primitiveResponse';
import {HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AppComponent } from '../app.component';
import { Observable } from 'rxjs';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  controllerUrl="User";
  headers= new HttpHeaders()
  .set('content-type', 'application/json');
  constructor() { }

  public registerUser(regReq:UserRegisterRequest, http:HttpClient):Observable<number | string>
  {
    let serviceUrl="register";
    let body = JSON.stringify(regReq);

    const fd = new FormData();
    fd.append('email',regReq.Email);
    fd.append('password',regReq.Password);
    fd.append('username',regReq.Username);
    fd.append('name',regReq.Name);
    fd.append('surname',regReq.Surname);
    fd.append('birthdate', regReq.Birthdate);
    fd.append('address',regReq.Address);
    fd.append('type',regReq.Type);
    fd.append('imageRaw', regReq.ImageRaw, regReq.ImageRaw.name);

    return http.post<string>(  `${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, 
                        fd);   
  }

  public loginUser(logReq:UserLoginRequest, http:HttpClient):Observable<PrimitiveResponse>
  {
    let serviceUrl="login";
    let body = JSON.stringify(logReq)

    return http.post<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, 
                        body, 
                        {'headers':this.headers});   
  }

  public getTokenJSON(rawToken:string | null):string
  {
    if(rawToken === null) return "";
    try {
      
      return JSON.stringify(jwt_decode(rawToken as string));
    } catch(Error) {
      return "";
    }
  }
}
