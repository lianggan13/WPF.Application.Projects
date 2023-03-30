using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace YunDa.ASIS.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IRegionManager Region;
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager region)
        {
            Region = region;
        }
        private DelegateCommand<string> _GoCommand;
        public DelegateCommand<string> GoCommand =>
            _GoCommand ?? (_GoCommand = new DelegateCommand<string>(ExecuteGoCommand));

        void ExecuteGoCommand(string uri)
        {
            Region.RequestNavigate("ContentRegion", uri);
        }
        private DelegateCommand<object> _DeleteCommand;
        public DelegateCommand<object> DeleteCommand =>
            _DeleteCommand ?? (_DeleteCommand = new DelegateCommand<object>(ExecuteGoCommand));

        void ExecuteGoCommand(object obj)
        {
            //删除目标导航视图
            Region.Regions["ContentRegion"].Remove(obj);
            //删除所有导航视图
            //Region.Regions["ContentRegion"].RemoveAll();
        }
    }
}

