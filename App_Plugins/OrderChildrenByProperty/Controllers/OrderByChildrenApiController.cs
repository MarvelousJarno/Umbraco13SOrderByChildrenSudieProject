using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Models;

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
                var list = new List<OrderByChildrenProperties>
                {
                    new()
                    {
                        Value = "Name",
                        OrderBy = "ASC",
                        Id = 1
                    },
                    new()
                    {
                        Value = "CreateDate",
                        OrderBy = "DESC",
                        Id = 2
                    },
                    new()
                    {
                        Value = "PublishDate",
                        OrderBy = "DESC",
                        Id = 3
                    }
                };

                var dt = dataTypeService.GetDataType(currentPageProp.PropertyType.DataTypeId);
                var config = dt?.ConfigurationAs<OrderChildrenByPropertyConfiguration>();
                var json = config?.Properties.ToString();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new JsonResult(list);
                }

                var properties = JsonConvert.DeserializeObject<IEnumerable<OrderByChildrenProperties>>(json);
                if (properties == null || !properties.Any())
                {
                    return new JsonResult(list);
                }

                var newId = 4;
                foreach (var property in properties)
                {
                    property.Id = newId;
                    newId++;
                }

                list.AddRange(properties);

                return new JsonResult(list);
            }
            return new JsonResult("No properties found");
        }
    }
}
