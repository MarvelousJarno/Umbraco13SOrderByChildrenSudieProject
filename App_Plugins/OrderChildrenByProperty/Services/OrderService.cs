using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Services
{
    public class OrderService(IContentService contentService, ICoreScopeProvider coreScopeProvider) : IOrderService
    {
        //todo kijken of we iets met cultures moeten doen
        //todo testen met verschillende contentypes waarbij de ene de propertie wel heeft en de andere niet
        //todo kijken wat het doet als het content item en de parent de sort propertie heeft
        public EventMessage? SortChildren(IContent? content)
        {
            var sortProperty = content?.Properties.FirstOrDefault(x =>
                x.PropertyType.PropertyEditorAlias == Global.OrderChildrenByPropertyAlias);

            if (sortProperty == null) return null;

            var value = content.GetValue<string>(sortProperty.PropertyType.Alias);

            var children = contentService.GetPagedChildren(content.Id, 0, 9999, out _);

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
                        children = children.OrderBy(x => x.GetValue(value));
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
            return new EventMessage("Sorting", $"The children of {content.Name} are sorted by {value}", EventMessageType.Success);
            //todo deze functie returnen of het is gelukt
        }
    }
}
