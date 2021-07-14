using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgreementManagement.Models.Agreements
{
    public class AgreementModel
    {
        public AgreementModel()
        {
            AvailableProductGroup = new List<SelectListItem>();
            AvailableProduct = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Product group is required field")]
        public int ProductGroupId { get; set; }
        public IList<SelectListItem> AvailableProductGroup { get; set; }
        public string ProductGroupName { get; set; }

        [Required(ErrorMessage = "Product is required field")]
        public int ProductId { get; set; }
        public IList<SelectListItem> AvailableProduct { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Effective date is required field")]
        public DateTime EffectiveDate { get; set; }
        [Required(ErrorMessage = "Expiration date is required field")]
        public DateTime ExpirationDate { get; set; }
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "Price date is required field")]
        public decimal NewPrice { get; set; }
    }
}
