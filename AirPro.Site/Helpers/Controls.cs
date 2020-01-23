using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Entities.Repair;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Notes;
using AirPro.Site.Models.Shared;
using AirPro.Site.Models.Uploads;
using Microsoft.AspNet.Identity;

namespace AirPro.Site.Helpers
{
    public static class Controls
    {
        public static string UploadControl(this HtmlHelper helper, UploadType type, string key, string title)
        {
            var model = new UploadControlViewModel
            {
                Type = (int)type,
                Key = key
            };

            if (!string.IsNullOrEmpty(title))
                model.Title = title;

            helper.RenderPartial("_UploadControl", model);

            return null;
        }

        public static MvcHtmlString ShopMembershipSelectionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool canEdit = true)
        {
            // Get Field Data.
            var field = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Load Field Values.
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as IList<Guid>;

            // Load Factory.
            var user = helper.ViewContext.HttpContext.User.Identity;
            var factory = new ServiceFactory(MvcApplication.ConnectionString, user);

            // Load Model.
            var model = new MembershipControlViewModel
            {
                PropertyName = field.PropertyName,
                MembershipTypeName = "Shop",
                SelectedItems = fieldValue,
                SelectionList = factory.GetDisplayList<IShopDto>(),
                CanEdit = canEdit
            };

            // Render Control.
            return helper.Partial("_MembershipControl", model);
        }

        public static MvcHtmlString AccountMembershipSelectionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool canEdit = true)
        {
            // Get Field Data.
            var field = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Load Field Values.
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as IList<Guid>;

            // Load Factory.
            var user = helper.ViewContext.HttpContext.User.Identity;
            var factory = new ServiceFactory(MvcApplication.ConnectionString, user);

            // Load Model.
            var model = new MembershipControlViewModel
            {
                PropertyName = field.PropertyName,
                MembershipTypeName = "Account",
                SelectedItems = fieldValue,
                SelectionList = factory.GetDisplayList<IAccountDto>(),
                CanEdit = canEdit
            };

            // Render Control.
            return helper.Partial("_MembershipControl", model);
        }

        public static MvcHtmlString RequestTypeFor<TModel>(this HtmlHelper<TModel> helper
            , int? requestTypeCategoryId
            , int requestTypeId
            , int orderId)
        {

            using (var db = new EntityDbContext())
            {
                // Load Request Types.
                var repair = db.RepairOrders.Where(x => x.OrderId == orderId).Select(x => new {
                    shopGuid = x.Shop.ShopGuid
                }).First();

                var previousRequestTypes = db.ScanRequests
                    .Where(r => r.OrderId == orderId && r.Report != null && r.Report.CompletedInd == true)
                    .Select(r => r.RequestTypeId).ToArray();
                var requestCategoryTypes = db.ScanRequestCategoryTypes.ToList();
                var categories = db.ScanRequestCategories.ToList().Select(x => new KeyValuePair<string, string>(x.RequestCategoryId.ToString(), x.RequestCategoryName)).ToList();

                // Load Model.
                var model = new RequestTypeControlViewModel
                {
                    RequestTypeCategoryId = requestTypeCategoryId,
                    RequestTypeId = requestTypeId,
                    RequestTypeCategories = categories,
                    RequestTypes = GetRequestTypeSelectListItems(previousRequestTypes, repair.shopGuid),
                    RequestCategoryTypes = requestCategoryTypes
                };

                model.RequestCategoryTypes = model.RequestCategoryTypes.Where(x => model.RequestTypes.Any(y => y.Value == x.RequestTypeId.ToString())).ToList();
                categories = model.RequestTypeCategories.Where(x => model.RequestCategoryTypes.Any(y => y.RequestCategoryId.ToString() == x.Key)).ToList();


                if (model.RequestTypes.Any(x => !model.RequestCategoryTypes.Any(y => y.RequestTypeId.ToString() == x.Value)))
                {
                    categories.Add(new KeyValuePair<string, string>("0", "Other"));
                }
                model.RequestTypeCategories = categories;

                // Render Control.
                return helper.Partial("_RequestTypeControl", model);
            }
        }

        private static IEnumerable<SelectListItem> GetRequestTypeSelectListItems(IEnumerable<int> previousRequestTypes, Guid? shopGuid = null)
        {
            // Get Request Types - Exclude Quick and Scan Analysis.
            var requestTypes = SelectListItemCache.RequestTypeSelectItems(shopGuid)
                .Where(t => t.Value != "1").ToList();

            // Filter Followup Scan - Requires Completion or Diagnostic.
            var typesForFollowUpScan = new[] { 2, 3 };
            var result = !previousRequestTypes.Intersect(typesForFollowUpScan).Any()
                ? requestTypes.Where(d => d.Value != "4")
                : requestTypes;

            return result;
        }

        public static MvcHtmlString InsuranceSelectFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string insuranceDataBind = "", string insuranceOtherDataBind = "")
        {
            // Load Field Values.
            var model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as HasInsuranceSelectViewModel;
            
            model.InsuranceDataBind = insuranceDataBind;
            model.InsuranceOtherDataBind = insuranceOtherDataBind;
            // Render Control.
            return helper.Partial("_InsuranceSelectControl", model);
        }

        public static MvcHtmlString ContactSelectFor<TModel>(this HtmlHelper<TModel> helper, string contactUserGuid, string 
contactOtherFirstName, string contactOtherLastName, string contactOtherPhone, int? repairId = 0, Guid? shopGuid = null, int labelBootstrapColumn = 3)
        {

            // Load Factory.
            var user = helper.ViewContext.HttpContext.User.Identity;
            var factory = new ServiceFactory(MvcApplication.ConnectionString, user);
            var serviceArgs = new ServiceArgs {{"RepairId", repairId}, {"ShopGuid", shopGuid ?? Guid.Empty}};
            var contacts = factory.GetDisplayList<IUserDto>(serviceArgs).ToList();
            contacts.Add(new KeyValuePair<string, string>("other", "Other"));

            var model = new ContactSelectViewModel {ContactUserGuid = contactUserGuid};
            if (string.IsNullOrWhiteSpace(model.ContactUserGuid))
            {
                var currentUserId = user.GetUserId();
                model.ContactUserGuid = contacts.Any(x => x.Key == currentUserId) ? currentUserId : null;
            }
            model.ContactOtherFirstName = contactOtherFirstName;
            model.ContactOtherLastName = contactOtherLastName;
            model.ContactOtherPhone = contactOtherPhone;
            model.LabelBootstrapColumn = labelBootstrapColumn;
            model.Contacts = new SelectList(contacts, "Key", "Value");

            // Render Control.
            return helper.Partial("_ContactSelectControl", model);
        }

        public static MvcHtmlString PermissionSelect<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            // Get Field Data.
            var field = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Load Field Values.
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model as ICollection<Guid>;

            var model = new PermissionSelectionViewModel
            {
                FieldName = field.PropertyName,
                PermissionGuids = fieldValue
            };
            // Render Control.
            return helper.Partial("_PermissionSelectionControl", model);
        }

        public static string NoteControl(this HtmlHelper helper, NoteType type, string key, string title, bool isReadOnly = false)
        {
            var model = new NoteControlViewModel
            {
                Type = type,
                Key = key,
                IsReadOnly = isReadOnly
            };

            var identity = helper.ViewContext.HttpContext.User.Identity;
            var factory = new ServiceFactory(MvcApplication.ConnectionString, identity);

            var user = factory.GetById<IUserDto>(identity.GetUserId());
            model.User = user.DisplayName;

            if (!string.IsNullOrEmpty(title))
                model.Title = title;

            helper.RenderPartial("_NoteControl", model);

            return null;
        }
    }
}