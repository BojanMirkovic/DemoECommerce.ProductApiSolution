
using ProductApi.Domain.EntityModels;

namespace ProductApi.Application.DTOs.Conversions
{/*Becouse this is Demo project and we do not have many Models, we can implement custom made conversions to and from Entity
   If the product is biger with more Models we should use autoMapper Nuget package */
    public static class ProductConversions
    {
        public static ProductModel ToEntity(ProductDTO product) => new()
        { 
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(ProductModel product, IEnumerable<ProductModel>? products)
        {
            //Return sinle product
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDTO
                    (
                      product!.Id,
                      product.Name!,
                      product.Quantity,
                      product.Price
                    );
                return (singleProduct, null);
            }

            //Return list of products
            if (products is not null || product is null)
            {
                var listOfProducts = products!.Select(p =>
                new ProductDTO(p.Id, p.Name!, p.Quantity, p.Price)).ToList();
                return(null, listOfProducts);
            }

            return (null, null);
        }
    }
}
