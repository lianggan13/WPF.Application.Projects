using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SmartParking.Client.Common;
using SmartParking.Client.Modules.Views;

namespace SmartParking.Client.Modules
{
    public class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Register View to Region
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(SystemString.LeftMenuTreeRegion, typeof(TreeMenuView));
            regionManager.RegisterViewWithRegion(SystemString.MainHeaderRegion, typeof(MainHeaderView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<TreeMenuView>();
            containerRegistry.Register<MainHeaderView>();

            containerRegistry.RegisterForNavigation<UserManagementView>();
            containerRegistry.RegisterDialog<UserModifyDialog>();
        }
    }
}
