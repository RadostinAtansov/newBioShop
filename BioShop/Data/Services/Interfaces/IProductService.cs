namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.ViewModels;

    public interface IProductService
    {
         Task<IEnumerable<ProductViewModel>> AddProduct(ProductViewModel product);

         Task<ProductViewModel> GetProductByIdAndAllHisRecipes(int id);

         Task<IEnumerable<ProductViewModel>> GetAllProducts();

         Task<ProductViewModel> UpdateProduct(int id, ProductViewModel newProduct);

         Task DeleteProductById(int id);

    }
}