using AgreementManagement.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgreementManagement.Data
{
    public class AgreementMap
    {
        public AgreementMap(EntityTypeBuilder<Agreement> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserId);
            entityBuilder.Property(t => t.ProductGroupId);
            entityBuilder.Property(t => t.ProductId);
            entityBuilder.Property(t => t.EffectiveDate);
            entityBuilder.Property(t => t.ExpirationDate);
            entityBuilder.Property(t => t.ProductPrice);
            entityBuilder.Property(t => t.NewPrice);
        }
    }
}

