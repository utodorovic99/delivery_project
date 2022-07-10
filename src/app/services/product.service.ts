import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { Order } from '../models/order';
import { Product } from '../models/product';
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

  public getOrderItemsFor(http:HttpClient, orderId:string):Observable<number | Array<OrderItem>>
  {
    let serviceUrl="orders";
    return http.get<Array<OrderItem>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${orderId}/items`, {headers: this.headers}); 
  }

  public getAllProducts(http:HttpClient):Observable<number | Array<Product>>
  {
    let serviceUrl="products";
    return http.get<Array<Product>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, {headers: this.headers}); 
  }

  public getAllIngredients(http:HttpClient):Observable<number | Array<string>>
  {
    let serviceUrl="products";
    let subService ="ingredients";
    return http.get<Array<string>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subService}`, {headers: this.headers}); 
  }

}



