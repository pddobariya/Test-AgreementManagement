using AgreementManagement.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgreementManagement.Data
{
    public class ProductMap
    {
        public ProductMap(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.ProductGroupId);
            entityBuilder.Property(t => t.ProductDescription);
            entityBuilder.Property(t => t.ProductNumber);
            entityBuilder.Property(t => t.Price);
            entityBuilder.Property(t => t.Active);
        }
    }
}
