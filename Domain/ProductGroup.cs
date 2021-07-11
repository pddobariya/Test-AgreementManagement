using System.ComponentModel.DataAnnotations;

namespace AgreementManagement.Domain
{
    /// <summary>
    /// Product group entity
    /// </summary>
    public class ProductGroup
    {
        [Key]
        public int Id { get; set; }
        public string GroupDescription { get; set; }
        public string GroupCode { get; set; }
        
        public bool Active { get; set; }
    }
}
