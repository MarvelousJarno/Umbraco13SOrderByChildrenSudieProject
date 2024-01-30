using Umbraco.Cms.Core.WebAssets;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Assets
{

    public class EditorJs : IAssetFile
    {
        public string? FilePath { get; set; } = "/App_Plugins/OrderChildrenByProperty/Editor.controller.js";
        public AssetType DependencyType { get; } = AssetType.Javascript;
    }
}
