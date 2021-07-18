using AgreementManagement.Data;
using AgreementManagement.Domain;
using AgreementManagement.Models.Agreements;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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

        public virtual async Task<(int recordsTotal, IList<Agreement> Agreement)> GetAgreementByUserId(AgreementSearchModel agreementSearchModel)
        {
            if (string.IsNullOrEmpty(agreementSearchModel.UserId))
                return (0,null);

            try
            {
                var pUserId = new SqlParameter("@USERId", agreementSearchModel.UserId);
                var pSearchValue = new SqlParameter("@SearchValue", agreementSearchModel.SearchValue);
                var pSortColumn = new SqlParameter("@SortColumn", agreementSearchModel.SortColumn);
                var pSortColumnDirection = new SqlParameter("@SortColumnDirection", agreementSearchModel.SortColumnDirection);
                var pSkip = new SqlParameter("@Skip", agreementSearchModel.Skip);
                var pPageSize = new SqlParameter("@PageSize", agreementSearchModel.PageSize);

                var pTotalRecords = new SqlParameter
                {
                    ParameterName = "totalRecords",
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output
                };

                var agreements = await _agreementEntity
                            .FromSqlRaw("exec SearchAgreement @USERId, @SearchValue,@SortColumn,@SortColumnDirection,@Skip,@PageSize, @totalRecords OUT",
                            pUserId, pSearchValue, pSortColumn, pSortColumnDirection, pSkip, pPageSize, pTotalRecords)
                            .ToListAsync();

                var totalRecords = (int)pTotalRecords.Value;
                
                return (totalRecords, agreements);
            }
            catch (Exception ex)
            {
                return (0,null);
            }
        }
        #endregion
    }
}
