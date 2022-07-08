import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { MatTable } from '@angular/material/table';

export interface Order{
  id:          number;
  consumer:    string;
  deliveryman: string;
  price:       number;
  address:     string;
  comment:     string;
  status:      string;
}

export interface OrderItem{

  name:        string;
  quantity:    number;
  unitPrice :  number;
  totalItemPrice : number;
}

const ELEMENT_DATA_ORDERS: Order[] = [];
const ELEMENT_DATA_ORDER_ITEMS: OrderItem[] = [];

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit  {

  @ViewChildren(MatTable)tables!: QueryList<MatTable<any>>;
  displayedColumnsOrders:     string[] = ['id', 'consumer',  'deliveryman', 'price', 'address', 'comment', 'status'];
  displayedColumnsOrderItems: string[] = ['name', 'quantity',  'unit price', 'total item price']
  dataSourceOrders = ELEMENT_DATA_ORDERS;
  dataSourceOrderItems = ELEMENT_DATA_ORDER_ITEMS;
  deliveryFee=0;
  constructor(private http: HttpClient, private productService:ProductService) { }

  ngOnInit(): void {
    this.productService.getOrders(this.http).subscribe(
      {
        next: (data)=>
        {
          this.deliveryFee=data[0]['deliveryFee'];
          this.dataSourceOrders.splice(0);
          let price =0;
          for(let order in data as Order[])
          {
            price = data[order]['deliveryFee'];      //Mogao sam izracunati na serveru da rasteretim mrezu, jer svakako OrderItems ucitavam po kliku.
            for(let item in data[order]['items'])
              price+=(data[order]['items'][item]['quantity'] as number *data[order]['items'][item]['unitPrice'] as number );         

            this.dataSourceOrders.push({id:data[order]['id'], consumer:data[order]['consumer'], deliveryman:data[order]['deliveryman'] == "" ? "none" : (data[order]['deliveryman']), 
                               price:price,          address:data[order]['address'],   comment:data[order]['comment'], 
                               status: this.GetOrderStatusByCode(data[order]['status'])});
          }
           this.tables.toArray()[0].renderRows(); 
        },

        error: (error)=>
        {
          //
        }
      }); 
  }

  private GetOrderStatusByCode(code:string):string
  {
    switch(code.toLowerCase())
    {
      case "a": {return "available";}
      case "t": {return "taken";}
      case "c": {return "completed";}
      default: {return "";}
    }
  }

  tableOrdersRowClick(rowOrder)
  {
    this.productService.getOrderItemsFor(this.http, rowOrder['id']).subscribe(
      {
        
        next: (data)=>
        {
          this.dataSourceOrderItems.splice(0);
          let itemPrice=0;
          for(let orderItem in data as any)
          {
            itemPrice=(data[orderItem]['quantity'] as number)*(data[orderItem]['unitPrice'] as number);
            this.dataSourceOrderItems.push({name:data[orderItem]['name'], quantity:data[orderItem]['quantity'] as number,  
                                            unitPrice:data[orderItem]['unitPrice'] as number, 
                                            totalItemPrice: itemPrice}); 
          }
          this.tables.toArray()[1].renderRows();
        },

        error: (error)=>
        {
          //
        }
      }); 
  }



}
