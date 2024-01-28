using Markdig;
using Markdig.Syntax;

namespace SiteGenerator.CompilerBook;

public sealed record class CompilerBookChapterPreprocessReplacement(string SignalText, string ReplacementText);

public sealed class CompilerBookChapterParser
{
    public CompilerBookChapterParser()
    {
    }

    public MarkdownDocument Parse(FileInfo file, CompilerBookChapterPreprocessReplacement[] replacements)
    {
        var text = file.ReadAllText();
        foreach (var replacement in replacements)
            text = text.Replace(replacement.SignalText, replacement.ReplacementText);

        var mdDoc = Markdown.Parse(text);
        return mdDoc;
    }
}
