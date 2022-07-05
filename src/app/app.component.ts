import { Component, Input, OnInit } from '@angular/core';
import { RegistrationService } from './services/registration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Delivery Manager';
  public SPVisualState = 0;
  public diplayLoginHTMLContext=0;

  constructor(private registrationService: RegistrationService) 
  { 
    let role=-1;
    switch(this.registrationService.getUserRole(localStorage.getItem('token')))
    {
      case "Administrator": { role=1; break;}
      case "Consumer":      { role=3; break;}
      case "Deliveryman":   { role=2; break;}     
    }
    if(role==-1) role=0;
    this.SPVisualState=role;
  }

  OnSPVisualState_changed($event)
  {
    console.log('Changing to:' + $event)
    this.SPVisualState =$event;
  }
}
