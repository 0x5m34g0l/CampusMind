namespace CampusMind.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ChangePasswordPage", typeof(Views.ChangePasswordPage));
            Routing.RegisterRoute("PrivacyPolicyPage", typeof(Views.PrivacyPolicyPage));
            Routing.RegisterRoute("AboutPage", typeof(Views.AboutPage));
        }
    }
}
