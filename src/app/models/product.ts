
export class Product
{
    Name         :string ;
    Price        :number ;
    Ingredients  :Array<string> ;

    constructor(name:string, price:number, ingredients  :Array<string>)
    {
      this.Name        = name;    
      this.Price       = price; 
      this.Ingredients = ingredients;     
    };
}