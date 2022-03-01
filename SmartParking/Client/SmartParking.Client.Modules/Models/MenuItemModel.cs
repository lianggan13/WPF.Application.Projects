using System.Collections.Generic;
using Prism.Mvvm;

namespace SmartParking.Client.Modules.Models
{
    public class MenuItemModel : BindableBase
    {
        private string menuIcon;
        public string MenuIcon
        {
            get { return menuIcon; }
            set { SetProperty(ref menuIcon, value); }
        }
        //public string MenuIcon { get; set; }
        public string MenuHeader { get; set; }
        public string TargetView { get; set; }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        public List<MenuItemModel> Children { get; set; }

        public MenuItemModel()
        {
            Children = new List<MenuItemModel>();
        }
    }
}
