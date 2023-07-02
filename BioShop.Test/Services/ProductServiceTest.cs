namespace BioShop.Test.Services
{
    using Moq;
    using Xunit;
    using BioShop.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.ProductModels;
    using BioShop.Data.ViewModels.RecipeModel;

    public class ProductServiceTest
    {
        private readonly Mock<IProductService> _productService;

        public ProductServiceTest()
        {
            _productService = new Mock<IProductService>();
        }

        [Fact]
        public async Task AddProductToDatabaseAndReturnCorrectResultAllProducts()
        {
            //Arrange
            var productList = await ProductData();
            int productListCount = 3;
            var fakeProduct = new AllRecipesProductViewModel()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 44,
                MadeInCountry = "Bg4",
                RecipesProduct = new List<AllRecipesOnProductViewModel>(),

            };
            _productService.Setup(x => x.AddProduct(fakeProduct))
                .Callback(() =>
                {
                    productList.Add(fakeProduct);
                    productListCount++;
                })
                .ReturnsAsync(productList);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.AddProductToShop(fakeProduct);
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
            AllRecipesProductViewModel fakeProduct = null;
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
            _productService.Setup(x => x.GetAllProducts())
                .ReturnsAsync(productList);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetAllProducts();
            var productResultModel = ((ObjectResult)productResult).Value as List<AllRecipesProductViewModel>;

            //Assert
            Assert.Equal(productList, productResultModel);
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
            _productService.Setup(x => x.GetProductByIdAndAllHisRecipes(productId))
                .Callback(() =>
                {
                    var product = productList.Find(x => x.Id == productId);
                })
                .ReturnsAsync(productList[2]);
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.GetProductByIdAndAllHisRecipes(productId);
            var productResultModul = ((ObjectResult)productResult).Value as AllRecipesProductViewModel;

            //Assert
            Assert.Equal(productId, productList[2].Id);
            Assert.True(productList[2].RecipesProduct.Count() > 0);
        }

        [Fact]
        public async Task UpdateProductIfProductIdDoesNotExistThrowArgumentNullException()
        {
            //Arrange
            var productList = await ProductData();
            int productId = 0;
            var fakeProduct = new AllRecipesProductViewModel()
            {
                Id = 4,
                Name = "Torta4",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco4, Milk4, Eggs4",
                Price = 44,
                MadeInCountry = "Bg4",
                RecipesProduct = new List<AllRecipesOnProductViewModel>(),

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
            var fakeProduct = new AllRecipesProductViewModel()
            {
                Id = 111,
                Name = "Torta111",
                Expires = DateTime.Now.AddDays(31),
                Ingredients = "Choco111, Milk111, Eggs111",
                Price = 111,
                MadeInCountry = "Bg111",
                RecipesProduct = new List<AllRecipesOnProductViewModel>(),

            };
            _productService.Setup(x => x.UpdateProduct(productId, fakeProduct))
                .Callback(() =>
                {
                    productList[0] = fakeProduct;
                });
            var productController = new ProductController(_productService.Object);

            //Act
            var productResult = await productController.UpdateProduct(productId, fakeProduct);
            
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

        private async Task<List<AllRecipesProductViewModel>> ProductData()
        {
            List<AllRecipesProductViewModel> productList = new List<AllRecipesProductViewModel>()
                {
                    new AllRecipesProductViewModel()
                    {
                        Id = 1,
                        Name = "Torta1",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs1",
                        Price = 12,
                        MadeInCountry = "Bg1",
                        RecipesProduct = new List<AllRecipesOnProductViewModel>(),

                    },
                    new AllRecipesProductViewModel()
                    {
                        Id = 2,
                        Name = "Torta2",
                        Expires = DateTime.Now.AddDays(31),
                        Ingredients = "Choco, Milk, Eggs2",
                        Price = 12,
                        MadeInCountry = "Bg2",
                        RecipesProduct = new List<AllRecipesOnProductViewModel>(),
                    },
                    new AllRecipesProductViewModel()
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
                                RecipeName = "IceCream"
                            },
                            new AllRecipesOnProductViewModel()
                            {
                                Id = 2,
                                RecipeName = "Cake"
                            },
                            new AllRecipesOnProductViewModel()
                            {
                                Id = 3,
                                RecipeName = "Chocolate"
                            },
                        },
                    },
                };
            return productList;
        }

    }
}