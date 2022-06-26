import { Injectable } from '@angular/core';
import { User } from '../models/user';
import {HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  url="User";
  //_headers =new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8').set('accept', 'text/plain');
  constructor() { }

  public registerUser(usr:User, http:HttpClient):any
  {
    console.log("Sending register user request");
    http.post<string>(`${environment.apiUrl}/${this.url}/register`, usr).subscribe(data=>
    {
      if(data == "") console.log("Register succeeded");
      else           console.log("Register failed with: "+data);
    });
  }
}
