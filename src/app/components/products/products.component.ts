import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { first } from 'rxjs';
import { ProductService } from 'src/app/services/product.service';

export interface ProductView{
  name         :string ;
  price        :number ;
  ingredients  :string;
}

export interface ProductItemView{
  name         :string ;
}

const ELEMENT_DATA_PRODUCTS: ProductView[] = [];
const ELEMENT_DATA_SELED_INGREDIENTS: ProductItemView[] = [];

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, AfterViewInit {
  @ViewChildren(MatTable)tables!:QueryList<MatTable<ProductView|ProductItemView>>;
  @ViewChild(MatPaginator) paginator!: MatPaginator
  displayedColumnsProducts: string[] = ['name',     'price',     'ingredients'];
  displayedColumnsSeledIngredients: string[]=['name'];
  
  dataSourceProducts = new MatTableDataSource<ProductView>(ELEMENT_DATA_PRODUCTS);
  dataSourceSelectedIngredients = new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);

  public selectedIngredient;      //For dropdown
  public selectedTableIngredient  //For table

  public name;
  public price;
  public ingredients:string[] =[];

  constructor(private http: HttpClient,private productService:ProductService) { }

  ngOnInit(): void {
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
          this.dataSourceProducts= new MatTableDataSource(ELEMENT_DATA_PRODUCTS);
          this.tables.toArray()[0].renderRows(); 
        },

        error: (error)=>
        {
          //
        }
      }); 

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
  }

  ngAfterViewInit(): void {
    setTimeout(() =>{
      this.dataSourceProducts.paginator = this.paginator;
    },60);  
  }

  tableProductItemsRowClick(rowIngredient)
  {
    this.selectedTableIngredient = rowIngredient;
  }

  addIngredient()
  {
    ELEMENT_DATA_SELED_INGREDIENTS.push({name:this.selectedIngredient});
    this.dataSourceSelectedIngredients= new MatTableDataSource<ProductItemView>(ELEMENT_DATA_SELED_INGREDIENTS);
    //this.tables.toArray[1].renderRows();
    this.selectedTableIngredient=this.dataSourceSelectedIngredients.data[this.dataSourceSelectedIngredients.data.length-1]; 
  }

  productItemRemove()
  {
    if(this.selectedTableIngredient===undefined) return;

    let elem;
    let updated: ProductItemView[] =[];
    let trigered=false;

    for(let ingredient in ELEMENT_DATA_SELED_INGREDIENTS)
    {
      elem  = ELEMENT_DATA_SELED_INGREDIENTS.pop();
      if(elem['name'] !== this.selectedTableIngredient['name'])                       //Not target keep it
      {
        console.log('first');
        updated.push(elem);     
      }
      else if(elem['name'] === this.selectedTableIngredient['name'] && trigered)           //Remove only one instance of target
      {
        console.log('second');
        updated.push(elem);    
      }      
      else if(elem['name'] === this.selectedTableIngredient['name'] && (!trigered))          //Remove first instance
      {
        console.log('third');
        trigered=true; 
      } 
    }

    this.dataSourceSelectedIngredients= new MatTableDataSource<ProductItemView>(updated);
    this.tables.toArray()[1].renderRows();
    this.selectedTableIngredient=this.dataSourceSelectedIngredients.data[this.dataSourceSelectedIngredients.data.length-1];
  }

}
