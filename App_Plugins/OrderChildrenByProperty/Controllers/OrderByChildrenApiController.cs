using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            var currentPage = contentService.GetById(currentPageId);
            var currentPageProp = currentPage?.Properties.FirstOrDefault(x => x.PropertyType.PropertyEditorAlias == Global.OrderChildrenByPropertyAlias);
            if (currentPageProp != null)
            {
                var dt = dataTypeService.GetDataType(currentPageProp.PropertyType.DataTypeId);
                var config = dt?.ConfigurationAs<OrderChildrenByPropertyConfiguration>();
                var jToken = config?.Properties as JToken;
                var dic = new Dictionary<string, string>
                {
                    { "Name", "Name" },
                    { "CreateDate", "Create date" },
                    { "PublishDate", "Publish date" }
                };

                if (jToken == null)
                {
                    return new JsonResult(dic);
                }

                foreach (var o in jToken.Children<JObject>())
                {
                    foreach (var p in o.Properties())
                    {
                        var value = (string)p.Value;
                        if (!dic.Keys.Contains(value))
                        {
                            dic.Add(value, value);
                        }
                    }
                }

                return new JsonResult(dic);
            }
            return new JsonResult("No properties found");
        }
    }
}
