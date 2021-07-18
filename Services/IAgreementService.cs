using AgreementManagement.Domain;
using AgreementManagement.Models.Agreements;
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

        Task<(int recordsTotal, IList<Agreement> Agreement)> GetAgreementByUserId(AgreementSearchModel agreementSearchModel);
    }
}
