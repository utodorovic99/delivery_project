import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { Order } from '../components/orders/orders.component';
import { OrderItem } from '../models/orderItem';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  controllerUrl="product";
  headers= new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  })
  constructor() { }

  public getOrders(http:HttpClient):Observable<number | Array<Order>>
  {
    let serviceUrl="orders";
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, {headers: this.headers}); 
  }

  //[Route("api/[controller]/orders/{orderId}/items")]
  public getOrderItemsFor(http:HttpClient, orderId:string):Observable<number | Array<OrderItem>>
  {
    let serviceUrl="orders";
    let subService ="items";
    return http.get<Array<OrderItem>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${orderId}/items`, {headers: this.headers}); 
  }
}



