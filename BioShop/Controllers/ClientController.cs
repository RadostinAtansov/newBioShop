﻿namespace BioShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.ClientModels;
    using BioShop.Data.ViewModels.ProductModels;

    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet("get-client-by/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var result = await clientService.GetClientByIdFromDb(id);

            return Ok(result);
        }

        [HttpPost("add-client-to-shop")]
        public async Task<IActionResult> AddClientToShop([FromBody] AddClientToShopViewModel client)
        {
            ArgumentNullException.ThrowIfNull(client);

            var newClient = await clientService.AddClient(client);

            return Ok(newClient);
        }

        [HttpPost("add-product-to-client/{id}")]
        public async Task<IActionResult> AddProductToClient([FromBody] AddProductViewModel client, int id)
        {
            ArgumentNullException.ThrowIfNull(client);

            await clientService.AddProductToClient(client, id);

            return Ok();
        }

        [HttpPut("remove-product-from-client/{clientId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromClient(int clientId, int productId)
        {
            await clientService.RemoveProductFromClient(clientId, productId);

            return Ok();
        }

        [HttpGet("view-all-client-products-by-id/{id}")]
        public async Task<IActionResult> ViewAllClientProducts(int id)
        {
            var allProducts = await clientService.ViewAllClientProducts(id);

            return Ok(allProducts);
        }
    }
}