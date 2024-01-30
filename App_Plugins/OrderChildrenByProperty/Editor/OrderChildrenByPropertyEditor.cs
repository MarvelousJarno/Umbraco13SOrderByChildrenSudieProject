using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor
{
    [DataEditor(
        alias: Global.OrderChildrenByPropertyAlias,
        type: EditorType.PropertyValue,
        name: "Order children by property editor",
        view: "~/App_Plugins/OrderChildrenByProperty/editor.html",
        ValueType = ValueTypes.Json,
        HideLabel = true)]
    public class OrderChildrenByPropertyEditor(IDataValueEditorFactory dataValueEditorFactory)
        : DataEditor(dataValueEditorFactory)
    {
        protected override IConfigurationEditor CreateConfigurationEditor() =>
            new OrderChildrenByPropertyConfigurationEditor(
                StaticServiceProvider.Instance.GetRequiredService<IIOHelper>(),
                StaticServiceProvider.Instance.GetRequiredService<IEditorConfigurationParser>());
    }

}
