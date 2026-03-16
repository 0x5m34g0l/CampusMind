namespace CampusMind.UI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // ✅ Restore saved theme preference before any page loads
        ApplySavedTheme();

        MainPage = new AppShell();
    }

    private static void ApplySavedTheme()
    {
        // Reads "IsDarkMode" key from device Preferences (defaults to false)
        bool isDark = Preferences.Default.Get("IsDarkMode", false);

        Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
    }
}
