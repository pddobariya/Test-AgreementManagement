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

        #endregion
    }
}
