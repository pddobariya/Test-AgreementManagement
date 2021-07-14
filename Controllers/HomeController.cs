using AgreementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AgreementManagement.Factories;
using AgreementManagement.Models.Agreements;
using AgreementManagement.Domain;
using AgreementManagement.Services;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Razor;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AgreementManagement.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAgreementFactory _agreementFactory;
        private readonly IProductService _productService;
        private readonly IAgreementService _agreementService;
        private readonly IRazorViewEngine _razorViewEngine;
        #endregion


        #region Ctor
        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            IAgreementFactory agreementFactory,
            IProductService productService,
            IAgreementService agreementService,
            IRazorViewEngine razorViewEngine)
        {
            _logger = logger;
            _userManager = userManager;
            _agreementFactory = agreementFactory;
            _productService = productService;
            _agreementService = agreementService;
            _razorViewEngine = razorViewEngine;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        protected virtual async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            //create action context
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

            //set view name as action name in case if not passed
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            //set model
            ViewData.Model = model;

            //try to get a view by the name
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
            if (viewResult.View == null)
            {
                //or try to get a view by the path
                viewResult = _razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }
            await using var stringWriter = new StringWriter();
            var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.GetStringBuilder().ToString();
        }

        public async Task<JsonResult> GetProductListByGroupId(int groupId)
        {
            var products = await _productService.GetProductsByGroupId(groupId);
            var availableproduct = new List<SelectListItem>();
            availableproduct.Add(new SelectListItem()
            {
                Text = "--SELECT--",
                Value = "0"
            });

            foreach (var product in products)
            {
                availableproduct.Add(new SelectListItem()
                {
                    Text = product.ProductNumber,
                    Value = product.Id.ToString(),
                });
            }

            return Json(new { availableproduct = availableproduct });
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AgreementList()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await _agreementFactory.PrepareAgreementModelsList(user.Id.ToString());
            var jsonData = new
            {
                data = model
            };
            return Json(jsonData);
        }

        public async Task<IActionResult> NewAgreement()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = await _agreementFactory.PrepareAgreementModel(null, null, user.Id.ToString());

            return PartialView("_NewAgreement", model);
            //return Json(new{html = await RenderPartialViewToStringAsync("_NewAgreement", model) });

        }

        [HttpPost]
        public async Task<IActionResult> NewAgreement(AgreementModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var product = await _productService.GetProductsById(model.ProductId);
                var agreement = new Agreement()
                {
                    UserId = user.Id.ToString(),
                    ProductGroupId = model.ProductGroupId,
                    ProductId = model.ProductId,
                    EffectiveDate = model.EffectiveDate,
                    ExpirationDate = model.ExpirationDate,
                    ProductPrice = product != null ? product.Price : 0,
                    NewPrice = model.NewPrice
                };
                await _agreementService.InsertAgreement(agreement);
            }

            model = await _agreementFactory.PrepareAgreementModel(model, null, user.Id.ToString());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var agreement = await _agreementService.GetAgreementById(Id);
            if (agreement != null)
            {
                await _agreementService.DeleteAgreement(agreement);
            }
            return Json(new { data = agreement });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
