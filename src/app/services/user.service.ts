import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import {PrimitiveResponse} from '../models/primitiveResponse'
import { UserUpdateRequest } from '../models/userUpdateRequest';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  controllerUrl="User";
  headers= new HttpHeaders()
  .set('content-type', 'application/json');

  constructor() { }

  public updateUser(updateReq:UserUpdateRequest, username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    let serviceUrl="update";
    let body = JSON.stringify(updateReq)

    return http.post<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`, 
                        body, 
                        {'headers':this.headers});   
  }

  public getProfileData(username:string, http:HttpClient):Observable<number | User>
  {
    let serviceUrl="profile";
    return http.get<User>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`); 
  }
}
