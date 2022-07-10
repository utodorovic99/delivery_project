import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Order } from 'src/app/models/order';

export interface OrderView{
  id:          number;
  consumer:    string;
  deliveryman: string;
  price:       number;
  address:     string;
  comment:     string;
  status:      string;
}

export interface OrderItemView{

  name:        string;
  quantity:    number;
  unitPrice :  number;
  totalItemPrice : number;
}

const ELEMENT_DATA_ORDERS: OrderView[] = [];
const ELEMENT_DATA_ORDER_ITEMS: OrderItemView[] = [];

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit, AfterViewInit  {
  @ViewChildren(MatTable)tables!:QueryList<MatTable<OrderView|OrderItemView>>;
  @ViewChild(MatPaginator) orderPaginator!: MatPaginator;
  @ViewChild(MatPaginator) orderItemsPaginator!: MatPaginator;

  displayedColumnsOrders:     string[] = ['id', 'consumer',  'deliveryman', 'price', 'address', 'comment', 'status'];
  displayedColumnsOrderItems: string[] = ['name', 'quantity',  'unit price', 'total item price']

  dataSourceOrders = new MatTableDataSource<OrderView>(ELEMENT_DATA_ORDERS);
  dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);
  
  deliveryFee=0;
  constructor(private http: HttpClient, private productService:ProductService)  {  }
  
  ngOnInit(): void {
    this.productService.getOrders(this.http).subscribe(
      {
        next: (data)=>
        {
          this.deliveryFee=data[0]['deliveryFee'];
          ELEMENT_DATA_ORDERS.splice(0);
          let price =0;
          for(let order in data as Order[])
          {
            price = data[order]['deliveryFee'];      //Mogao sam izracunati na serveru da rasteretim mrezu, jer svakako OrderItems ucitavam po kliku.
            for(let item in data[order]['items'])
              price+=(data[order]['items'][item]['quantity'] as number *data[order]['items'][item]['unitPrice'] as number );         

              ELEMENT_DATA_ORDERS.push({id:data[order]['id'], consumer:data[order]['consumer'], deliveryman:data[order]['deliveryman'] == "" ? "none" : (data[order]['deliveryman']), 
                               price:price,          address:data[order]['address'],   comment:data[order]['comment'], 
                               status: this.GetOrderStatusByCode(data[order]['status'])});
          }
          this.dataSourceOrders = new MatTableDataSource(ELEMENT_DATA_ORDERS);
          this.tables.toArray()[0].renderRows(); 
        },

        error: (error)=>
        {
          //
        },
      }); 
  }

  ngAfterViewInit(): void {
    setTimeout(() =>{
      this.dataSourceOrders.paginator = this.orderPaginator;
    }, 60); 
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
          ELEMENT_DATA_ORDER_ITEMS.splice(0);
          let itemPrice=0;
          for(let orderItem in data as any)
          {
            itemPrice=(data[orderItem]['quantity'] as number)*(data[orderItem]['unitPrice'] as number);
            ELEMENT_DATA_ORDER_ITEMS.push({name:data[orderItem]['name'], quantity:data[orderItem]['quantity'] as number,  
                                            unitPrice:data[orderItem]['unitPrice'] as number, 
                                            totalItemPrice: itemPrice}); 
          }
          this.tables.toArray()[1].renderRows();
          setTimeout(() =>{
            this.dataSourceOrderItems.paginator = this.orderItemsPaginator;
          },60); 
        },

        error: (error)=>
        {
          //
        }
      }); 
  }



}
