import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule} from '@angular/material/dialog';
import { ProfileComponent } from './components/profile/profile.component';
import { MenuComponent } from './components/menu/menu.component';
import {MatTableModule} from '@angular/material/table'
import { DataSource } from '@angular/cdk/table';
import { OrdersComponent } from './components/orders/orders.component'

@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    ProfileComponent,
    MenuComponent,
    OrdersComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatTableModule,   
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
