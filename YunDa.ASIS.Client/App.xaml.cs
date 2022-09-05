using Prism.Ioc;
using System.Windows;
using YunDa.ASIS.Client.Views;

namespace YunDa.ASIS.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<View1>(nameof(View1));
            containerRegistry.RegisterForNavigation<View2>(nameof(View2));
            containerRegistry.RegisterForNavigation<SignalRClientView>(nameof(SignalRClientView));


        }
    }
}
