using System.Linq;
using Prism.Commands;
using Prism.Regions;
using SmartParking.Client.Common;
using Unity;

namespace SmartParking.Client.Modules.ViewModels
{
    public abstract class ViewModelBase : INavigationAware, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        protected IUnityContainer unityContainer;

        /// <summary>
        /// 区域管理器
        /// </summary>
        protected IRegionManager regionManager;

        public string NavUri { get; set; }
        public virtual string PageTitle { get; set; }
        public bool IsCanClose { get; set; } = true;

        public ViewModelBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// TabItem 关闭
        /// </summary>
        public DelegateCommand<string> CloseCommand
        {
            get => new DelegateCommand<string>(arg =>
            {
                var registration = unityContainer.Registrations.FirstOrDefault(r => r.Name == NavUri);
                string typeName = registration.MappedToType.Name;
                var region = regionManager.Regions[SystemString.MainContentRegion];
                var view = region.Views.FirstOrDefault(v => v.GetType().Name == typeName);
                if (view != null)
                {
                    region.Remove(view);
                }
            });
        }

        /// <summary>
        /// 标记上一个视图是否被销毁
        /// </summary>
        public bool KeepAlive => true;

        public void ConfirmNavigationRequest(NavigationContext navigationContext, System.Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        #region 导航接口方法组

        /// <summary>
        /// 是否使用导航后的目标视图
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 导航前
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        /// <summary>
        /// 导航后
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavUri = navigationContext.Uri.ToString();
        }


        #endregion
    }
}
