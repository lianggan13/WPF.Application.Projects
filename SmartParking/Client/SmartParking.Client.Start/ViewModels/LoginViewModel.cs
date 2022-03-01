using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using SmartParking.Client.BLL;
using SmartParking.Client.Model;
using System.Windows;
using System.Windows.Input;

namespace SmartParking.Client.Start.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string userName = "admin";

        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }

        private string password = "55a0dfde4b46392d4589553a510c3d51cf10d81f2db1d36b4e23e93efd286e28";

        public string Password
        {
            get { return password; }
            set { SetProperty<string>(ref password, value); }
        }

        private string errorMsg;

        public string ErrorMsg
        {
            get { return errorMsg; }
            set { SetProperty<string>(ref errorMsg, value); }
        }

        public ICommand LoginCommand => new DelegateCommand<object>(OnLogin);

        private readonly IUserBll loginBll;
        private readonly IMenuBll menuBll;

        public LoginViewModel(IUserBll loginBll, IMenuBll menuBll)
        {
            this.loginBll = loginBll;
            this.menuBll = menuBll;
        }

        private void OnLogin(object obj)
        {
            //try
            //{
            this.ErrorMsg = "";
            if (string.IsNullOrEmpty(this.UserName))
            {
                this.ErrorMsg = "请输入用户名";
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                this.ErrorMsg = "请输入密码";
                return;
            }
            var userJson = loginBll.Login(userName, password).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<sys_user>(userJson);

                var menus = menuBll.GetMenusByUserId((int)user.user_id).GetAwaiter().GetResult();

                GlobalInfo.CurrentUser = user;
                GlobalInfo.CurrentMenus = menus;

                // 关闭登录窗口，显示主窗口
                (obj as Window).DialogResult = true;
            }
            else
            {

            }
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}






