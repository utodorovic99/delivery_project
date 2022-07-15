import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserService } from '../../services/user.service';
import {HttpClient, HttpEventType } from '@angular/common/http';
import { User } from '../../models/user';
import { UserUpdateRequest } from '../../models/userUpdateRequest';
import { PrimitiveResponse } from '../../models/primitiveResponse';
import { DomSanitizer } from '@angular/platform-browser';

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
  public state = "";
  public typeIdx = 1;
  public img_url='assets\\Images\\select_image.png';
  public errTxt="";
  public dissabledFlag;
  
  constructor(private http: HttpClient, private userService:UserService, private sanitizer: DomSanitizer) { }
  ngOnInit(): void {
    this.errTxt="";
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
    let updateReq = new UserUpdateRequest(this.email, this.password, this.username,this.name, this.surname, this.birthdate, this.address, this.type,this.new_password);
    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted =""; 
    if(coverted != null)
    {     
      console.log(updateReq);
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
          this.type        = this.mapTypeValueToStr((data as User)['type']);
          this.typeIdx     = this.mapTypeToIndex((data as User)['type']);
          this.state       = this.mapStateCodeToStr((data as User)['state']);

          var url;
          var urlPass;
          if((data as User)['imageRaw']!=='AA==' && (data as User)['imageRaw'].length>0)
          {
            url = 'data:image/png;base64,' + (data as User)['imageRaw'];
            urlPass= this.sanitizer.bypassSecurityTrustUrl(url);
          }
          else urlPass = 'assets\\Images\\select_image.png'
          
          this.img_url= urlPass ;
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

  private mapStateCodeToStr(code:number):string
  {
    switch(code)
    {
      case 0: {return "unconfirmed";}
      case 1: {return "confirmed";}
      case 2: {return "rejected";}
      case 3: {return "pending";}
    }
    return "-";
  }

  private mapTypeValueToStr(type:string)
  {
    switch(type)
    {
      case "administrator": { return "admin";}
      case "deliveryman":   { return "delivery";}
      case "consumer":      { return "consumer";}
      default:              {return "unknown";}
    }
  }

  private mapTypeToIndex(type:string):number
  {
    switch(type)
    {
      case "administrator": { return 0;}
      case "deliveryman":   { return 1;}
      case "consumer":      { return 2;}
      default:              {return -1;}
    }
  }
}
