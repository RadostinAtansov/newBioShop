namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IProductService
    {
         Task DeleteProductById(int id);

         Task<IEnumerable<ProductViewModel>> GetAllProducts();

         Task<ProductViewModel> GetProductByIdAndAllHisRecipes(int id);

         Task<IEnumerable<ProductViewModel>> AddProduct(ProductViewModel product);

         Task<ProductViewModel> UpdateProduct(int id, ProductViewModel newProduct);
    }
}