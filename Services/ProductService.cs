using AgreementManagement.Data;
using AgreementManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgreementManagement.Services
{
    public interface IProductService
    {
        Task<IList<Product>> GetProductsByGroupId(int groupId);
        Task<Product> GetProductsById(int id);
    }

    public class ProductService : IProductService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _productEntity;
        #endregion

        #region Ctor
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
            _productEntity = _context.Set<Product>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get product by group id
        /// </summary>
        /// <param name="groupId">Group identity</param>
        /// <returns>List of product</returns>
        public async Task<IList<Product>> GetProductsByGroupId(int groupId)
        {
            return await _productEntity.Where(p => p.ProductGroupId == groupId).ToListAsync();
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id">product identity</param>
        /// <returns>Product entity</returns>
        public async Task<Product> GetProductsById(int id)
        {
            return await _productEntity.FirstOrDefaultAsync(p => p.Id == id);
        }

        #endregion
    }
}
