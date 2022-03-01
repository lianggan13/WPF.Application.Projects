using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismModule1.Views;

namespace PrismModule1
{
    public class PrismModule1Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // 添加组件 至 对应区域
            var regionManager = containerProvider.Resolve<IRegionManager>();
            //<ContentControl prism:RegionManager.RegionName="ContentRegion" />
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ViewA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}