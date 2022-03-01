using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SmartParking.Client.Common;
using SmartParking.Client.Model;
using SmartParking.Client.Modules.Models;
using SmartParking.Client.Modules.Views;

namespace SmartParking.Client.Modules.ViewModels
{
    public class TreeMenuViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public List<MenuItemModel> Menus { get; set; } = new List<MenuItemModel>();
        public TreeMenuViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            FillMenus(Menus, 0);
        }

        private void FillMenus(List<MenuItemModel> menus, long id)
        {
            var orgMenus = GlobalInfo.CurrentMenus;
            if (orgMenus == null)
                return;

            var subMenus = orgMenus.Where(m => m.parent_id == id).OrderBy(o => o.menu_id);

            foreach (var sub in subMenus)
            {
                var model = new MenuItemModel()
                {
                    //MenuIcon = @"&#xe66d;"
                    //MenuIcon = "\xe66d",
                    MenuIcon = sub.icon,
                    MenuHeader = sub.name,
                    TargetView = sub.url,
                };
                model.TargetView = nameof(UserManagementView);
                menus.Add(model);
                FillMenus(model.Children, sub.menu_id);
            }
        }

        public ICommand OpenViewCommand
        {
            get => new DelegateCommand<object>((obj) =>
            {
                var item = obj as MenuItemModel;
                // 子项
                if ((item.Children == null || item.Children.Count == 0) &&
              !string.IsNullOrEmpty(item.TargetView))
                {
                    // 页面导航
                    regionManager.RequestNavigate(SystemString.MainContentRegion, item.TargetView);
                }
                // 父项
                else
                {
                    item.IsExpanded = !item.IsExpanded;
                }
            });
        }
    }
}
