import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  @Output() public SPVisualContext_changedEvent = new EventEmitter();
  @Output() public SPVisualState_changedEvent = new EventEmitter();
  @Input() public roleContext;
  constructor() { }
  
  ngOnInit(): void {
    this.SPVisualState_changedEvent.emit('Home');
  }

  logout()
  {
    localStorage.clear();
    this.SPVisualContext_changedEvent.emit('Login');
  }

  renderProfileView()
  {
    this.SPVisualState_changedEvent.emit("Profile");
  }

  renderUsersView()
  {
    this.SPVisualState_changedEvent.emit("Users");
  }

  renderOrdersView()
  {
    this.SPVisualState_changedEvent.emit("Orders");
  }

  renderHomeView()
  {
    this.SPVisualState_changedEvent.emit("Home");
  }

  renderProductsView()
  {
    this.SPVisualState_changedEvent.emit("Products");
  }

  renderAvailableOrdersView()
  {
    this.SPVisualState_changedEvent.emit("OrdersAvailable");
  }

  renderCompletedOrdersView()
  {
    this.SPVisualState_changedEvent.emit("OrdersCompleted");
  }

  renderCurrentOrderView()
  {
    this.SPVisualState_changedEvent.emit("OrdersCurrent");
  }

  renderHistoryOrderView()
  {
    this.SPVisualState_changedEvent.emit("History");
  }

  renderNewCurrentOrderView()
  {
    this.SPVisualState_changedEvent.emit("New-Current");
  }
}
