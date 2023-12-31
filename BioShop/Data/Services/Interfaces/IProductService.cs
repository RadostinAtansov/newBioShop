﻿namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels.ProductModels;

    public interface IProductService
    {
         Task DeleteProductById(int id);

         Task<IEnumerable<AllRecipesProductViewModel>> GetAllProducts();

         Task<AllRecipesProductViewModel> GetProductByIdAndAllHisRecipes(int id);

         Task<IEnumerable<AddProductViewModel>> AddProduct(AddProductViewModel product);

         Task<UpdateProductViewModel> UpdateProduct(int id, UpdateProductViewModel newProduct);
    }
}