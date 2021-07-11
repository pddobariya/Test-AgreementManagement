using System.ComponentModel.DataAnnotations;

namespace AgreementManagement.Domain
{
    /// <summary>
    /// Product entity
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductNumber { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
    }
}
