using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Produce_EnsureEnoughInventoryQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;
            if (produceViewModel != null)
            {
                if (produceViewModel.Product != null && produceViewModel.Product.ProductInventories != null)
                {
                    produceViewModel.ErrorList = null;
                    foreach(var pi in produceViewModel.Product.ProductInventories)
                    {
                        if (pi.Inventory != null &&
                            pi.InventoryQuantity * produceViewModel.QuantityToProduce > pi.Inventory.Quantity)
                        {
                            if (produceViewModel.ErrorList != null) produceViewModel.ErrorList = produceViewModel.ErrorList + "<br/>";
                            produceViewModel.ErrorList = produceViewModel.ErrorList + $"The inventory ({pi.Inventory.InventoryName}) is not enough to produce {produceViewModel.QuantityToProduce} products" ;
                        }
                    }

                    if (produceViewModel.ErrorList != null)
                    {
                        return new ValidationResult($"{ produceViewModel.ErrorList }", new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
