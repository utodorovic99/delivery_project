import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { EmailValidator } from '@angular/forms';
import {HttpClient, HttpEventType } from '@angular/common/http';
import { EventType } from '@angular/router';
import { RegistrationService } from '../../services/registration.service';
import * as shajs from 'sha.js';
import { UserRegisterRequest } from '../../models/userRegisterRequest';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-loggin',
  templateUrl: './loggin.component.html',
  styleUrls: ['./loggin.component.css'],
  providers: [RegistrationService]
})
export class LogginComponent implements OnInit {
  @Input() public diplayHTMLContext;                            //Invoked by Root Component

  public email       ="";
  public password    ="";
  public re_password ="";
  public username    ="";
  public name        ="";
  public surname     ="";
  public birthdate   ="";
  public address     ="";
  public type        ="";
  public img_url='assets\\Images\\select_image.png';
  private _selected_img;
  public get selected_img(): File {
    return this._selected_img;
  }
  public set selected_img(value: File) {
    this._selected_img = value;
  }

  public errTxt="";

  constructor(private http: HttpClient, private registrationService: RegistrationService) { }
  ngOnInit(): void 
  {
    
  }

  SubmitLogin(Login:any)
  {
   
  }

  SubmitRegister(Register:any)
  {

    let errStr="";

    if(this.email ==""     || this.password   =="" || this.re_password=="" || this.surname    ==""||
       this.address    =="" || this.type       =="" || this.username   ==""|| this.name =="")
       errStr+="\n\tEmpty fields detected.\n";
    else if (this.birthdate  =="")
      errStr+="\n\tPlease select birthdate.\n";
    else
    {
      if(this.password.length <8) errStr+="\n\tPassword must not be shorter than 8 characters";
      else if(this.password!=this.re_password)errStr+="\n\tPassword mismatch";

      if(this.username.length <8) errStr+="\n\tUsername must not be shorter than 8 characters";
      if(this.address.length <8) errStr+="\n\tAddress must not be shorter than 8 characters";

      if(!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email))) errStr+="\n\tIvalid email format";
            
    }

    if(errStr=="")  //Try register on backend and return error
    {
      let regReq = new UserRegisterRequest()
      regReq.email      =this.email;
      regReq.password   = this.password;
      regReq.re_password =this.re_password;
      regReq.username    =this.username;
      regReq.name        =this.name;
      regReq.surname     =this.surname;
      regReq.birthdate   =this.birthdate;
      regReq.address     =this.address;
      regReq.type        =this.type;
      regReq.Img        =this.selected_img;

      let res=-1;
      this.registrationService.registerUser(regReq, this.http);

      if(res==0) this.diplayHTMLContext=0;
    }
    else
    {
      this.errTxt="Errors:"+errStr;
    }
  }

  onImageSelected(event)
  {
    this.selected_img=<File>event.target.files[0];
    var reader = new FileReader();
    reader.onload=(event:any)=>
      {this.img_url = event.target.result;}
    
      reader.readAsDataURL(this.selected_img);
  }

  setSigninContext():void
  {
    this.diplayHTMLContext=1;
    this.errTxt="";
  }

  setLoginContext():void
  {
    this.diplayHTMLContext=0;
    this.errTxt="";
  }
}
