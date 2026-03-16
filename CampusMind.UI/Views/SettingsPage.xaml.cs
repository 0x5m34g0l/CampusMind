using CampusMind.Logic1.Services;

namespace CampusMind.UI.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadProfile();

            DarkModeSwitch.IsToggled =
                Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void LoadProfile()
        {
            if (!UserSession.IsLoggedIn) return;

            ProfileNameLabel.Text = UserSession.CurrentUser.Name;
            ProfileEmailLabel.Text = UserSession.CurrentUser.Email;
            ProfileAvatarLabel.Text = UserSession.GetAvatarInitials();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Sign Out",
                "Are you sure you want to sign out?",
                "Yes, Sign Out",
                "Cancel");

            if (confirm)
            {
                UserSession.Logout();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        // NEW: Navigate to Change Password page
        private async void OnChangePasswordTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("ChangePasswordPage");

        // NEW: Navigate to Privacy Policy page
        private async void OnPrivacyPolicyTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("PrivacyPolicyPage");

        // NEW: Navigate to About page
        private async void OnAboutTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("AboutPage");

        private async void OnHomeTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//HomePage");

        private async void OnNewChatTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//ChatPage");

        // NEW: Dark mode for the whole app
        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
            Preferences.Default.Set("IsDarkMode", e.Value);
        }
    }
}
