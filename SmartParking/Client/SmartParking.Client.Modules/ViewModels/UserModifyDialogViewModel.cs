using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SmartParking.Client.BLL;
using SmartParking.Client.Model;

namespace SmartParking.Client.Modules.ViewModels
{
    public class UserModifyDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IUserBll userBLL;

        private sys_user user;
        public sys_user User
        {
            get { return user; }
            set { SetProperty<sys_user>(ref user, value); }
        }


        public UserModifyDialogViewModel(IUserBll userBLL)
        {
            this.userBLL = userBLL;
        }

        public DelegateCommand ConfirmCommand
        {
            get => new DelegateCommand(async () =>
            {
                await userBLL.SaveUser(User);
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            });
        }

        public DelegateCommand CancelCommand
        {
            get => new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
        }

        #region 对话框方法组
        public string Title => "编辑用户信息";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            User = parameters.GetValue<sys_user>("user");
        }
        #endregion
    }
}
