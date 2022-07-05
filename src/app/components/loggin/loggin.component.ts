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
  @Input() public diplayHTMLContext;                            //Invoked by Root Component
  @Input() public SPVisualState;
  @Output() public SPVisualState_changedEvent = new EventEmitter();

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
            if(data['value'] == null || data['value'] =="")
            {          
              this.errTxt="Errors: Login failed";
            }
            else
            {

              localStorage.setItem('token', JSON.parse(JSON.stringify(data)).value);
              switch(this.registrationService.getUserRole(localStorage.getItem('token')))
              {
                case "Administrator": { console.log('HERE');this.setAdminEnvironment();      break;}
                case "Consumer":      { console.log('HERE');this.setConsumerEnvironment();   break;}
                case "Deliveryman":   { console.log('HERE');this.setDeliverymanEnvironment();break;}
              }
            }
          },

          error: (error)=>
          {
            console.log('Error happened');
            console.log(error);
            //if(error.status === 400)
            //  this.errTxt="Errors:"+error.error;
           // else if (error.status === 503)
              //this.errTxt="Errors: Service is offline";
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
    this.SPVisualState = 0;
    this.SPVisualState_changedEvent.emit(0);
  }

  setAdminEnvironment()
  {
    this.SPVisualState = 1;
    this.SPVisualState_changedEvent.emit(1);
  }

  setDeliverymanEnvironment()
  {
    this.SPVisualState = 2;
    this.SPVisualState_changedEvent.emit(2);
  }

  setConsumerEnvironment()
  {
    this.SPVisualState = 3;
    this.SPVisualState_changedEvent.emit(3);
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
  //#endregion UISetters 
}
