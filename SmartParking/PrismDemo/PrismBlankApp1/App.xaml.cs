using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using PrismBlankApp1.Views;
using PrismModule1;

namespace PrismBlankApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<PrismModule1Module>();

        }
    }
}
