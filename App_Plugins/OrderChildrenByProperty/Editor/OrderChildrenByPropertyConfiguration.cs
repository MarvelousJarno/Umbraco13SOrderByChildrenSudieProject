using Umbraco.Cms.Core.PropertyEditors;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor
{
    public class OrderChildrenByPropertyConfiguration
    {
        [ConfigurationField("properties", "Custom properties", "/App_Plugins/OrderChildrenByProperty/Settings/Properties.html",
            Description = "Configure the block types available to the user.")]
        public object Properties { get; set; }
    }
}
