using AgreementManagement.Data;
using AgreementManagement.Domain;
using AgreementManagement.Models.Agreements;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgreementManagement.Services
{
    public class AgreementService : IAgreementService
    {
        #region Field
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Agreement> _agreementEntity;
        #endregion

        #region Ctor
        public AgreementService(ApplicationDbContext context)
        {
            _context = context;
            _agreementEntity = _context.Set<Agreement>();
        }
        #endregion

        #region Methods
        public virtual async Task InsertAgreement(Agreement agreement)
        {
            await _agreementEntity.AddAsync(agreement);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAgreement(Agreement agreement)
        {
            _context.Update<Agreement>(agreement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAgreement(Agreement agreement)
        {
            _agreementEntity.Remove(agreement);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<Agreement> GetAgreementById(int id)
        {
            if (id == 0)
                return null;
            
            return await _agreementEntity.FirstOrDefaultAsync(p => p.Id == id);
        }

        public virtual async Task<List<Agreement>> GetAgreementByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            return await _agreementEntity.Where(p => p.UserId == userId).ToListAsync();
        }
        #endregion
    }
}
