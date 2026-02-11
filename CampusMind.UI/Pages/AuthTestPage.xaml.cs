using CampusMind.Logic1.Services;
using Microsoft.Maui.Controls;
namespace CampusMind.UI.Pages;

public partial class AuthTestPage : ContentPage
{
    public AuthTestPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        var name = NameEntry.Text?.Trim();
        var pass = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pass))
        {
            await DisplayAlert("Missing", "Please fill Email, Name, Password.", "OK");
            return;
        }

        var user = AuthService.Register(email, pass, name);

        ResultLabel.Text = user != null
            ? $"REGISTER OK ✅\nId={user.Id}\nName={user.Name}\nEmail={user.Email}"
            : "REGISTER FAILED ❌ (maybe email already exists)";
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        var pass = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
        {
            await DisplayAlert("Missing", "Please fill Email and Password.", "OK");
            return;
        }

        var user = AuthService.Login(email, pass);

        ResultLabel.Text = user != null
            ? $"LOGIN OK ✅\nId={user.Id}\nName={user.Name}\nEmail={user.Email}"
            : "LOGIN FAILED ❌ (wrong email or password)";
    }
}
