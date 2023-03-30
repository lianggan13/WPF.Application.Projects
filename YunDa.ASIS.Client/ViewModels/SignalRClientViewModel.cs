using Prism.Mvvm;
using Prism.Regions;

namespace YunDa.ASIS.Client.ViewModels
{
    public class SignalRClientViewModel : BindableBase, INavigationAware
    {
        private string _title = "SignalRClientView";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public SignalRClientViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
