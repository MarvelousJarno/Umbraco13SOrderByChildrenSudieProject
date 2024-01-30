using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Umbraco13StudieProject.App_Plugins.OrderChildrenByProperty.Editor
{

    public class OrderChildrenByPropertyConfigurationEditor(
        IIOHelper ioHelper,
        IEditorConfigurationParser editorConfigurationParser)
        : ConfigurationEditor<OrderChildrenByPropertyConfiguration>(ioHelper, editorConfigurationParser);
}

