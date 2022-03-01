using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using SmartParking.Client.BLL;
using SmartParking.Client.Modules.Models;
using SmartParking.Client.Modules.Views;
using Unity;

namespace SmartParking.Client.Modules.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IUserBll userBLL;

        public override string PageTitle { get; set; } = "用户信息管理";

        public ObservableCollection<UserModel> UserList { get; set; } = new ObservableCollection<UserModel>();

        public UserManagementViewModel(IUserBll userBLL, IUnityContainer unityContainer, IRegionManager regionManager, IDialogService dialogService) : base(unityContainer, regionManager)
        {
            this.userBLL = userBLL;
            this.dialogService = dialogService;
            Refresh();
        }

        public DelegateCommand RefreshCommand
        {
            get => new DelegateCommand(() =>
            {
                Refresh();
            });
        }

        public DelegateCommand AddCommand
        {
            get => new DelegateCommand(() =>
            {
                // TODO: add user
            });
        }

        public DelegateCommand<object> ResetRoleCommand
        {
            get => new DelegateCommand<object>((arg) =>
            {
                // TODO: reset role
            });
        }

        public DelegateCommand<object> EditCommand
        {
            get => new DelegateCommand<object>((arg) =>
            {
                var user = arg as UserModel;
                DialogParameters param = new DialogParameters();
                param.Add("user", user.SysUser);
                dialogService.ShowDialog(nameof(UserModifyDialog), param, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        MessageBox.Show("数据保存成功!", "提示");
                        Refresh();
                    }
                });
            });
        }

        public DelegateCommand<object> DeleteCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (MessageBox.Show("是否确定删除此用户信息？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // 把用户的状态的置成不可用（逻辑删除），也可以进行数据物理删除
                    var user = arg as UserModel;
                    if (user != null)
                        await userBLL.SetState(user.SysUser.user_id, 1);

                    this.Refresh();
                }
            });
        }

        private void Refresh()
        {
            UserList.Clear();
            Task.Run(() =>
            {
                var users = userBLL.GetUsers().GetAwaiter().GetResult()?.Select(u =>
                {
                    UserModel m = new UserModel(u);
                    m.UserIcon = "pack://application:,,,/SmartParking.Client.Assets;component/Images/avatar.png";
                    var roles = userBLL.GetRolesByUserId(u.user_id).GetAwaiter().GetResult();
                    roles.ForEach(r => m.Roles.Add(r));
                    return m;
                });

                unityContainer.Resolve<Dispatcher>().Invoke(() =>
                {
                    foreach (var user in users)
                    {
                        UserList.Add(user);
                    }
                });
            });
        }
    }
}
