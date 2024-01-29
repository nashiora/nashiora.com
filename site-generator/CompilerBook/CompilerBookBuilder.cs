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

        string htmlTemplateText = BookDirectory.ChildFile("page-template.html").ReadAllText();

        for (int i = 0; i < tree.Sections.Length; i++)
        {
            var section = tree.Sections[i];

            string outSectionFileName = section.SourceFile.Name.Replace(".md", ".html");
            var outSectionFile = outDir.ChildFile(outSectionFileName);

            Debug.Assert(section.MarkdownDocument is not null);
            string sectionHtml = section.MarkdownDocument.ToHtml();

            for (int j = 0; j < section.Chapters.Length; j++)
            {
                var chapter = section.Chapters[j];

                string outChapterFileName = chapter.SourceFile.Name.Replace(".md", ".html");
                var outChapterFile = outDir.ChildFile(outChapterFileName);

                var chapterDoc = chapter.MarkdownDocument;
                string chapterHtml = chapterDoc.ToHtml();

                string chapterNavHtml = BuildNavHtmlForChatper(tree, i, j);

                chapterHtml = htmlTemplateText.Replace("$nav-content$", chapterNavHtml).Replace("$chapter-content$", chapterHtml);
                outChapterFile.WriteAllText(chapterHtml);
            }

            string sectionNavHtml = BuildNavHtmlForSection(tree, i);

            sectionHtml = htmlTemplateText.Replace("$nav-content$", sectionNavHtml).Replace("$chapter-content$", sectionHtml);
            outSectionFile.WriteAllText(sectionHtml);
        }

        string contentsHtml = BookDirectory.ChildFile("contents.html").ReadAllText();
        string contentsNavHtml = BuildNavHtmlForContents(tree);

        contentsHtml = htmlTemplateText.Replace("$nav-content$", contentsNavHtml).Replace("$chapter-content$", contentsHtml);
        outDir.ChildFile("contents.html").WriteAllText(contentsHtml);
    }

    private void BuildNavHtmlHeader(CompilerBookTree tree, StringBuilder builder, string url, string title)
    {
        builder.AppendLine($@"<h2><a href=""{url}"">{title}</a></h2>");
    }

    private string BuildNavHtmlForContents(CompilerBookTree tree)
    {
        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, "/compiler-book/contents.html", "Table of Contents");

        builder.AppendLine($@"<ul>");
        for (int i = 0; i < tree.Sections.Length; i++)
        {
            var section = tree.Sections[i];
            string sectionTitle = section.SectionTitle;
            builder.AppendLine($@"    <li><a href=""/compiler-book/{section.FileNameWithoutExtension}.html""><small>{roman[i]}</small>{sectionTitle}</a></li>");
        }

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""/compiler-book/contents.html"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""/compiler-book/{tree.Sections[0].FileNameWithoutExtension}.html"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForSection(CompilerBookTree tree, int sectionIndex)
    {
        var section = tree.Sections[sectionIndex];
        string url = $"/compiler-book/{section.FileNameWithoutExtension}.html";

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, section.SectionTitle);

        builder.AppendLine($@"<ul>");
        for (int i = 0; i < section.Chapters.Length; i++)
        {
            var chapter = section.Chapters[i];
            string chapterTitle = chapter.ChapterTitle;
            builder.AppendLine($@"    <li><a href=""/compiler-book/{chapter.FileNameWithoutExtension}.html""><small>{chapter.ChapterNumber}</small>{chapterTitle}</a></li>");
        }

        string prevHref = "/compiler-book/contents.html";
        if (sectionIndex > 0)
        {
            var prevSection = tree.Sections[sectionIndex - 1];
            prevHref = $"/compiler-book/{prevSection.Chapters.Last().FileNameWithoutExtension}.html";
        }

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevHref}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""/compiler-book/{section.Chapters[0].FileNameWithoutExtension}.html"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForChatper(CompilerBookTree tree, int sectionIndex, int chapterIndex)
    {
        var chapter = tree.Sections[sectionIndex].Chapters[chapterIndex];
        string url = $"/compiler-book/{chapter.FileNameWithoutExtension}.html";

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, chapter.ChapterTitle);

        string prevHref;
        if (chapterIndex > 0)
        {
            var prevChapter = tree.Sections[sectionIndex].Chapters[chapterIndex - 1];
            prevHref = $"/compiler-book/{prevChapter.FileNameWithoutExtension}.html";
        }
        else
        {
            prevHref = $"/compiler-book/{tree.Sections[sectionIndex].FileNameWithoutExtension}.html";
        }

        string? nextHref = null;
        if (chapterIndex + 1 < tree.Sections[sectionIndex].Chapters.Length)
        {
            var nextChapter = tree.Sections[sectionIndex].Chapters[chapterIndex + 1];
            nextHref = $"/compiler-book/{nextChapter.FileNameWithoutExtension}.html";
        }
        else if (sectionIndex + 1 < tree.Sections.Length)
        {
            var nextSection = tree.Sections[sectionIndex + 1];
            nextHref = $"/compiler-book/{nextSection.FileNameWithoutExtension}.html";
        }

        builder.AppendLine($@"<ul>");
        //builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevHref}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/{tree.Sections[sectionIndex].FileNameWithoutExtension}.html"">↑ Up</a>");
        if (nextHref is not null)
            builder.AppendLine($@"    <a class=""right"" href=""{nextHref}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }
}
