// HomePage.xaml.cs
using CampusMind.Logic1.Core;
using CampusMind.Logic1.Services;

namespace CampusMind.UI.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUserData();
            LoadConversations();
        }

        private void LoadUserData()
        {
            if (!UserSession.IsLoggedIn) return;

            GreetingLabel.Text = $"{UserSession.GetGreeting()} 👋";
            UserNameLabel.Text = UserSession.CurrentUser.Name;
            AvatarLabel.Text = UserSession.GetAvatarInitials();
        }

        private void LoadConversations()
        {
            if (!UserSession.IsLoggedIn) return;

            // ✅ Matches ConversationService.GetUserConversations(userId)
            var conversations = ConversationService
                .GetUserConversations(UserSession.CurrentUser.Id);

            ConversationsStack.Children.Clear();

            if (conversations == null || conversations.Count == 0)
            {
                EmptyStateView.IsVisible = true;
                ConversationsCountLabel.Text = "0";
                return;
            }

            EmptyStateView.IsVisible = false;
            ConversationsCountLabel.Text = conversations.Count.ToString();

            // Build conversation cards dynamically
            foreach (var conv in conversations)
            {
                var card = BuildConversationCard(conv);
                ConversationsStack.Children.Add(card);
            }
        }

        private Frame BuildConversationCard(Conversation conv)
        {
            // Pick emoji based on ToolType
            var icon = conv.ToolType switch
            {
                enToolType.CourseExplainer => "🧠",
                enToolType.PolicyAssistant => "📜",
                enToolType.StudyPlanner => "🗓",
                enToolType.FileConverter => "📂",
                _ => "💬"
            };

            var card = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 16,
                BorderColor = Color.FromArgb("#E5E7EB"),
                Padding = new Thickness(14),
                HasShadow = false
            };

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(50) },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 12
            };

            // Icon Frame
            var iconFrame = new Frame
            {
                WidthRequest = 46,
                HeightRequest = 46,
                CornerRadius = 14,
                BackgroundColor = Color.FromArgb("#EEF0FF"),
                BorderColor = Colors.Transparent,
                Padding = 0,
                HasShadow = false,
                Content = new Label
                {
                    Text = icon,
                    FontSize = 22,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            // Text Stack
            var textStack = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 2
            };
            textStack.Children.Add(new Label
            {
                Text = conv.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#1E1B4B")
            });
            textStack.Children.Add(new Label
            {
                Text = conv.ToolType.ToString(),
                FontSize = 12,
                TextColor = Color.FromArgb("#6B7280")
            });

            grid.Add(iconFrame, 0);
            grid.Add(textStack, 1);
            card.Content = grid;

            // Navigate to chat on tap
            card.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                    await Shell.Current.GoToAsync(
                        $"ChatPage?conversationId={conv.Id}&toolType={(int)conv.ToolType}"))
            });

            return card;
        }

        // ═══ Tool Taps — Start Conversation ═══

        private async void OnCourseExplainerTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Course Explainer will be available soon.", "OK");
        }

        private async void OnPolicyAssistantTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Policy Assistant will be available soon.", "OK");
        }

        private async void OnStudyPlannerTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Study Planner will be available soon.", "OK");
        }

        private async void OnFileConverterTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "File Converter will be available soon.", "OK");
        }

        private async Task NavigateToChatWithTool(enToolType toolType, string title)
        {
            // ✅ Start a new conversation using existing service
            var conv = ConversationService.StartConversation(
                UserSession.CurrentUser.Id,
                toolType,
                title);

            if (conv != null)
                await Shell.Current.GoToAsync(
                    $"ChatPage?conversationId={conv.Id}&toolType={(int)toolType}");
        }

        private async void OnNewChatTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//ChatPage");

        private async void OnSettingsTapped(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//SettingsPage");
    }
}
