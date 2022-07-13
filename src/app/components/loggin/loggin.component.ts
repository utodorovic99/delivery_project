import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { EmailValidator } from '@angular/forms';
import {HttpClient, HttpEventType } from '@angular/common/http';
import { EventType } from '@angular/router';
import { RegistrationService } from '../../services/registration.service';
import * as shajs from 'sha.js';
import { UserRegisterRequest } from '../../models/userRegisterRequest';
import { MatDialog } from '@angular/material/dialog';
import { UserLoginRequest } from '../../models/userLoginRequest';

@Component({
  selector: 'app-loggin',
  templateUrl: './loggin.component.html',
  styleUrls: ['./loggin.component.css'],
  providers: [RegistrationService]
})
export class LogginComponent implements OnInit {

  @Input() public SPVisualContext;
  @Output() public SPVisualContext_changedEvent = new EventEmitter();

  public imgFile;

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
    this.setLoginEnvironment();
    this.setLoginContext();
    this.errTxt="";
  }

  SubmitLogin(Login:any)
  {
    let errStr="";
    this.errTxt="";
    if(this.email ==""     || this.password   =="")
      errStr+="\n\tEmpty fields detected.\n";
    else
    {
      if(this.password.length <8) errStr+="\n\tPassword must not be shorter than 8 characters";
      if(!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email))) errStr+="\n\tIvalid email format";           
    }
   
    if(errStr=="")
    {       
      let logReq = new UserLoginRequest(this.email, this.password);    

      this.registrationService.loginUser(logReq, this.http).subscribe(
        {
          
          next: (data)=>
          {
            this.errTxt="";
            if(data['value'] == null || data['value'] =="")
            {          
              this.errTxt="Errors: Login failed";
            }
            else
            {

              localStorage.setItem('token', JSON.parse(JSON.stringify(data)).value);
              localStorage.setItem('token_converted',  this.registrationService.getTokenJSON(localStorage.getItem('token')));
              let coverted = localStorage.getItem('token_converted');
              if (coverted==null) coverted ="";
              switch(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"])
              {
                case "Administrator": { this.setAdminEnvironment();      break;}
                case "Consumer":      { this.setConsumerEnvironment();   break;}
                case "Deliveryman":   { this.setDeliverymanEnvironment();break;}
              }
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
    else
      this.errTxt="Errors:"+errStr;
  }

  SubmitRegister(Register:any)
  {
    let errStr="";
    this.errTxt="";
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
      let regReq = new UserRegisterRequest(this.email, this.password, this.re_password, this.username, this.name, 
                                           this.surname, this.birthdate, this.address, this.type,this.selected_img);

      console.log(regReq);
      this.registrationService.registerUser(regReq, this.http).subscribe(
        {
          next: (data)=>
          {
            if(data ==null)
            {          
              this.setLoginContext();
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

  //#region UISetters 
  setLoginEnvironment()
  {
    this.SPVisualContext = 'Login';
    this.SPVisualContext_changedEvent.emit('Login');
  }

  setAdminEnvironment()
  {
    this.SPVisualContext = 'Administrator';
    this.SPVisualContext_changedEvent.emit('Administrator');
  }

  setDeliverymanEnvironment()
  {
    this.SPVisualContext = 'Deliveryman';
    this.SPVisualContext_changedEvent.emit('Deliveryman');
  }

  setConsumerEnvironment()
  {
    this.SPVisualContext = 'Consumer';
    this.SPVisualContext_changedEvent.emit('Consumer');
  }

  setSigninContext():void
  {
    this.email       =""; 
    this.password    ="";
    this.re_password ="";
    this.username    ="";
    this.name        ="";
    this.surname     ="";
    this.birthdate   ="";
    this.address     ="";
    this.type        ="";
    this.img_url='assets\\Images\\select_image.png';

    this.SPVisualContext='Register';
    this.errTxt="";
  }

  setLoginContext():void
  {
    //this.email       =""; //Keep entered value for login, restart others
    this.password    ="";
    this.re_password ="";
    this.username    ="";
    this.name        ="";
    this.surname     ="";
    this.birthdate   ="";
    this.address     ="";
    this.type        ="";
    this.img_url='assets\\Images\\select_image.png';

    this.SPVisualContext='Login';
    this.errTxt="";
  }
  //#endregion UISetters 
}
