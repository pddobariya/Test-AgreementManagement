using AgreementManagement.Domain;
using AgreementManagement.Models.Agreements;
using AgreementManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace AgreementManagement.Factories
{

    public interface IAgreementFactory
    {
        Task<AgreementModel> PrepareAgreementModel(AgreementModel model, Agreement agreement, string userId);

        Task<(int recordsTotal,IList<AgreementModel> agreementModels)> PrepareAgreementModelsList(AgreementSearchModel agreementSearchModel);
    }

    public class AgreementFactory : IAgreementFactory
    {
        #region Fields
        private readonly IProductGroupService _productGroupService;
        private readonly IProductService _productService;
        private readonly IAgreementService _agreementService;
        #endregion

        #region Ctor
        public AgreementFactory(IProductGroupService productGroupService,
            IProductService productService,
            IAgreementService agreementService)
        {
            _productGroupService = productGroupService;
            _productService = productService;
            _agreementService = agreementService;
        }

        
        #endregion

        #region Utilities
        private async Task<List<SelectListItem>> PrepareProductGroupList(int selectedId =0)
        {
            var pGroups = await _productGroupService.GetProductGroups();

            var availableGrouo = new List<SelectListItem>();
            availableGrouo.Add(new SelectListItem()
            {
                Text = "--SELECT--",
                Value = ""
            });
            foreach (var pGroup in pGroups)
            {
                availableGrouo.Add(new SelectListItem()
                {
                    Text = pGroup.GroupCode,
                    Value = pGroup.Id.ToString(),
                    Selected = pGroup.Id == selectedId
                });
            }
            return availableGrouo;
        }

        private async Task<List<SelectListItem>> PrepareProductList(int groupId,int selectedId = 0)
        {
            var products = await _productService.GetProductsByGroupId(groupId);

            var availableproduct = new List<SelectListItem>();
            availableproduct.Add(new SelectListItem()
            {
                Text = "--SELECT--",
                Value = ""
            });
            foreach (var product in products)
            {
                availableproduct.Add(new SelectListItem()
                {
                    Text = product.ProductNumber,
                    Value = product.Id.ToString(),
                    Selected = product.Id == selectedId
                });
            }
            return availableproduct;
        }

        
        #endregion

        #region Methods
        public async Task<AgreementModel> PrepareAgreementModel(AgreementModel model, Agreement agreement,string userId)
        {
            

            if (agreement == null)
            {
                if (model == null)
                    model = new AgreementModel();
                model.EffectiveDate = DateTime.Now;
                model.ExpirationDate = DateTime.Now;
                model.AvailableProductGroup = await PrepareProductGroupList();
                model.AvailableProduct= await PrepareProductList(0);

            }
            else
            {
                model = new AgreementModel()
                {
                    Id = agreement.Id,
                    UserId = agreement.UserId,
                    ProductGroupId = agreement.ProductGroupId,
                    ProductId = agreement.ProductId,
                    EffectiveDate = agreement.EffectiveDate,
                    ExpirationDate = agreement.ExpirationDate,
                    NewPrice = agreement.NewPrice,
                    Active = agreement.Active
                };

                model.AvailableProductGroup = await PrepareProductGroupList(agreement.ProductGroupId);
                model.AvailableProduct = await PrepareProductList(agreement.ProductGroupId, agreement.ProductId);
            }

            return model;
        }

        public async Task<(int recordsTotal, IList<AgreementModel> agreementModels)> PrepareAgreementModelsList(AgreementSearchModel agreementSearchModel)
        {
            var data = await _agreementService.GetAgreementByUserId(agreementSearchModel);

            var agreementModel = new List<AgreementModel>();
            foreach (var agreement in data.Agreement)
            {
                var product = await _productService.GetProductsById(agreement.ProductId);
                var productGroup = await _productGroupService.GetProductGroupById(agreement.ProductGroupId);
                agreementModel.Add(new AgreementModel() {
                    Id = agreement.Id,
                    UserId = agreement.UserId,
                    ProductGroupId = agreement.ProductGroupId,
                    ProductId = agreement.ProductId,
                    EffectiveDate = agreement.EffectiveDate,
                    ExpirationDate = agreement.ExpirationDate,
                    NewPrice = agreement.NewPrice,
                    ProductPrice = agreement.ProductPrice,
                    Active = agreement.Active,

                    ProductName = product.ProductNumber,
                    ProductGroupName = productGroup.GroupCode
                });
            }
            return (data.recordsTotal, agreementModel);
        }
        #endregion
    }
}
