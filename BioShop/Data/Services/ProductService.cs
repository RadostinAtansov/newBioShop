namespace BioShop.Data.Services
{
    using BioShop.Data.Models;
    using BioShop.Data.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using BioShop.Data.Services.Interfaces;

    public class ProductService : IProductService
    {
        private readonly BioShopDataContext _dataContext;

        public ProductService(BioShopDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<ProductViewModel>> AddProduct(ProductViewModel product)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price,
                MadeInCountry = product.MadeInCountry,
                Expires = product.Expires,
                Ingredients = product.Ingredients,
            };

            await _dataContext.Products.AddAsync(newProduct);
            await _dataContext.SaveChangesAsync();

            var allProducts = _dataContext.Products.Select(n => new ProductViewModel()
            {
                Name = product.Name,
                Price = product.Price,
                MadeInCountry = product.MadeInCountry,
                Expires = product.Expires,
                Ingredients = product.Ingredients,
            }).ToList();

            return allProducts;
        } //Yes

        public async Task<ProductViewModel> GetProductByIdAndAllHisRecipes(int productId)
        {
            var product = _dataContext.Products.FindAsync(productId);

            ArgumentNullException.ThrowIfNull(product);

            var productAndRecipes = _dataContext.Products.Where(p => p.Id == productId).Select(n => new ProductViewModel()
            {
                  Name = n.Name,
                  Price = n.Price,
                  Expires = n.Expires,
                  Ingredients = n.Ingredients,
                  MadeInCountry = n.MadeInCountry,
                  RecipesProduct = n.Recipes.Where(p => p.CurrentProductId == productId).Select(n => new RecipeViewModel()
                  {
                      RecipeName = n.ProductName,
                      Size = n.Size,
                      Portions = n.Portions,
                      DesciptionStepByStepHowToBeMade = n.DesciptionStepByStepHowToBeMade,
                      TimeYouNeedToBeMade = n.TimeYouNeedToBeMade,
                      NecesseryProductsAndQuantity = n.NecesseryProductsAndQuantity
                  }).ToList()

            }).FirstOrDefaultAsync();


            return await productAndRecipes;
        } //Yes

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            var allProducts = _dataContext.Products.Select(n => new ProductViewModel()
            {
                Name = n.Name,
                Price = n.Price,
                Expires = n.Expires,
                Ingredients = n.Ingredients,
                MadeInCountry = n.MadeInCountry,
            }).ToListAsync();

            return await allProducts;
        } //Yes

        public async Task<ProductViewModel> UpdateProduct(int id, ProductViewModel newProduct)
        {
            var product = await _dataContext.Products.FindAsync(id);

            ArgumentNullException.ThrowIfNull(product);

            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.Expires = newProduct.Expires;
            product.Ingredients = newProduct.Ingredients;
            product.MadeInCountry = newProduct.MadeInCountry;

            this._dataContext.SaveChanges();

            var updatedProduct = _dataContext.Products.Select(n => new ProductViewModel()
            {
                Name = n.Name,
                Price = n.Price,
                Expires = n.Expires,
                Ingredients = n.Ingredients,
                MadeInCountry = n.MadeInCountry,
            }).FirstOrDefaultAsync(f => f.Id == id);

            return await updatedProduct;
        } //Yes

        public async Task DeleteProductById(int id)
        {
            var product = await _dataContext.Products.FindAsync(id);

            ArgumentNullException.ThrowIfNull(product);

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
        }
    }
}