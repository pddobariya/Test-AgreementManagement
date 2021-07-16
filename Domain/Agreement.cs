using System;
using System.ComponentModel.DataAnnotations;

namespace AgreementManagement.Domain
{
    /// <summary>
    /// User Agreement entity
    /// </summary>
    public class Agreement
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductGroupId { get; set; }
        public int ProductId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal NewPrice { get; set; }
        public bool Active { get; set; }
    }
}
