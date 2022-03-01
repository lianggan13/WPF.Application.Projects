using Prism.Ioc;
using Prism.Mvvm;
using SmartParking.Client.Model;

namespace SmartParking.Client.Modules.ViewModels
{
    public class MainHeaderViewModel : BindableBase
    {
        public string CurrentUserName { get; private set; }

        public MainHeaderViewModel(IContainerProvider containerProvider)
        {
            if (GlobalInfo.CurrentUser != null)
            {
                CurrentUserName = GlobalInfo.CurrentUser.nickname;

            }
        }

    }
}
