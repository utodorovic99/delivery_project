using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Model
{
  public class ProductDefiniton
  {
    public ProductDefiniton(int productId, int ingredientId)
    {
      ProductId = productId;
      IngredientId = ingredientId;
    }

    public ProductDefiniton()
    {
      ProductId = 0;
      IngredientId = 0;
    }

    //Composite key ProductId + IngredientId (assign specific ingredient to specific product)
    public int ProductId { get; set; }        //Product ID (Wich Product is associated with specific ingredient referenced by IngredientID)
    public int IngredientId { get; set; }     //Ingredient ID (References Ingredients table)
  }
}
