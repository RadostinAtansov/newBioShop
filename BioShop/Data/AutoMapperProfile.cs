namespace BioShop.Data
{
    using AutoMapper;
    using BioShop.Data.Models;
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModel;
    using BioShop.Data.ViewModels.ProductModels;
    using BioShop.Data.ViewModels.RecipeModel;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Client
            CreateMap<AddClientToShopViewModel, Client>();
            CreateMap<Client, AddClientToShopViewModel>();
            CreateMap<Client, GetClientIdViewModel>();
            CreateMap<Client, GetClientAndAllHisProductsViewModel>();

            //Product
            CreateMap<AllRecipesProductViewModel, Product>();
            CreateMap<Product, AllRecipesProductViewModel>();

            //Recipe

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, GetClientAndAllHisProductsViewModel>();
                cfg.CreateMap<ClientProduct, AllRecipesProductViewModel>();
            });
        }
    }
}