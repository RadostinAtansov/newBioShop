namespace BioShop.Data.Services.Interfaces
{
    using BioShop.Data.Models;
    using BioShop.Data.ViewModels;

    public interface IProductService
    {
         Task<IEnumerable<ProductViewModel>> AddProduct(ProductViewModel product);

         Task<ProductViewModel> GetProductById(int id);

         Task<IEnumerable<ProductViewModel>> GetAllProducts();

         Task<Product> UpdateProduct(int id, ProductViewModel newProduct);

         Task DeleteProductById(int id);

    }
}
