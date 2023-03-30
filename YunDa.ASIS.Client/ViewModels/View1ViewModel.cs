using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YunDa.ASIS.Client.ViewModels
{
    public class View1ViewModel : BindableBase, INavigationAware
    {
        private string _title = "View1";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public View1ViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
