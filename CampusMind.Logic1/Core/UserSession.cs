// CampusMind.Logic1.Core — UserSession.cs (update existing)
using CampusMind.Logic1.Core;

public static class UserSession
{
    public static User CurrentUser { get; set; } = null;
    public static bool IsLoggedIn => CurrentUser != null;

    public static void Logout()
    {
        CurrentUser = null;
    }

    public static string GetAvatarInitials()
    {
        if (CurrentUser == null || string.IsNullOrWhiteSpace(CurrentUser.Name))
            return "?";

        var parts = CurrentUser.Name.Split(' ')
            .Where(w => w.Length > 0)
            .Take(2)
            .Select(w => w[0].ToString().ToUpper());

        return string.Concat(parts);
    }

    public static string GetGreeting()
    {
        var hour = System.DateTime.Now.Hour;
        return hour < 12 ? "Good Morning" :
               hour < 17 ? "Good Afternoon" : "Good Evening";
    }
}
