using Umbraco.Cms.Core.WebAssets;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Assets
{

    public class PropertiesJs : IAssetFile
    {
        public string? FilePath { get; set; } = "/App_Plugins/OrderChildrenByProperty/Settings/Properties.controller.js";
        public AssetType DependencyType { get; } = AssetType.Javascript;
    }
}
