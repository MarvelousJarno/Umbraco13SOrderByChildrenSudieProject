using Umbraco.Cms.Core.Composing;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Services;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Composers
{
    public class RegisterDependencies : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<IOrderService, OrderService>();
        }
    }
}
