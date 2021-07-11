using AgreementManagement.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgreementManagement.Data
{
    public class ProductGroupMap
    {
        public ProductGroupMap(EntityTypeBuilder<ProductGroup> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.GroupDescription);
            entityBuilder.Property(t => t.GroupCode);
            entityBuilder.Property(t => t.Active);
        }
    }
}
