export class OrderItem
{
    Name:         string;
    Quantity :    number;
    UnitPrice :   number;

    constructor(Name:string, Quantity:number, UnitPrice:number)
    {
      this.Name        = Name;    
      this.Quantity    = Quantity; 
      this.UnitPrice   = UnitPrice;      
    };

}