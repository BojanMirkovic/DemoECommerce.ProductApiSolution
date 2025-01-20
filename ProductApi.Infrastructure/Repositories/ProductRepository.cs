

using eCommerce.SharedLibrary.LogFolder;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.EntityModels;
using ProductApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(ProductModel entity)
        {
            try
            {
                //check if the product slready exist
                var getProduct = await GetByAsync(product => product.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} is already added");
                }

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();

                if (currentEntity is not null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} product successfuly added to database.");
                }
                else 
                {
                    return new Response(false, $"Error occured while adding {entity.Name} to database.");
                }

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                return new Response(false, "Error occurred adding new product.");
            }
        }

        public async Task<Response> DeleteAsync(ProductModel entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, $"{entity.Name} not found");
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is deleted successfully.");
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                return new Response(false, "Error occurred deleting product.");
            }
        }

        public async Task<ProductModel?> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product;
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                throw new Exception("Error occured retriving product");
            }
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                throw new Exception("Error occured while retriving all products");
            }
        }

        public async Task<ProductModel?> GetByAsync(Expression<Func<ProductModel, bool>> predicate)
        {
            try
            {
                var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(predicate);
                return product;
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                throw new Exception("An error occurred while retrieving the product.");
            }
        }

        public async Task<Response> UpadteAsync(ProductModel entity)
        {
            try
            {
                var existingProduct = await FindByIdAsync(entity.Id);
                if (existingProduct is null) 
                {
                    return new Response(false, $"{entity.Name} not found");
                }

                context.Entry(existingProduct).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is updated successfully.");

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                // Display a user-friendly error message
                return new Response(false,"An error occurred while updating the existing product.");
            }
        }
    }
}
