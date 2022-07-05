import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  @Output() public SPVisualState_changedEvent = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  logout()
  {
    localStorage.removeItem('token');
    this.SPVisualState_changedEvent.emit(0);
  }

}
