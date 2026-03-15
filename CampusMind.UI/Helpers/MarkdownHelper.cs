using Markdig;

public static class MarkdownHelper
{
    public static string ToPlainText(string markdown)
    {
        return Markdown.ToPlainText(markdown);
    }
}