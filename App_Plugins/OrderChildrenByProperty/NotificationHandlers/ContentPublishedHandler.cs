using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Services;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.NotificationHandlers
{
    public class ContentPublishedHandler(IContentService contentService, IOrderService orderService) : INotificationHandler<ContentPublishedNotification>
    {
        public void Handle(ContentPublishedNotification notification)
        {
            foreach (var content in notification.PublishedEntities)
            {
                var message = orderService.SortChildren(content);
                if (message != null)
                {
                    notification.Messages.Add(message);
                }
            }

            var parentIds = notification.PublishedEntities.Select(x => x.ParentId).Distinct();
            foreach (var parentId in parentIds)
            {
                //check if item has valid parent
                if (parentId > 0)
                {
                    var parent = contentService.GetById(parentId);
                    var message = orderService.SortChildren(parent);
                    if (message != null)
                    {
                        notification.Messages.Add(message);
                    }
                }
            }
        }
    }
}
