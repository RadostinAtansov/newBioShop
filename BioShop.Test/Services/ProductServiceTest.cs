namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.ProductModels;
    using BioShop.Data.ViewModels.RecipeModel;
    using BioShop.Data.Models;
    using AutoMapper;
    using FakeItEasy;

    public class ProductServiceTest
    {
        private readonly Mock<IProductService> _productService;
        private readonly IMapper _mapper;

        public ProductServiceTest()
        {
            _productService = new Mock<IProductService>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task AddProductToDatabaseAndReturnCorrectResultAllProducts()
        {
            //Arrange
            var productList = await ProductData();
            int productListCount = 3;
            var fakeProduct = new Product()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 44,
                MadeInCountry = "Bg4",
            };
            var fakeProductWithModel = new AddProductViewModel()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 44,
                MadeInCountry = "Bg4",
            };
            var productListMapped = _mapper.Map <List<AddProductViewModel>>(productList);

            _productService.Setup(x => x.AddProduct(fakeProductWithModel))
                .Callback(() =>
                {
                    productList.Add(fakeProduct);
                    productListCount++;
                })
                .ReturnsAsync(productListMapped);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.AddProductToShop(fakeProductWithModel);
            var productResultModel = ((ObjectResult)productResult).Value as List<AllRecipesProductViewModel>;

            //Assert
            Assert.True(productList[3] == fakeProduct);
            Assert.True(productList[3].Id == fakeProduct.Id);
            Assert.True(productList.Count == productListCount);
        }

        [Fact]
        public async Task AddProductToDatabaseAndReturnArgumentNullExceptionIfProductIsNull()
        {
            //Arrange
            var productList = await ProductData();
            AddProductViewModel fakeProduct = null;
            _productService.Setup(x => x.AddProduct(fakeProduct));  
            var productController = new ProductController(_productService.Object);

            //Act
            //Assert
           await Assert.ThrowsAsync<ArgumentNullException>(() => productController.AddProductToShop(fakeProduct));

        }

        [Fact]
        public async Task GetAllProductsAndReturnCorrectAllProducts()
        {
            //Arrange
            var productList = await ProductData();
            var allProductMapped = _mapper.Map<List<AllRecipesProductViewModel>>(productList);
            _productService.Setup(x => x.GetAllProducts())
                .ReturnsAsync(allProductMapped);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetAllProducts();
            var productResultModel = ((ObjectResult)productResult).Value as List<AllRecipesProductViewModel>;

            //Assert
            Assert.Equal(allProductMapped, productResultModel);
        }

        [Fact]
        public async Task GetProductByIdAndAllHisRecipesIfProductIsNullThrowArgumentNullException()
        {
            //Arrange
            int productId = 0;
            var productList = await ProductData();
            _productService.Setup(x => x.GetProductByIdAndAllHisRecipes(productId))
                .Callback(() =>
                {
                    var product = productList.Find(x => x.Id == productId);
                    ArgumentNullException.ThrowIfNull(product);
                });
            var productController = new ProductController(_productService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => productController.GetProductByIdAndAllHisRecipes(productId));
        }

        [Fact]
        public async Task GetProductByIdAndAllHisRecipesIfNotNullReturnCorrectResultProductAndAllRecipes()
        {
            //Arrange
            int productId = 3;
            var productList = await ProductData();
            var returnProductRecipe = new AllRecipesProductViewModel()
            {
                Id = 3,
                Name = "Torta3",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco, Milk, Eggs3",
                Price = 12,
                MadeInCountry = "Bg3",
                RecipesProduct = new List<AllRecipesOnProductViewModel>()
                        {
                            new AllRecipesOnProductViewModel()
                            {
                                Id = 1,
                                ProductName = "test",
                                Size = 100,
                                Portions = 8,
                                TimeYouNeedToBeMade = 2
                            }
                        }
            };

            _productService.Setup(x => x.GetProductByIdAndAllHisRecipes(productId))
                .Callback(() =>
                {
                    var product = productList.Find(x => x.Id == productId);
                })
                .ReturnsAsync(returnProductRecipe);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetProductByIdAndAllHisRecipes(productId);
            var productResultModul = ((ObjectResult)productResult).Value as AllRecipesProductViewModel;

            //Assert
            Assert.Equal(productId, productList[2].Id);
            Assert.True(returnProductRecipe.RecipesProduct.Count() > 0);
        }

        [Fact]
        public async Task UpdateProductIfProductIdDoesNotExistThrowArgumentNullException()
        {
            //Arrange
            var productList = await ProductData();
            int productId = 0;
            var fakeProduct = new UpdateProductViewModel()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 44,
                MadeInCountry = "Bg4",
            };
            _productService.Setup(x => x.UpdateProduct(productId, fakeProduct))
                .Callback(() =>
                {
                    var product = productList.Find(x => x.Id == productId);
                    ArgumentNullException.ThrowIfNull(product);
                });
            var productController = new ProductController(_productService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => productController.UpdateProduct(productId, fakeProduct));
        }

        [Fact]
        public async Task UpdateProductByIdAndReturnUpdatedProduct()
        {
            //Arrange
            var productList = await ProductData();
            int productId = 1;
            var fakeProductViewModel = new UpdateProductViewModel()
            {
                Id = 111,
                Name = "Torta111",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco111, Milk111, Eggs111",
                Price = 111,
                MadeInCountry = "Bg111",

            };
            var fakeProduct = new Product()
            {
                Id = 111,
                Name = "Torta111",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco111, Milk111, Eggs111",
                Price = 111,
                MadeInCountry = "Bg111",

            };
            _productService.Setup(x => x.UpdateProduct(productId, fakeProductViewModel))
                .Callback(() =>
                {
                    productList[0] = fakeProduct;
                });
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.UpdateProduct(productId, fakeProductViewModel);
            
            //Assert
            Assert.Equal(fakeProduct, productList[0]);
            Assert.Equal(fakeProduct.Id, productList[0].Id);
            Assert.Equal(fakeProduct.Name, productList[0].Name);
            Assert.Equal(fakeProduct.Price, productList[0].Price);
            Assert.Equal(fakeProduct.Expires, productList[0].Expires);
            Assert.Equal(fakeProduct.Ingredients, productList[0].Ingredients);
        }

        [Fact]
        public async Task DeleteProductByIdIfDoesNotExistThrowArgumentNullException()
        {
            //Arrange
            var productList = await ProductData();
            int productId = 0;
            _productService.Setup(x => x.DeleteProductById(productId))
                .Callback(() =>
                {
                    var product = productList.Find(f => f.Id == productId);
                    ArgumentNullException.ThrowIfNull(product);
                });
            var productController = new ProductController(_productService.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => productController.DeleteProductById(productId));
        }

        [Fact]
        public async Task DeleteProductByIdAndCheckForCorrectReseult()
        {
            //Arrange
            var productList = await ProductData();
            int productId = 3;
            _productService.Setup(x => x.DeleteProductById(productId))
                .Callback(() =>
                {
                    var product = productList.Find(f => f.Id == productId);
                    productList.Remove(product);
                });
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.DeleteProductById(productId);

            //Assert
            Assert.True(productList.Count == 2);
            Assert.True(productList[0].Id == 1);
            Assert.True(productList[1].Id == 2);
        }

        private async Task<List<Product>> ProductData()
        {
            List<Product> productList = new List<Product>()
                {
                    new Product()
                    {
                        Id = 1,
                        Name = "Torta1",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        Clients_Products = new List<ClientProduct>(),
                        Recipes = new List<Recipe>()
                    },
                    new Product()
                    {
                        Id = 2,
                        Name = "Torta2",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs2",
                        Price = 12,
                        MadeInCountry = "Bg2",
                        Clients_Products = new List<ClientProduct>(),
                        Recipes = new List<Recipe>()
                    },
                    new Product()
                    {
                        Id = 3,
                        Name = "Torta3",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs3",
                        Price = 12,
                        MadeInCountry = "Bg3",
                        Clients_Products = new List<ClientProduct>(),
                        Recipes = new List<Recipe>()
                        {
                            new Recipe()
                            {
                                Id = 1,
                                ProductName = "test",
                                Size = 100,
                                Portions = 8,
                                TimeYouNeedToBeMade = 2
                            }
                        }
                    },
                };
            return productList;
        }
    }
}