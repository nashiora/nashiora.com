using System.Diagnostics;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace SiteGenerator.CompilerBook;

public sealed class CompilerBookBuilder(DirectoryInfo bookDir)
{
    private static readonly string[] roman = [
        "I",
        "II",
        "III",
        "IV",
        "V",
        "VI",
        "VII",
        "VIII",
        "IX",
        "X",
    ];

    public DirectoryInfo BookDirectory { get; } = bookDir;

    public void Build(CompilerBookTree tree, DirectoryInfo outDir)
    {
        outDir.Create();

        string htmlTemplateText = BookDirectory.ChildFile("page-template.html")
            .ReadAllText()
            .Replace("$book-title$", "C is for Compiler");

        string prevUrl, nextUrl;
        for (int i = 0; i < tree.Units.Length; i++)
        {
            var unit = tree.Units[i];

            string outUnitFileName = unit.SourceFile.Name.Replace(".md", ".html");
            var outUnitFile = outDir.ChildFile(outUnitFileName);

            Debug.Assert(unit.MarkdownDocument is not null);
            string unitHtml = $"<article>{unit.MarkdownDocument.ToHtml()}</article>";

            for (int j = 0; j < unit.Chapters.Length; j++)
            {
                var chapter = unit.Chapters[j];

                string outChapterFileName = chapter.SourceFile.Name.Replace(".md", ".html");
                var outChapterFile = outDir.ChildFile(outChapterFileName);

                var chapterDoc = chapter.MarkdownDocument;
                string chapterHtml = $"<article>{chapterDoc.ToHtml()}</article>";

                string chapterNavHtml = BuildNavHtmlForChatper(tree, i, j, out prevUrl, out nextUrl);

                chapterHtml = htmlTemplateText
                    .Replace("$nav-content$", chapterNavHtml)
                    .Replace("$nav-prev-url$", prevUrl)
                    .Replace("$nav-next-url$", nextUrl)
                    .Replace("$chapter-content$", chapterHtml);
                outChapterFile.WriteAllText(chapterHtml);
            }

            string unitNavHtml = BuildNavHtmlForUnit(tree, i, out prevUrl, out nextUrl);

            unitHtml = htmlTemplateText
                .Replace("$nav-content$", unitNavHtml)
                .Replace("$nav-prev-url$", prevUrl)
                .Replace("$nav-next-url$", nextUrl)
                .Replace("$chapter-content$", unitHtml);
            outUnitFile.WriteAllText(unitHtml);
        }

        string contentsHtml = BuildTableOfContents(tree);
        string contentsNavHtml = BuildNavHtmlForContents(tree, out prevUrl, out nextUrl);

        contentsHtml = htmlTemplateText
            .Replace("$nav-content$", contentsNavHtml)
            .Replace("$nav-prev-url$", prevUrl)
            .Replace("$nav-next-url$", nextUrl)
            .Replace("$chapter-content$", contentsHtml);
        outDir.ChildFile("contents.html").WriteAllText(contentsHtml);
    }

    private string BuildTableOfContents(CompilerBookTree tree)
    {
        const int MaxNumUnitsInFirstColumn = 3;

        var builder = new StringBuilder();
        builder.AppendLine(@"<div class=""contents""><h1 class=""part"">Table of Contents</h1><div class=""chapters""><div class=""columns"">");

        builder.AppendLine(@"<div class=""first"">");
        for (int i = 0; i < Math.Min(MaxNumUnitsInFirstColumn, tree.Units.Length); i++)
        {
            var unit = tree.Units[i];
            BuildUnit(unit);
        }

        builder.AppendLine(@"</div>");

        builder.AppendLine(@"<div class=""second"">");
        for (int i = MaxNumUnitsInFirstColumn; i < tree.Units.Length; i++)
        {
            var unit = tree.Units[i];
            BuildUnit(unit);
        }

        builder.AppendLine(@"</div>");

        builder.AppendLine(@"</div></div></div>");
        return builder.ToString();

        void BuildUnit(CompilerBookUnit unit)
        {
            builder.AppendLine($@"<h2><span class=""num"">{roman[unit.UnitNumber - 1]}.</span><a href=""/compiler-book/{unit.FileNameWithoutExtension}.html"" name=""{unit.FileNameWithoutExtension}"">{unit.UnitTitle}</a></h2>");
            builder.AppendLine("<ul>");
            for (int i = 0; i < unit.Chapters.Length; i++)
            {
                var chapter = unit.Chapters[i];
                BuildChapter(chapter);
            }

            builder.AppendLine("</ul>");
        }

        void BuildChapter(CompilerBookChapter chapter)
        {
            builder.AppendLine($@"<li><h3><span class=""num"">{chapter.ChapterNumber}.</span><a href=""/compiler-book/{chapter.FileNameWithoutExtension}.html"">{chapter.ChapterTitle}</a></h3></li>");
        }
    }

    private void BuildNavHtmlHeader(CompilerBookTree tree, StringBuilder builder, string url, string title)
    {
        builder.AppendLine($@"<h2><a href=""{url}"">{title}</a></h2>");
    }

    private string BuildNavHtmlForContents(CompilerBookTree tree, out string prevUrl, out string nextUrl)
    {
        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, "/compiler-book/contents.html", "Table of Contents");

        builder.AppendLine($@"<ul>");
        for (int i = 0; i < tree.Units.Length; i++)
        {
            var unit = tree.Units[i];
            string unitTitle = unit.UnitTitle;
            builder.AppendLine($@"    <li><a href=""/compiler-book/{unit.FileNameWithoutExtension}.html""><small>{roman[i]}</small>{unitTitle}</a></li>");
        }

        prevUrl = "/compiler-book/contents.html";
        nextUrl = $"/compiler-book/{tree.Units[0].FileNameWithoutExtension}.html";

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForUnit(CompilerBookTree tree, int unitIndex, out string prevUrl, out string nextUrl)
    {
        var unit = tree.Units[unitIndex];
        string url = $"/compiler-book/{unit.FileNameWithoutExtension}.html";

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, unit.UnitTitle);

        builder.AppendLine($@"<ul>");
        for (int i = 0; i < unit.Chapters.Length; i++)
        {
            var chapter = unit.Chapters[i];
            string chapterTitle = chapter.ChapterTitle;
            builder.AppendLine($@"    <li><a href=""/compiler-book/{chapter.FileNameWithoutExtension}.html""><small>{chapter.ChapterNumber}</small>{chapterTitle}</a></li>");
        }

        prevUrl = "/compiler-book/contents.html";
        if (unitIndex > 0)
        {
            var prevUnit = tree.Units[unitIndex - 1];
            prevUrl = $"/compiler-book/{prevUnit.Chapters.Last().FileNameWithoutExtension}.html";
        }

        nextUrl = $"/compiler-book/{unit.Chapters.LastOrDefault()?.FileNameWithoutExtension ?? "contents"}.html";

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForChatper(CompilerBookTree tree, int unitIndex, int chapterIndex, out string prevUrl, out string nextUrl)
    {
        var chapter = tree.Units[unitIndex].Chapters[chapterIndex];
        string url = $"/compiler-book/{chapter.FileNameWithoutExtension}.html";

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, chapter.ChapterTitle);

        if (chapterIndex > 0)
        {
            var prevChapter = tree.Units[unitIndex].Chapters[chapterIndex - 1];
            prevUrl = $"/compiler-book/{prevChapter.FileNameWithoutExtension}.html";
        }
        else
        {
            prevUrl = $"/compiler-book/{tree.Units[unitIndex].FileNameWithoutExtension}.html";
        }

        nextUrl = "#";
        if (chapterIndex + 1 < tree.Units[unitIndex].Chapters.Length)
        {
            var nextChapter = tree.Units[unitIndex].Chapters[chapterIndex + 1];
            nextUrl = $"/compiler-book/{nextChapter.FileNameWithoutExtension}.html";
        }
        else if (unitIndex + 1 < tree.Units.Length)
        {
            var nextUnit = tree.Units[unitIndex + 1];
            nextUrl = $"/compiler-book/{nextUnit.FileNameWithoutExtension}.html";
        }

        builder.AppendLine($@"<ul>");
        //builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/{tree.Units[unitIndex].FileNameWithoutExtension}.html"">↑ Up</a>");
        if (nextUrl != "#")
            builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }
}
