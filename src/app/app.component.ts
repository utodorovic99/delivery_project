import { Component, Input, OnInit, Output } from '@angular/core';
import { RegistrationService } from './services/registration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Delivery Manager';
  @Output() public SPVisualContext ;
  @Output() public SPVisualState ;

  ngOnInit()
  {
    this.SPVisualContext = 'Login';
    this.SPVisualState= 'Home';
  }

  OnSPVisualState_changed($event)
  {
    this.SPVisualState =$event;
  }

  OnSPVisualContext_changed($event)
  {
    this.SPVisualContext=$event;
  }
}
