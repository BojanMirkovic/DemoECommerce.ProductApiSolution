using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.EntityModels;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No product detected in the database");
            }

            var ( _ , list) = ProductConversions.FromEntity(null!, products);
          
            return list!.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet]
        [Route("products/{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var getProduct = await productInterface.FindByIdAsync(id);
            if (getProduct == null) { return NotFound("Product requested not found");}

            var (product, _ ) = ProductConversions.FromEntity(getProduct, null!);
            return product is not null ? Ok(product) : NotFound("Product not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity); ;
            return response.SuccessFlag ? Ok(response.Message) : NotFound(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //convert to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpadteAsync(getEntity); ;
            return response.SuccessFlag is true ? Ok(response.Message) : NotFound(response.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.SuccessFlag is true ? Ok(response.Message) : NotFound(response.Message);
        }
    }
}
