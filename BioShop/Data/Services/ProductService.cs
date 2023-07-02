namespace BioShop.Data.Services
{
    using AutoMapper;
    using BioShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.RecipeModel;
    using BioShop.Data.ViewModels.ProductModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProductService : IProductService
    {
        private readonly BioShopDataContext _dataContext;
        private readonly IMapper _mapper;

        public ProductService(BioShopDataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddProductProductViewModel>> AddProduct([FromBody] AddProductProductViewModel product)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Expires = product.Expires,
                Price = product.Price,
                Ingredients = product.Ingredients,
                MadeInCountry = product.MadeInCountry,
            };
                //_mapper.Map<Product>(product);              

            await _dataContext.Products.AddAsync(newProduct);
            await _dataContext.SaveChangesAsync();

            var allProducts = await _dataContext.Products.ToListAsync();

            List<AddProductProductViewModel> productViewModelAddProducts = new List<AddProductProductViewModel>() { };

            foreach (var productItem in allProducts)
            {
                var returnProducts = new AddProductProductViewModel()
                {
                    Id = productItem.Id,
                    Name = productItem.Name,
                    Expires = productItem.Expires,
                    Price = productItem.Price,
                    Ingredients = productItem.Ingredients,
                    MadeInCountry = productItem.MadeInCountry,
                };

                productViewModelAddProducts.Add(returnProducts);
            }

                //_mapper.Map<List<ProductViewModelAddProduct>>(allProducts);

            return productViewModelAddProducts;
        }

        public async Task<AllRecipesProductViewModel> GetProductByIdAndAllHisRecipes(int productId)
        {
            var product = await _dataContext.Products.FindAsync(productId);

            ArgumentNullException.ThrowIfNull(product);

            var productAndRecipes = await _dataContext.Products.Where(p => p.Id == productId).Select(n => new AllRecipesProductViewModel()
            {
                Id = n.Id,
                Name = n.Name,
                Price = n.Price,
                Expires = n.Expires,
                Ingredients = n.Ingredients,
                MadeInCountry = n.MadeInCountry,
                RecipesProduct = n.Recipes.Where(p => p.CurrentProductId == productId).Select(n => new AllRecipesOnProductViewModel()
                {
                    Id = n.Id,
                    Size = n.Size,
                    Portions = n.Portions,
                    RecipeName = n.ProductName,
                    TimeYouNeedToBeMade = n.TimeYouNeedToBeMade,
                    NecesseryProductsAndQuantity = n.NecesseryProductsAndQuantity,
                    DesciptionStepByStepHowToBeMade = n.DesciptionStepByStepHowToBeMade,
                    WhitchProductBelongThisRecipe = _dataContext.Products.FindAsync(productId).Result.Name,
                }).ToList()

            }).FirstOrDefaultAsync();

            return productAndRecipes;
        }

        public async Task<IEnumerable<AllRecipesProductViewModel>> GetAllProducts()
        {
            var allProducts = _dataContext.Products.Select(n => new AllRecipesProductViewModel()
            {
                Id = n.Id,
                Name = n.Name,
                Price = n.Price,
                Expires = n.Expires,
                Ingredients = n.Ingredients,
                MadeInCountry = n.MadeInCountry,
            }).ToListAsync();

            return await allProducts;
        }

        public async Task<UpdateProductViewModel> UpdateProduct(int id, UpdateProductViewModel newProduct)
        {
            var product = await _dataContext.Products.FindAsync(id);

            ArgumentNullException.ThrowIfNull(product);

            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.Expires = newProduct.Expires;
            product.Ingredients = newProduct.Ingredients;
            product.MadeInCountry = newProduct.MadeInCountry;

            this._dataContext.SaveChanges();

            var updatedProduct = _dataContext.Products.Select(n => new UpdateProductViewModel()
            {
                Id = n.Id,
                Name = n.Name,
                Price = n.Price,
                Expires = n.Expires,
                Ingredients = n.Ingredients,
                MadeInCountry = n.MadeInCountry,
            }).FirstOrDefaultAsync(f => f.Id == id);

            return await updatedProduct;
        } 

        public async Task DeleteProductById(int id)
        {
            var product = await _dataContext.Products.FindAsync(id);

            ArgumentNullException.ThrowIfNull(product);

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
        }
    }
}