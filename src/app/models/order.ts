import { OrderItem } from "./orderItem";

export class Order
{
    Items:       Array<OrderItem>;
    DeliveryFee: number;
    Address:     string;
    Comment:     string;
    Status:      string;
    Id:          number;
    Consumer:    string;
    Deliveryman: string;
    Price:       number;

    

    constructor(Items:Array<OrderItem>, DeliveryFee:number, Address:string, Comment:string,
        Status:string, Id:number,Consumer:string, Deliveryman: string,Price: number)
    {
        this.Items        = Items; 
        this.DeliveryFee  = DeliveryFee;
        this.Address      = Address;
        this.Comment      = Comment;
        this.Status       = Status;
        this.Id           = Id;
        this.Consumer     = Consumer;
        this.Deliveryman  = Deliveryman;
        this.Price        = Price;
    };
}