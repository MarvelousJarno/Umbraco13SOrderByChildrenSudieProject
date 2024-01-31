using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.NotificationHandlers;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Composers
{
    public class RegisterNotificationHandlers : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ContentPublishedNotification, ContentPublishedHandler>();
        }
    }
}
