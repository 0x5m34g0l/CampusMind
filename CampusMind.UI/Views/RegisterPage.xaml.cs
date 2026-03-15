// RegisterPage.xaml.cs
using CampusMind.Logic1.Services;

namespace CampusMind.UI.Views
{
    public partial class RegisterPage : ContentPage
    {
        private bool _isPasswordVisible = false;

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void OnTogglePassword(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            PasswordEntry.IsPassword = !_isPasswordVisible;
            TogglePasswordLabel.Text = _isPasswordVisible ? "🙈" : "👁";
        }

        private void OnPasswordChanged(object sender, TextChangedEventArgs e)
        {
            var pwd = e.NewTextValue ?? "";
            int strength = 0;

            if (pwd.Length >= 8) strength++;
            if (pwd.Any(char.IsUpper)) strength++;
            if (pwd.Any(char.IsDigit)) strength++;
            if (pwd.Any(c => !char.IsLetterOrDigit(c))) strength++;

            var activeColor = strength switch
            {
                1 => Color.FromArgb("#EF4444"),
                2 => Color.FromArgb("#F59E0B"),
                3 => Color.FromArgb("#10B981"),
                4 => Color.FromArgb("#4F46E5"),
                _ => Color.FromArgb("#E5E7EB")
            };

            var inactiveColor = Color.FromArgb("#E5E7EB");

            StrengthBar1.Color = strength >= 1 ? activeColor : inactiveColor;
            StrengthBar2.Color = strength >= 2 ? activeColor : inactiveColor;
            StrengthBar3.Color = strength >= 3 ? activeColor : inactiveColor;
            StrengthBar4.Color = strength >= 4 ? activeColor : inactiveColor;

            StrengthLabel.Text = strength switch
            {
                1 => "Weak",
                2 => "Fair",
                3 => "Good",
                4 => "Strong",
                _ => "Password strength"
            };
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)
                                           || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "⚠️ Please fill in all fields.";
                ErrorLabel.IsVisible = true;
                return;
            }

            if (password != confirmPassword)
            {
                ErrorLabel.Text = "❌ Passwords do not match.";
                ErrorLabel.IsVisible = true;
                return;
            }

            if (!TermsCheckBox.IsChecked)
            {
                ErrorLabel.Text = "⚠️ Please accept the Terms & Privacy Policy.";
                ErrorLabel.IsVisible = true;
                return;
            }

            ErrorLabel.IsVisible = false;
            RegisterButton.IsVisible = false;
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;

            try
            {
                // ✅ Matches AuthService.Register(email, passwordPlain, name)
                var user = await Task.Run(() => AuthService.Register(email, password, name));

                if (user != null)
                {
                    // ✅ Auto-login after register
                    UserSession.CurrentUser = user;
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    ErrorLabel.Text = "❌ Email may already be registered.";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = $"❌ {ex.Message}";
                ErrorLabel.IsVisible = true;
            }
            finally
            {
                RegisterButton.IsVisible = true;
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//LoginPage");
    }
}
