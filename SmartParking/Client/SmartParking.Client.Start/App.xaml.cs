using System.Windows;
using System.Windows.Threading;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using SmartParking.Client.BLL;
using SmartParking.Client.DAL;
using SmartParking.Client.Modules;
using SmartParking.Client.Start.Views;

namespace SmartParking.Client.Start
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override void InitializeShell(Window shell)
        {

            if (Container.Resolve<LoginView>().ShowDialog() == true)
            {
                base.InitializeShell(shell);
            }
            else
            {
                Application.Current?.Shutdown();

            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IUserDal, UserDal>();
            containerRegistry.Register<IUserBll, UserBll>();

            containerRegistry.Register<IMenuDal, MenuDal>();
            containerRegistry.Register<IMenuBll, MenuBll>();

            containerRegistry.Register<Dispatcher>(() => Application.Current.Dispatcher);

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
        }
    }
}
