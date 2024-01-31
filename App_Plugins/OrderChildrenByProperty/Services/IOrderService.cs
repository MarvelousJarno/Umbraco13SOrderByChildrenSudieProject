using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Services
{
    public interface IOrderService
    {
        EventMessage? SortChildren(IContent? content);
    }
}
