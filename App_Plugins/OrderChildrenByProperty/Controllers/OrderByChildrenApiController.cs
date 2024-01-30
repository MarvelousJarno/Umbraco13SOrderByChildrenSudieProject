using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Controllers
{
    [Umbraco.Cms.Web.Common.Attributes.PluginController("GetData")]
    public class OrderByChildrenApiController(IContentService contentService, IDataTypeService dataTypeService) : UmbracoAuthorizedApiController
    {
        public ActionResult GetAllPropertiesOfChildren(int currentPageId)
        {
            var children = contentService.GetPagedChildren(currentPageId, 0, 9999, out _);

            if (children != null && children.Any())
            {
                var currentPage = contentService.GetById(currentPageId);
                var currentPageProp = currentPage.Properties.FirstOrDefault(x => x.PropertyType.PropertyEditorAlias == Global.OrderChildrenByPropertyAlias);
                var dt = dataTypeService.GetDataType(currentPageProp.PropertyType.DataTypeId);
                var config = dt.ConfigurationAs<OrderChildrenByPropertyConfiguration>();
                var properties = config.Properties; //object is JToken. Set deze data naar eigen object en lees dit uit

                ////var pageTypes = children.Count(x => x.ContentType)

                //////todo checken wat we doen als er meerdere pageTypes zijn. Misschien iets van configuratie hiervoor maken?
                ////var properties = 

                var dic = new Dictionary<string, string>
                {
                    { "Name", "Name" },
                    { "CreateDate", "Create date" },
                    { "PublishDate", "Publish date" }
                };

                return new JsonResult(dic);
            }
            return new JsonResult("No children found");
        }
    }
}
