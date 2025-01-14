

using eCommerce.SharedLibrary.LogFolder;
using eCommerce.SharedLibrary.Responses;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.EntityModels;
using ProductApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public Task<Response> CreateAsync(ProductModel entity)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free message to the user
                return new Response(false, "Error occured adding new product.");
                
            }
        }

        public Task<Response> DeleteAsync(ProductModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductModel>> GetaAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetByAsync(Expression<Func<ProductModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpadteAsync(ProductModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
