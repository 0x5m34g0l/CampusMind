// LoginPage.xaml.cs
using CampusMind.Logic1.Services;

namespace CampusMind.UI.Views
{
    public partial class LoginPage : ContentPage
    {
        private bool _isPasswordVisible = false;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnTogglePassword(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            PasswordEntry.IsPassword = !_isPasswordVisible;
            TogglePasswordLabel.Text = _isPasswordVisible ? "🙈" : "👁";
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "⚠️ Please fill in all fields.";
                ErrorLabel.IsVisible = true;
                return;
            }

            ErrorLabel.IsVisible = false;
            LoginButton.IsVisible = false;
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;

            try
            {
                // ✅ Matches AuthService.Login(email, passwordPlain)
                var user = await Task.Run(() => AuthService.Login(email, password));

                if (user != null)
                {
                    // ✅ Set global session
                    UserSession.CurrentUser = user;
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    ErrorLabel.Text = "❌ Invalid email or password.";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = $"❌ Error: {ex.Message}";
                ErrorLabel.IsVisible = true;
            }
            finally
            {
                LoginButton.IsVisible = true;
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        private async void OnNavigateToRegister(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//RegisterPage");
    }
}
