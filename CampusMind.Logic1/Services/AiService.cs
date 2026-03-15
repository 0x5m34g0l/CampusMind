using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CampusMind.Logic1.Core;

namespace CampusMind.Logic1.Services
{
    public static class AiService
    {
        // Single static HttpClient instance
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        private const string API_URL = "https://openrouter.ai/api/v1/chat/completions";
        private const string MODEL = "meta-llama/llama-3.1-8b-instruct";
        private static readonly string API_KEY =
            Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
            ?? throw new Exception("OPENROUTER_API_KEY environment variable not set.");

        public static async Task<string> AskAI(List<Message> messages)
        {
            try
            {
                var apiMessages = new List<object>
                {
                    new
                    {
                        role = "system",
                        content = "You are CampusMind AI, a helpful assistant for university students."
                    }
                };

                foreach (var msg in messages)
                {
                    apiMessages.Add(new
                    {
                        role = msg.Role == enRole.User ? "user" : "assistant",
                        content = msg.Content
                    });
                }

                var requestBody = new
                {
                    model = MODEL,
                    messages = apiMessages,
                    max_tokens = 350,
                    temperature = 0.7
                };

                var json = JsonSerializer.Serialize(requestBody);

                using var request = new HttpRequestMessage(HttpMethod.Post, API_URL);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
                request.Headers.Add("HTTP-Referer", "https://campusmind.local");
                request.Headers.Add("X-Title", "CampusMind");

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send request
                var response = await _httpClient.SendAsync(request);

                var responseText = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"[AiService] Status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"[AiService] Response: {responseText}");

                if (!response.IsSuccessStatusCode)
                {
                    return $"API Error {(int)response.StatusCode}: {responseText}";
                }

                var doc = JsonDocument.Parse(responseText);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No response received.";
            }
            catch (TaskCanceledException)
            {
                return "⏱ Request timed out. Please check your internet connection and try again.";
            }
            catch (HttpRequestException ex)
            {
                return $"🌐 Network error: {ex.Message}";
            }
            catch (JsonException ex)
            {
                return $"⚠️ Failed to parse AI response: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"❌ Unexpected error: {ex.Message}";
            }
        }
    }
}