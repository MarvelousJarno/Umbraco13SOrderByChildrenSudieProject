using Umbraco.Cms.Core.Composing;
using Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Assets;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Composers
{
    public class Properties : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.BackOfficeAssets().Append<EditorJs>();
            builder.BackOfficeAssets().Append<PropertiesJs>();
        }
    }
}
