using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Model;

namespace DeliveryService.Services
{
  public class ProductValidator
  {
    public static bool ValidateNewOrderParams(DeliveryDataContext dataContext, OrderDTO order, out Dictionary<string, string> errs)
    {
      errs = new Dictionary<string, string>();
      if (order.Consumer == null || order.Consumer == "")
        errs.Add("Consumer", "Empty Field");

      {
        User user = null;
        if ((user = dataContext.Users.Find(order.Consumer)) == null)
          errs.Add("Consumer", "Invalid username");
        else if (!user.Type.Equals('c'))
          errs.Add("Consumer", "Invalid user type (consumer expected)");
      }

      if (order.Address == null || order.Consumer == "")
        errs.Add("Address", "Empty Field");
      else if (order.Address.Length < 8)
        errs.Add("Address", "Minimum length 8");

      List<string> corruptedProducts = new List<string>();
      foreach (var orderItem in order.Items)
      {
        if (dataContext.Products.FirstOrDefault(x => x.Name.Equals(orderItem.Name)) == null)
          corruptedProducts.Add(orderItem.Name);
      }
      if (corruptedProducts.Count > 0)
      {
        var errStr = "Products not found: ";
        foreach (var item in corruptedProducts)
          errStr += $"{item},";
        errStr.TrimEnd(',');
        errStr.Append('.');
        errs.Add("Items", errStr);
      }

      return errs.Count == 0;
    }

    public static bool ValidateNewProductParams(DeliveryDataContext dataContext, ProductDTO product, out Dictionary<string, string> errs)
    {
      errs = new Dictionary<string, string>();
      List<string> corruptedIngredients = new List<string>();
      foreach (var ingredient in product.Ingredients)
      {
        if (dataContext.Ingredients.FirstOrDefault(x => x.Name.Equals(ingredient)) == null)
          corruptedIngredients.Add(ingredient);
      }
      if (corruptedIngredients.Count > 0)
      {
        var errStr = "Ingredients not found: ";
        foreach (var item in corruptedIngredients)
          errStr += $"{item},";
        errStr.TrimEnd(',');
        errStr.Append('.');
        errs.Add("Ingredients", errStr);
      }

      return errs.Count == 0;
    }

    public static void ErrCollectionToString(Dictionary<string, string> errs, out string errStr)
    {
      errStr = "ERS:";
      foreach (var kvPair in errs)
      {
        errStr += $"{kvPair.Key}={kvPair.Value};";
      }
    }
  }
}
