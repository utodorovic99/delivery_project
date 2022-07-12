import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { first } from 'rxjs';
import { PrimitiveResponse } from 'src/app/models/primitiveResponse';
import { Product } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';


export interface ProductView{
  name         :string ;
  price        :number ;
  ingredients  :string;
}

export interface ProductItemView{
  name         :string ;
}

export interface OrderItemView{

  name:        string;
  quantity:    number;
  unitPrice :  number;
  totalItemPrice : number;
}

const ELEMENT_DATA_ORDER_ITEMS: OrderItemView[] = [];

const ELEMENT_DATA_PRODUCTS: ProductView[] = [];
const ELEMENT_DATA_SELED_INGREDIENTS: ProductItemView[] = [];

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  @ViewChildren(MatTable)tables!:QueryList<MatTable<ProductView|ProductItemView|OrderItemView>>;
  @ViewChild(MatPaginator) productPaginator!: MatPaginator;
  
  @Input() RenderNewProduct;
  
  displayedColumnsProducts: string[] = ['name',     'price',     'ingredients'];
  displayedColumnsSeledIngredients: string[]=['name'];
  displayedColumnsOrderItems: string[] = ['name', 'quantity',  'unit price', 'total item price']
  
  dataSourceProducts = new MatTableDataSource<ProductView>(ELEMENT_DATA_PRODUCTS);
  dataSourceSelectedIngredients = new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);
  dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);

  public selectedIngredient;      //For dropdown
  public selectedTableIngredient;  //For table
  public selectedProduct;
  public selectedItem;

  submitAvailable=false;
  unitsNum=1;
  deliveryFee=0;
  totalOrderPrice;

  public name;
  public price;
  public ingredients:string[] =[];
  public errTxt="";

  constructor(private http: HttpClient,private productService:ProductService) { }

  ngOnInit(): void {
    ELEMENT_DATA_ORDER_ITEMS.splice(0);
    this.LoadProducts();

      this.productService.getAllIngredients(this.http).subscribe(
        {
          next: (data)=>
          {
            for(let ingredient in data as unknown as string[]) 
              this.ingredients.push(data[ingredient]);

              this.selectedIngredient=this.ingredients[0];
          },
  
          error: (error)=>
          {
            //
          }
        }); 


        this.LoadProducts();

      this.productService.getDeliveryFee(this.http).subscribe(
        {
          next: (data)=>
          {
            this.deliveryFee=(data as PrimitiveResponse)['value'] ;
            this.totalOrderPrice = this.deliveryFee;
          },
  
          error: (error)=>
          {
            //
          }
        }); 
  }

  private LoadProducts()
  {
    this.productService.getAllProducts(this.http).subscribe(
      {
        next: (data)=>
        {
          ELEMENT_DATA_PRODUCTS.splice(0);
          ELEMENT_DATA_SELED_INGREDIENTS.splice(0);
          for(let product in data as unknown as ProductView[])
          {       
            let ingredientsStr = "";
            for(let ingredient in data[product]['ingredients'])
             ingredientsStr+=(data[product]['ingredients'][ingredient]+',');
            
            ingredientsStr=ingredientsStr.slice(0, -1);
            ingredientsStr+=';';

            ELEMENT_DATA_PRODUCTS.push({name:data[product]['name'], price:data[product]['price'],ingredients: ingredientsStr});
          }
          this.dataSourceProducts= new MatTableDataSource<ProductView>(ELEMENT_DATA_PRODUCTS);
          this.tables.toArray()[0].renderRows();
          
          setTimeout(() =>{
            this.dataSourceProducts.paginator = this.productPaginator;
          },150);  

        },

        error: (error)=>
        {
          //
        }
      }); 
  }

  tableProductItemsRowClick(rowIngredient)
  {
    this.selectedTableIngredient = rowIngredient;
  }

  addIngredient()
  {
    ELEMENT_DATA_SELED_INGREDIENTS.push({name:this.selectedIngredient});
    this.dataSourceSelectedIngredients= new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);
    this.selectedTableIngredient=this.dataSourceSelectedIngredients.data[this.dataSourceSelectedIngredients.data.length-1]; 
  }

  productItemRemove()
  {
    if(this.selectedTableIngredient===undefined) return;

    let updated: ProductItemView[] =[];
    let trigered=false;

    for(let ingredient in ELEMENT_DATA_SELED_INGREDIENTS)
    {
      if(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]['name'] != this.selectedTableIngredient['name'])                       //Not target keep it
      {
        updated.push(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]);     
      }
      else if(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]['name'] == this.selectedTableIngredient['name'] && trigered)           //Remove only one instance of target
      {
        updated.push(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]);    
      }      
      else if(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]['name'] == this.selectedTableIngredient['name'] && (!trigered))          //Remove first instance
      {
        trigered=true; 
      } 
    }

    ELEMENT_DATA_SELED_INGREDIENTS.splice(0);
    for(let update in updated)
      ELEMENT_DATA_SELED_INGREDIENTS.push(updated[update]);

    this.dataSourceSelectedIngredients= new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);
    this.tables.toArray()[1].renderRows();
    this.selectedTableIngredient=this.dataSourceSelectedIngredients.data[this.dataSourceSelectedIngredients.data.length-1];
  }

  saveProduct()
  {
    this.errTxt="";
    let ingredients:string[] = [];
    for(let ingredient in ELEMENT_DATA_SELED_INGREDIENTS)
      ingredients.push(ELEMENT_DATA_SELED_INGREDIENTS[ingredient]['name']);

    let product = new Product(this.name, this.price, ingredients);
    this.errTxt=this.validateProduct(product);

    if(this.errTxt=='')
    {
      this.productService.addProduct(this.http, product).subscribe(
        {
          next: (data)=>
          {
            this.LoadProducts();
            this.errTxt='';
            this.name="";
            this.price=0;

            ELEMENT_DATA_SELED_INGREDIENTS.splice(0);
            this.dataSourceSelectedIngredients= new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);
            this.tables.toArray()[1].renderRows(); 
          },
  
          error: (error)=>
          {
            this.errTxt==error.errTxt;
          }
        }); 
    }
  }

  private validateProduct(product:Product):string
  {
    let errTxt="";
    if(product.Name == undefined || product.Name.length<5)         
      errTxt+='Name must not be shorter than 5 characters;';
    else if (product.Name != undefined && product.Name.length>30)     
      errTxt+='Name must not be larger than 30 characters;';

    if(product.Price === undefined || product.Price <0 )             errTxt+='Price must be greater then 0 (0 if promotion);';

    console.log(product.Ingredients);
    if(product.Ingredients === undefined  || product.Ingredients.length <2) 
      errTxt+='Minimal number of ingredients is 2;';
    else if(product.Ingredients !== undefined  && product.Ingredients.length >15) 
      errTxt+='Maximal number of ingredients is 15;';
    else
    {
      let unknownDetected= false;
      let detected=false;
      for(let ingredient in product.Ingredients)
      {
        detected = false;
        for(let ingredientSeled in this.ingredients)
        {
          if(product.Ingredients[ingredient] == this.ingredients[ingredientSeled])
          {
            detected = true;
            break;
          }
        }

        if(!detected)
        {
          unknownDetected=true;
          break;
        }
      }

      if(unknownDetected) errTxt+='Unsupported ingredients detected;';
    }

    for(let productEx in ELEMENT_DATA_PRODUCTS)
    {
      if(product.Name == ELEMENT_DATA_PRODUCTS[productEx]['name'])
        errTxt+='Name already used;';
    }

    return errTxt;
  }
  
  tableProductsRowClick(selectedProduct)
  {
    this.selectedProduct = selectedProduct;
  }

  tableItemsRowClick(rowItem)
  {
    this.selectedItem = rowItem;
  }

  tryOrderSubmit()
  {

  }

  tryAddToCart()
  {
    let itemPrice=parseFloat(this.unitsNum.toString()) * parseFloat(this.selectedProduct['price']);
    ELEMENT_DATA_ORDER_ITEMS.push({name:this.selectedProduct['name'], quantity:this.unitsNum as number,  
    unitPrice:this.selectedProduct['price'] as number, totalItemPrice: itemPrice});
    this.dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);
    this.tables.toArray()[1].renderRows();
    this.totalOrderPrice= parseFloat(this.totalOrderPrice) + parseFloat(itemPrice.toString());
    this.unitsNum=1;
  }

  tryRemoveFromCart()
  {
    if(this.selectedItem===undefined) return;

    let updated: OrderItemView[] =[];

    let triggered = false;
    for(let ingredient in ELEMENT_DATA_ORDER_ITEMS)
    {
      if(ELEMENT_DATA_ORDER_ITEMS[ingredient]['name'] == this.selectedItem['name'] && 
           ELEMENT_DATA_ORDER_ITEMS[ingredient]['quantity'] == this.selectedItem['quantity'] &&
           ELEMENT_DATA_ORDER_ITEMS[ingredient]['qunitPrice'] == this.selectedItem['unitPrice'] &&
           ELEMENT_DATA_ORDER_ITEMS[ingredient]['totalItemPrice'] == this.selectedItem['totalItemPrice'] )                       
      {
        if(triggered)
          updated.push(ELEMENT_DATA_ORDER_ITEMS[ingredient]);  //First instance deleted keep
        else
        {
          this.totalOrderPrice= parseFloat(this.totalOrderPrice) - (parseFloat(this.selectedItem['quantity'].toString()) * parseFloat(this.selectedItem['price']));
          triggered = true;
        }     
      }
      else
      {
        updated.push(ELEMENT_DATA_ORDER_ITEMS[ingredient]);  
      }

      this.dataSourceOrderItems = new MatTableDataSource<OrderItemView>(ELEMENT_DATA_ORDER_ITEMS);
      this.tables.toArray()[1].renderRows();
    }
  }

}
