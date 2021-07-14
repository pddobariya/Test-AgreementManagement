using AgreementManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgreementManagement.Services
{
    public interface IAgreementService
    {
        Task InsertAgreement(Agreement agreement);

        Task UpdateAgreement(Agreement agreement);

        Task DeleteAgreement(Agreement agreement);

        Task<Agreement> GetAgreementById(int id);

        Task<List<Agreement>> GetAgreementByUserId(string userId);
    }
}
