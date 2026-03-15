using CampusMind.Logic1.Core;
using CampusMind.Logic1.Services;


namespace CampusMind.UI.Views
{
    [QueryProperty(nameof(ConversationId), "conversationId")]
    [QueryProperty(nameof(ToolTypeValue), "toolType")]
    public partial class ChatPage : ContentPage
    {
        private bool _hasLoadedMessages;
        private bool _isSendingMessage;

        public int ConversationId { get; set; }
        public int ToolTypeValue { get; set; }

        public ChatPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!UserSession.IsLoggedIn)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            SetChatTitle();

            if (!_hasLoadedMessages)
            {
                LoadExistingMessages();
                _hasLoadedMessages = true;
            }
        }

        private void SetChatTitle()
        {
            if (ToolTypeValue <= 0)
            {
                ChatTitleLabel.Text = "CampusMind AI";
                return;
            }

            var toolType = (enToolType)ToolTypeValue;

            ChatTitleLabel.Text = toolType switch
            {
                enToolType.CourseExplainer => "Course Explainer",
                enToolType.PolicyAssistant => "Policy Assistant",
                enToolType.StudyPlanner => "Study Planner",
                enToolType.FileConverter => "File Converter",
                _ => "CampusMind AI"
            };
        }

        private void LoadExistingMessages()
        {
            if (ConversationId <= 0)
                return;

            var messages = MessageService.GetConversationMessages(ConversationId);

            foreach (var msg in messages)
            {
                if (msg.Role == enRole.User)
                    AddUserBubble(msg.Content);
                else
                    AddAIBubble(msg.Content);
            }
        }

        private void SafeScroll()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Task.Delay(150);

                    await MessagesScrollView.ScrollToAsync(
                        0,
                        MessagesStack.Height,
                        true);
                }
                catch { }
            });
        }

        private async void OnSendMessage(object sender, EventArgs e)
        {
            if (_isSendingMessage) return;

            var text = MessageEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            _isSendingMessage = true;
            MessageEntry.Text = string.Empty;
            MessageEntry.Unfocus();

            string safeResponse = string.Empty;

            TypingIndicator.IsVisible = true;

            try
            {
                if (ConversationId <= 0)
                {
                    var conv = await Task.Run(() =>
                        ConversationService.StartConversation(
                            UserSession.CurrentUser.Id,
                            enToolType.None,
                            "New Chat"));

                    if (conv == null)
                    {
                        safeResponse = "Unable to start a conversation.";
                        return;
                    }

                    ConversationId = conv.Id;
                }

                await Task.Run(() =>
                    MessageService.SendUserMessage(ConversationId, text));

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    AddUserBubble(text);
                    SafeScroll();
                });

                var history = await Task.Run(() =>
                    MessageService.GetConversationMessages(ConversationId));

                var aiResponse = await AiService.AskAI(history);

                safeResponse = string.IsNullOrWhiteSpace(aiResponse)
                    ? "I couldn't generate a response."
                    : aiResponse;

                await Task.Run(() =>
                    MessageService.SendAiMessage(ConversationId, safeResponse));
            }
            catch (Exception ex)
            {
                safeResponse = $"Something went wrong: {ex.Message}";
            }
            finally
            {
                TypingIndicator.IsVisible = false;
                _isSendingMessage = false;
            }

            if (!string.IsNullOrEmpty(safeResponse))
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    AddAIBubble(safeResponse);
                    SafeScroll();
                });
            }
        }

        private void AddUserBubble(string text)
        {
            var wrapper = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var bubble = new Frame
            {
                BackgroundColor = Color.FromArgb("#4F46E5"),
                CornerRadius = 18,
                Padding = new Thickness(14, 10),
                HorizontalOptions = LayoutOptions.End,
                MaximumWidthRequest = 265,
                HasShadow = false,
                Content = new Label
                {
                    Text = text,
                    FontSize = 14,
                    TextColor = Colors.White,
                    LineBreakMode = LineBreakMode.WordWrap
                }
            };

            wrapper.Add(bubble, 1);

            if (MessagesStack.Children.Count > 0)
                MessagesStack.Children.Insert(MessagesStack.Children.Count - 1, wrapper);
            else
                MessagesStack.Children.Add(wrapper);
        }

        private async Task TypeMessage(Label label, string text)
        {
            label.Text = "";

            foreach (char c in text)
            {
                label.Text += c;
                await Task.Delay(10);
            }
        }

        private void AddAIBubble(string text)
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 10
            };

            var avatar = new Frame
            {
                WidthRequest = 36,
                HeightRequest = 36,
                CornerRadius = 18,
                BackgroundColor = Color.FromArgb("#4F46E5"),
                Padding = 0,
                HasShadow = false,
                Content = new Image
                {
                    Source = "owl_logo.png",
                    WidthRequest = 22,
                    HeightRequest = 22
                }
            };

            var label = new Label
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#1E1B4B"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            var bubble = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 18,
                BorderColor = Color.FromArgb("#E5E7EB"),
                Padding = new Thickness(14, 10),
                MaximumWidthRequest = 280,
                HasShadow = false,
                Content = label
            };

            grid.Add(avatar, 0);
            grid.Add(bubble, 1);

            if (MessagesStack.Children.Count > 0)
                MessagesStack.Children.Insert(MessagesStack.Children.Count - 1, grid);
            else
                MessagesStack.Children.Add(grid);

            var cleanText = MarkdownHelper.ToPlainText(text);

            _ = TypeMessage(label, cleanText);
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void OnNewChatClicked(object sender, EventArgs e)
        {
            while (MessagesStack.Children.Count > 2)
                MessagesStack.Children.RemoveAt(1);

            ConversationId = 0;
            _hasLoadedMessages = false;
            ChatTitleLabel.Text = "CampusMind AI";
        }
    }
}