import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserService } from '../../services/user.service';
import {HttpClient, HttpEventType } from '@angular/common/http';
import { User } from '../../models/user';
import { UserUpdateRequest } from '../../models/userUpdateRequest';
import { PrimitiveResponse } from '../../models/primitiveResponse';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  @Output() public SPVisualContext_changedEvent = new EventEmitter();
  public email       ="";
  public password    ="";
  public new_password="";
  public re_new_password ="";
  public username    ="";
  public name        ="";
  public surname     ="";
  public birthdate   ="";
  public address     ="";
  public type        ="";
  public img_url='assets\\Images\\select_image.png';
  public errTxt="";
  public dissabledFlag;
  
  constructor(private http: HttpClient, private userService:UserService) { }
  ngOnInit(): void {

    this.dissabledFlag=true;
    this.loadData();
  }

  reloadData()
  {
    this.dissabledFlag = false;
    this.loadData();
    this.password    ="";
    this.new_password="";
    this.re_new_password ="";
    this.errTxt="";
    this.dissabledFlag = true;
  }

  updateData()
  {
    let updateReq = new UserUpdateRequest(this.email, this.name, this.surname, this.birthdate, this.address,this.password, this.new_password, "");
    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted =""; 
    if(coverted != null)
    {     
      this.userService.updateUser( updateReq,  JSON.parse(coverted)['username'], this.http).subscribe(
      {
        
        next: (data)=>
        {
          let dataStr = data['value'];
          if(dataStr.charAt(dataStr.length-1)=='T')  
          {  
            localStorage.clear();
            this.SPVisualContext_changedEvent.emit('Login');
            return;
          }
          dataStr=dataStr.substring(0, dataStr.length-1);

          this.reloadData(); 
          this.dissabledFlag = true;
          if(dataStr != undefined) this.errTxt = dataStr;
          else this.errTxt="";
        },

        error: (error)=>
        {
          console.log('Error happened: ');
          console.log(error);
          if(error.status === 400)
              this.errTxt="Errors:"+error.error;
            else if (error.status === 503)
              this.errTxt="Errors: Service is offline";
        }
      }); 
    }
  }

  editData()
  {
    this.dissabledFlag = false;
  }

  loadData()
  {
    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted =""; 
    if(coverted != null)
    {
      this.userService.getProfileData( JSON.parse(coverted)['username'], this.http).subscribe(
      {
        
        next: (data)=>
        {
          this.errTxt="";
          this.email       =(data as User)['email'];
          this.username    =(data as User)['username'];
          this.name        =(data as User)['name'];
          this.surname     =(data as User)['surname'];
          this.birthdate   =(data as User)['birthdate'];
          this.address     =(data as User)['address'];
          this.type        =(data as User)['type'];
          if((data as User).ImageRaw  == undefined)
          {
            this.img_url='assets\\Images\\select_image.png';
          }
          
        },

        error: (error)=>
        {
          if(error.status === 400)
              this.errTxt="Errors:"+error.error;
            else if (error.status === 503)
              this.errTxt="Errors: Service is offline";
        }
      }); 
    }
  }
}
