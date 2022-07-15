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
  headers= new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  });

  constructor() { }

  public updateUser(updateReq:UserUpdateRequest, username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders()
    .set('content-type', 'application/json');

    let serviceUrl="update";
    let body = JSON.stringify(updateReq)

    return http.post<PrimitiveResponse>(`${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`, 
                        body, 
                        {'headers':this.headers});   
  }

  public getProfileData(username:string, http:HttpClient):Observable<number | User>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    let serviceUrl="profile";
    return http.get<User>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`); 
  }

  public getState(username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    let serviceUrl="profile";
    return http.get<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}/state`);  
  }

  public getAllUsers(http:HttpClient):Observable<number | Array<User>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    console.log(this.headers);
    return http.get<Array<User>>(  `${environment.apiUrl}/${this.controllerUrl}`,{headers:this.headers}); 
  }

  public userReject(username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    let serviceUrl="decline";
    return http.post<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`,
                                            "",
                                            {'headers':this.headers}); 
  }

  public userVerification(username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    let serviceUrl="pending";
    return http.post<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`,
                                            "",                                     
                                            {'headers':this.headers}); 
  }

  public userVerify(username:string, http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    });


    let serviceUrl="accept";
    return http.post<PrimitiveResponse>(  `${environment.apiUrl}/${this.controllerUrl}/${username}/${serviceUrl}`,
                                           "",                        
                                          {'headers':this.headers}); 
  }
}
