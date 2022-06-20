import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { EmailValidator } from '@angular/forms';

@Component({
  selector: 'app-loggin',
  templateUrl: './loggin.component.html',
  styleUrls: ['./loggin.component.css']
})
export class LogginComponent implements OnInit {
  @Input() public diplayHTMLContext;                            //Invoked by Root Component

  public email      ="";
  public password   ="";
  public re_password="";
  public username   ="";
  public name       ="";
  public surname    ="";
  public birthdate  ="";
  public address    ="";
  public type       ="";

  public errTxt="";

  constructor() { }
  ngOnInit(): void 
  {
    
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

  LoginFormTryLogin():void{
    
  }

  LoginFormTryRegister():void{
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
      
      this.errTxt="";
    }
    else
    {
      this.errTxt="Errors:"+errStr;
    }
  }

}
