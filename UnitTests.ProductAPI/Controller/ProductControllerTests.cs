using eCommerce.SharedLibrary.Responses;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.EntityModels;
using ProductApi.Presentation.Controllers;

namespace UnitTests.ProductAPI.Controller
{
    public class ProductControllerTests
    {
        private readonly IProduct productInterface;
        private readonly ProductsController productsController;

        public ProductControllerTests()
        {
            //Set up dependency
            productInterface = A.Fake<IProduct>();

            //Set up system Under Test -SUT
            productsController = new ProductsController(productInterface);
        }

        //GET ALL PRODUCTS TEST
        [Fact]
        public async Task GetAllProducts_WhenProductExist_ReturnOkResponseWithProducts()
        {
            //Arange
            var products = new List<ProductModel>()
            { 
                new() {Id = 1, Name = "Product 1", Quantity = 10, Price = 100.7m },
                new() {Id = 2, Name = "Product 2", Quantity = 100, Price = 1040.7m }
            };

            //Set up fake response for GetAllAsync method
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            //Act
            var result = await productsController.GetProducts();

            //Asert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductDTO>;
            returnedProducts.Should().NotBeNull();
            returnedProducts.Should().HaveCount(2);
            returnedProducts.First().Id.Should().Be(1);
            returnedProducts.Last().Id.Should().Be(2);

        }
        [Fact]
        public async Task GetAllProducs_WhenNoProductsExist_ReturnNotFoundResponse()
        {
            //Arrange
            var products = new List<ProductModel>();

            //Set up fake response for GetAllAsync()
            A.CallTo(()=> productInterface.GetAllAsync()).Returns(products);

            //Act
            var result = await productsController.GetProducts();

            //Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = notFoundResult.Value as string;
            message.Should().Be("No product detected in the database");
        }

        //CREATE PRODUCT TESTS
        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            //Arrange
            var productDTO = new ProductDTO(1,"product 1",34,67.8m);
            productsController.ModelState.AddModelError("Name", "Required");

            //Act
            var result = await productsController.CreateProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async Task CretaeProduct_WhenCreateIsSuccessful_ReturnOkResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);
         
            var response = new Response(true,"Created");
            //Act
            A.CallTo(()=> productInterface.CreateAsync(A<ProductModel>.Ignored)).Returns(response);

            var result = await productsController.CreateProduct(productDTO);
            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as string;
            responseResult!.Should().Be(response.Message);    
        }
        [Fact]
        public async Task CreateProduct_WhenCreateFaild_ReturnBadRequestResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);

            var response = new Response(false, "Faild");
            //Act
            A.CallTo(() => productInterface.CreateAsync(A<ProductModel>.Ignored)).Returns(response);

            var result = await productsController.CreateProduct(productDTO);
            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as string;
            responseResult!.Should().Be(response.Message);
        }

        //UPDATE PRODUCT TESTS
        [Fact]
        public async Task UpdateProduct_WhenUpdateIsSuccessful_ReturnOkResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);
            var response = new Response(true, "Success");

            //Act
            A.CallTo(()=> productInterface.UpadteAsync(A<ProductModel>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as string;
            responseResult!.Should().Be(response.Message);
        }
        [Fact]
        public async Task UpdateProduct_WhenUpdateFaild_ReturnNotFoundResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);
            var response = new Response(false, "Faild");

            //Act
            A.CallTo(() => productInterface.UpadteAsync(A<ProductModel>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            //Assert
            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var responseResult = notFound.Value as string;
            responseResult!.Should().Be("Faild");
        }

        //DELETE PRODUCT TESTS
        [Fact]
        public async Task DeleteProduct_WhenDeleteIsSuccessful_ReturnOkResultResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);
            var response = new Response(true, "Success");

            //Act
            A.CallTo(()=> productInterface.DeleteAsync(A<ProductModel>.Ignored)).Returns(response);
            var result = await productsController.DeleteProduct(productDTO);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as string;
            responseResult!.Should().Be(response.Message);
        }
        [Fact]
        public async Task DeleteProduct_WhenDeleteFaild_ReturnNotFoundResponse()
        {
            //Arrange
            var productDTO = new ProductDTO(1, "product 1", 34, 67.8m);
            var response = new Response(false, "Faild");

            //Act
            A.CallTo(()=> productInterface.DeleteAsync(A<ProductModel>.Ignored)).Returns(response);
            var result = await productsController.DeleteProduct(productDTO);

            //Assert
            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var responseResult = notFound.Value as string;
            responseResult!.Should().Be("Faild");
        }
    }
}
