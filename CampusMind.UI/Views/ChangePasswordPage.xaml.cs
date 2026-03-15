using CampusMind.Logic1.Services;

namespace CampusMind.UI.Views
{
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void OnNewPasswordChanged(object sender, TextChangedEventArgs e)
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

            var inactive = Color.FromArgb("#E5E7EB");

            StrengthBar1.Color = strength >= 1 ? activeColor : inactive;
            StrengthBar2.Color = strength >= 2 ? activeColor : inactive;
            StrengthBar3.Color = strength >= 3 ? activeColor : inactive;
            StrengthBar4.Color = strength >= 4 ? activeColor : inactive;

            StrengthLabel.Text = strength switch
            {
                1 => "Weak",
                2 => "Fair",
                3 => "Good",
                4 => "Strong",
                _ => "Password strength"
            };
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var current = CurrentPasswordEntry.Text;
            var newPwd = NewPasswordEntry.Text;
            var confirm = ConfirmPasswordEntry.Text;

            if (string.IsNullOrEmpty(current) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(confirm))
            {
                StatusLabel.TextColor = Color.FromArgb("#EF4444");
                StatusLabel.Text = "Please fill in all fields.";
                StatusLabel.IsVisible = true;
                return;
            }

            if (newPwd != confirm)
            {
                StatusLabel.TextColor = Color.FromArgb("#EF4444");
                StatusLabel.Text = "New passwords do not match.";
                StatusLabel.IsVisible = true;
                return;
            }

            if (newPwd.Length < 8)
            {
                StatusLabel.TextColor = Color.FromArgb("#EF4444");
                StatusLabel.Text = "Password must be at least 8 characters.";
                StatusLabel.IsVisible = true;
                return;
            }

            StatusLabel.IsVisible = false;
            SaveButton.IsVisible = false;
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;

            try
            {
                // Verify current password first
                var user = await Task.Run(() =>
                    AuthService.Login(UserSession.CurrentUser.Email, current));

                if (user == null)
                {
                    StatusLabel.TextColor = Color.FromArgb("#EF4444");
                    StatusLabel.Text = "Current password is incorrect.";
                    StatusLabel.IsVisible = true;
                    return;
                }

                // Change password via AuthService
                var success = await Task.Run(() =>
                    AuthService.ChangePassword(UserSession.CurrentUser.Id, newPwd));

                if (success)
                {
                    StatusLabel.TextColor = Color.FromArgb("#10B981");
                    StatusLabel.Text = "Password updated successfully!";
                    StatusLabel.IsVisible = true;

                    // Clear fields
                    CurrentPasswordEntry.Text = string.Empty;
                    NewPasswordEntry.Text = string.Empty;
                    ConfirmPasswordEntry.Text = string.Empty;

                    await Task.Delay(1500);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    StatusLabel.TextColor = Color.FromArgb("#EF4444");
                    StatusLabel.Text = "Failed to update password. Try again.";
                    StatusLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                StatusLabel.TextColor = Color.FromArgb("#EF4444");
                StatusLabel.Text = $"Error: {ex.Message}";
                StatusLabel.IsVisible = true;
            }
            finally
            {
                SaveButton.IsVisible = true;
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
