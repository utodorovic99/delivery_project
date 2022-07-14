import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { Order } from '../models/order';
import { Product } from '../models/product';
import { OrderItem } from '../models/orderItem';
import { PrimitiveResponse } from '../models/primitiveResponse';

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
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    let serviceUrl="orders";
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, {headers: this.headers}); 
  }

  public getAvailableOrders(http:HttpClient, username:string):Observable<number | Array<Order>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    
    let serviceUrl="orders";
    let subserviceUrl = 'available-for'
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subserviceUrl}/${username}`, {headers: this.headers}); 
  }

  public getCompletedOrders(http:HttpClient, username:string):Observable<number | Array<Order>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    let subserviceUrl = 'completed-for'
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subserviceUrl}/${username}`, {headers: this.headers});  
  }

  public getHistoryOrders(http:HttpClient, username:string):Observable<number | Array<Order>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    let subserviceUrl = 'confirmed-for'
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subserviceUrl}/${username}`, {headers: this.headers});  
  }
  
  public getCurrentOrder(http:HttpClient, username:string):Observable<number | Array<Order>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    let subserviceUrl = 'current-for'
    return http.get<Array<Order>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subserviceUrl}/${username}`, {headers: this.headers});  
  }

  public getOrderItemsFor(http:HttpClient, orderId:string, username:string):Observable<number | Array<OrderItem>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    return http.get<Array<OrderItem>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${orderId}/items`, {headers: this.headers}); 
  }

  public getAllProducts(http:HttpClient):Observable<number | Array<Product>>
  {

    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="products";
    return http.get<Array<Product>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}`, {headers: this.headers}); 
  }

  public getAllIngredients(http:HttpClient):Observable<number | Array<string>>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="products";
    let subService ="ingredients";
    return http.get<Array<string>>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subService}`, {headers: this.headers}); 
  }

  public addProduct(http:HttpClient, product:Product):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="products";
    let subService ="create";

    let body = JSON.stringify(product);
    return http.post<PrimitiveResponse>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subService}`,body, {headers: this.headers}); 
  }

  public acceptOrder(http:HttpClient, orderId:string):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    let subService ="accept";

    return http.post<PrimitiveResponse>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subService}/${orderId}`, '', {headers: this.headers}); 
  }

  public submitOrder(http:HttpClient, order:Order, username:string):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    
    let serviceUrl="orders";
    let subService ="publish";

    let body = JSON.stringify(order);
    return http.post<PrimitiveResponse>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subService}/${username}`,body, {headers: this.headers}); 
  }

  public getDeliveryFee(http:HttpClient):Observable<number | PrimitiveResponse>
  {
    this.headers= new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })


    let serviceUrl="orders";
    let subserviceUrl = 'delivery-fee'
    return http.get<PrimitiveResponse>(`${environment.apiUrl}/${this.controllerUrl}/${serviceUrl}/${subserviceUrl}`, {headers: this.headers});  
  }
}



