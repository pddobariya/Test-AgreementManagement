using AgreementManagement.Data;
using AgreementManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgreementManagement.Services
{
    public interface IProductGroupService
    {
        Task<IList<ProductGroup>> GetProductGroups();
        Task<ProductGroup> GetProductGroupById(int id);
    }
    public class ProductGroupService : IProductGroupService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ProductGroup> _productGroupEntity;
        #endregion

        #region Ctor
        public ProductGroupService(ApplicationDbContext context)
        {
            _context = context;
            _productGroupEntity = _context.Set<ProductGroup>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all group list
        /// </summary>
        /// <returns>List of product group</returns>
        public async Task<IList<ProductGroup>> GetProductGroups()
        {
            return await _productGroupEntity.ToListAsync();
        }

        /// <summary>
        /// Get product group by od
        /// </summary>
        /// <param name="id">product group identity</param>
        /// <returns>product group entity</returns>
        public async Task<ProductGroup> GetProductGroupById(int id)
        {
            if (id == 0)
                return null;

            return await _productGroupEntity.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

    }
}
