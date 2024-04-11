using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Implement;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Models;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Services
{
    public class OrderService(IContentService contentService, ICoreScopeProvider coreScopeProvider, IDataTypeService dataTypeService) : IOrderService
    {
        public EventMessage? SortChildren(IContent? content)
        {
            var sortProperty = content?.Properties.FirstOrDefault(x =>
                x.PropertyType.PropertyEditorAlias == Global.OrderChildrenByPropertyAlias);

            if (sortProperty == null) return null;

            var value = content.GetValue<string>(sortProperty.PropertyType.Alias); // todo zorgen dat ze id wordt opgeslagen ipv de value

            var children = contentService.GetPagedChildren(content.Id, 0, 9999, out _);
            
            if (children == null || !children.Any())
            {
                return new EventMessage("Sorting", $"No children found for content item {content.Name}", EventMessageType.Warning);
            }

            var childrenHaveOrderProp = children.Any(x =>
                x.Properties.Any(p => p.Alias.ToUpperInvariant() == value?.ToUpperInvariant()));

            switch (value)
            {
                case "Name":
                    children = children.OrderBy(x => x.Name);
                    break;
                case "CreateDate":
                    children = children.OrderByDescending(x => x.CreateDate);
                    break;
                case "PublishDate":
                    children = children.OrderByDescending(x => x.PublishDate);
                    break;
                default:
                    if (childrenHaveOrderProp)
                    {
                        var dataType = dataTypeService.GetDataType(sortProperty.PropertyType.DataTypeId);
                        var config = dataType?.ConfigurationAs<OrderChildrenByPropertyConfiguration>();
                        var json = config?.Properties.ToString();
                        var properties = JsonConvert.DeserializeObject<IEnumerable<OrderByChildrenProperties>>(json);

                        var selectedProp = properties.FirstOrDefault(x => x.Value == value);

                        children = selectedProp.OrderBy == "ASC"
                            ? children
                                .OrderBy(x =>
                                    x.GetValue(value) != null
                                        ? 0
                                        : 1) //do this so items with no property are always last
                                .ThenBy(x => x.GetValue(value))
                            : children
                                .OrderBy(x =>
                                    x.GetValue(value) != null
                                        ? 0
                                        : 1) //do this so items with no property are always last
                                .ThenByDescending(x => x.GetValue(value));
                    }
                    else
                    {
                        return new EventMessage("Sorting", $"No property value found on content item {content.Name} for property {sortProperty.PropertyType.Alias}", EventMessageType.Warning);
                    }
                    break;
            }
            ////use coreScopeProvider so we don't trigger new save or publish notifications
            using var scope = coreScopeProvider.CreateCoreScope(autoComplete: true);
            {
                using (_ = scope.Notifications.Suppress())
                {
                    contentService.Sort(children);
                }
            }
            return new EventMessage("Sorting", $"The children of content item {content.Name} are sorted by {value}", EventMessageType.Success);
        }
    }
}
