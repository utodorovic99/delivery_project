import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, QueryList, ViewChild, ViewChildren } from '@angular/core';
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

  @Input() ordersContext='none';
  @Output() public OrdersComponentHidden_changedEvent = new EventEmitter();

  displayedColumnsOrders:     string[] = ['id', 'consumer',  'deliveryman', 'price', 'address', 'comment', 'status'];
  displayedColumnsOrderItems: string[] = ['name', 'quantity',  'unit price', 'total item price']

  dataSourceOrders = new MatTableDataSource<OrderView>(ELEMENT_DATA_ORDERS);
  dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);

  deliveryFee=0;
  controlHidden=false;
  acceptAvailable=false;
  private seledOrderId;

  constructor(private http: HttpClient, private productService:ProductService)  {  }
  
  ngOnInit(): void 
  {
    switch(this.ordersContext)
    {
      case 'all':          { this.getAllOrders();       break;}
      case 'available':    { this.getAvailableOrders(); break;}
      case 'completed':    { this.getCompletedOrders(); break;}
      case 'current':      {this.getCurrentOrder();     break;}  //For Deliveryman
      case 'history':      {this.getHistoryOrders();    break;}
      case 'new-current':  {this.getNewCurrentOrders(); break;}  //For Consumer
    }
  }

  ngAfterViewInit(): void {

    ELEMENT_DATA_ORDER_ITEMS.splice(0);
    this.dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);
    this.tables.toArray()[1].renderRows(); 

    setTimeout(() =>{
      this.dataSourceOrders.paginator = this.orderPaginator;
    }, 60); 

    setTimeout(() =>{
      this.dataSourceOrderItems.paginator = this.orderItemsPaginator;
    },100);
  }

  private getAllOrders()
  {
    if(this.ordersContext!='all') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Administrator') return;

    this.productService.getOrders(this.http).subscribe(
      {
        next: (data)=>
        {
          this.PlotOrders(data); 
        },

        error: (error)=>
        {
          //
        },
      }); 
  }

  private getAvailableOrders()
  {
    if(this.ordersContext!='available') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Deliveryman') return;

    this.productService.getAvailableOrders(this.http, JSON.parse(coverted)['username']).subscribe(
      {
        next: (data)=>
        {
          this.PlotOrders(data);
        },

        error: (error)=>
        {
          //
        },
      });
  }

  private getCompletedOrders()
  {
    if(this.ordersContext!='completed') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Deliveryman') return;

    this.productService.getCompletedOrders(this.http, JSON.parse(coverted)['username']).subscribe(
      {
        next: (data)=>
        {
          this.PlotOrders(data); 
        },

        error: (error)=>
        {
          //
        },
      }); 
  }

  private getHistoryOrders()
  {
    if(this.ordersContext!='history') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Consumer') return;

    this.productService.getHistoryOrders(this.http, JSON.parse(coverted)['username']).subscribe(
      {
        next: (data)=>
        {
          this.PlotOrders(data); 
        },

        error: (error)=>
        {
          //
        },
      });
  }

  private getCurrentOrder()
  {
    if(this.ordersContext!='current') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Deliveryman' &&
    JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Consumer') return;

    this.productService.getCurrentOrder(this.http, JSON.parse(coverted)['username']).subscribe(
      {
        next: (data)=>
        {       
          this.PlotOrders(data); 
        },

        error: (error)=>
        {
          //
        },
      });
  }

  private getNewCurrentOrders()
  {
    if(this.ordersContext!='new-current') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Consumer') return;

    this.productService.getCurrentOrder(this.http, JSON.parse(coverted)['username']).subscribe(
      {
        next: (data)=>
        {
          if((data as Order[]).length==0 && this.ordersContext=='new-current')
          {
            this.controlHidden = true;
            this.OrdersComponentHidden_changedEvent.emit(true);
          }
          else
          {
            this.controlHidden = false;
            this.OrdersComponentHidden_changedEvent.emit(false);
          }
          this.PlotOrders(data); 
        },

        error: (error)=>
        {
          //
        },
      });
  }

  private PlotOrders(data)
  {
    if(data[0]!=undefined)
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
    this.dataSourceOrders = new MatTableDataSource<OrderView>(ELEMENT_DATA_ORDERS);
    this.tables.toArray()[0].renderRows();
    setTimeout(() =>{
      this.dataSourceOrders.paginator = this.orderPaginator;
    },60);
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

  private PlotOrderItems(data)
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
    this.dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);
    this.tables.toArray()[1].renderRows();
    setTimeout(() =>{
      this.dataSourceOrderItems.paginator = this.orderItemsPaginator;
    },100); 

    this.acceptAvailable = true;
  }

  tableOrdersRowClick(rowOrder)
  {
    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";

    this.seledOrderId=rowOrder['id'];
    this.productService.getOrderItemsFor(this.http, this.seledOrderId, JSON.parse(coverted)['username']).subscribe(
      {
        
        next: (data)=>
        {
          this.PlotOrderItems(data);
        },

        error: (error)=>
        {
          //
        }
      }); 
  }

  tryOrderAccept()
  {
    if(this.ordersContext!='available') return;

    let coverted = localStorage.getItem('token_converted');
    if (coverted==null) coverted ="";
    if(JSON.parse(coverted)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!='Deliveryman') return;

    this.productService.acceptOrder(this.http, this.seledOrderId).subscribe(
      {
        next: (data)=>
        {       
          this.ordersContext='current';
          this.getCurrentOrder();
          this.PlotOrderItems('');
        },

        error: (error)=>
        {
          //
        },
      });
  }
}
