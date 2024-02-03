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

        string prevUrl, nextUrl;
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

                string chapterNavHtml = BuildNavHtmlForChatper(tree, i, j, out prevUrl, out nextUrl);

                chapterHtml = htmlTemplateText
                    .Replace("$nav-content$", chapterNavHtml)
                    .Replace("$nav-prev-url$", prevUrl)
                    .Replace("$nav-next-url$", nextUrl)
                    .Replace("$chapter-content$", chapterHtml);
                outChapterFile.WriteAllText(chapterHtml);
            }

            string sectionNavHtml = BuildNavHtmlForSection(tree, i, out prevUrl, out nextUrl);

            sectionHtml = htmlTemplateText
                .Replace("$nav-content$", sectionNavHtml)
                .Replace("$nav-prev-url$", prevUrl)
                .Replace("$nav-next-url$", nextUrl)
                .Replace("$chapter-content$", sectionHtml);
            outSectionFile.WriteAllText(sectionHtml);
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
        const int MaxNumSectionsInFirstColumn = 3;

        var builder = new StringBuilder();
        builder.AppendLine(@"<div class=""contents""><h1 class=""part"">Table of Contents</h1><div class=""chapters""><div class=""columns"">");

        builder.AppendLine(@"<div class=""first"">");
        for (int i = 0; i < Math.Min(MaxNumSectionsInFirstColumn, tree.Sections.Length); i++)
        {
            var section = tree.Sections[i];
            BuildSection(section);
        }

        builder.AppendLine(@"</div>");

        builder.AppendLine(@"<div class=""second"">");
        for (int i = MaxNumSectionsInFirstColumn; i < tree.Sections.Length; i++)
        {
            var section = tree.Sections[i];
            BuildSection(section);
        }

        builder.AppendLine(@"</div>");

        builder.AppendLine(@"</div></div></div>");
        return builder.ToString();

        void BuildSection(CompilerBookSection section)
        {
            builder.AppendLine($@"<h2><span class=""num"">{roman[section.SectionNumber - 1]}.</span><a href=""/compiler-book/{section.FileNameWithoutExtension}.html"" name=""{section.FileNameWithoutExtension}"">{section.SectionTitle}</a></h2>");
            builder.AppendLine("<ul>");
            for (int i = 0; i < section.Chapters.Length; i++)
            {
                var chapter = section.Chapters[i];
                BuildChapter(chapter);
            }

            builder.AppendLine("</ul>");
        }

        void BuildChapter(CompilerBookChapter chapter)
        {
            builder.AppendLine($@"<li><span class=""num"">{chapter.ChapterNumber}.</span><a href=""/compiler-book/{chapter.FileNameWithoutExtension}.html"">{chapter.ChapterTitle}</a></li>");
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
        for (int i = 0; i < tree.Sections.Length; i++)
        {
            var section = tree.Sections[i];
            string sectionTitle = section.SectionTitle;
            builder.AppendLine($@"    <li><a href=""/compiler-book/{section.FileNameWithoutExtension}.html""><small>{roman[i]}</small>{sectionTitle}</a></li>");
        }

        prevUrl = "/compiler-book/contents.html";
        nextUrl = $"/compiler-book/{tree.Sections[0].FileNameWithoutExtension}.html";

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForSection(CompilerBookTree tree, int sectionIndex, out string prevUrl, out string nextUrl)
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

        prevUrl = "/compiler-book/contents.html";
        if (sectionIndex > 0)
        {
            var prevSection = tree.Sections[sectionIndex - 1];
            prevUrl = $"/compiler-book/{prevSection.Chapters.Last().FileNameWithoutExtension}.html";
        }

        nextUrl = $"/compiler-book/{section.Chapters.LastOrDefault()?.FileNameWithoutExtension ?? "contents"}.html";

        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/contents.html"">↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForChatper(CompilerBookTree tree, int sectionIndex, int chapterIndex, out string prevUrl, out string nextUrl)
    {
        var chapter = tree.Sections[sectionIndex].Chapters[chapterIndex];
        string url = $"/compiler-book/{chapter.FileNameWithoutExtension}.html";

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, chapter.ChapterTitle);

        if (chapterIndex > 0)
        {
            var prevChapter = tree.Sections[sectionIndex].Chapters[chapterIndex - 1];
            prevUrl = $"/compiler-book/{prevChapter.FileNameWithoutExtension}.html";
        }
        else
        {
            prevUrl = $"/compiler-book/{tree.Sections[sectionIndex].FileNameWithoutExtension}.html";
        }

        nextUrl = "#";
        if (chapterIndex + 1 < tree.Sections[sectionIndex].Chapters.Length)
        {
            var nextChapter = tree.Sections[sectionIndex].Chapters[chapterIndex + 1];
            nextUrl = $"/compiler-book/{nextChapter.FileNameWithoutExtension}.html";
        }
        else if (sectionIndex + 1 < tree.Sections.Length)
        {
            var nextSection = tree.Sections[sectionIndex + 1];
            nextUrl = $"/compiler-book/{nextSection.FileNameWithoutExtension}.html";
        }

        builder.AppendLine($@"<ul>");
        //builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"" href=""{prevUrl}"">← Previous</a>");
        builder.AppendLine($@"    <a href=""/compiler-book/{tree.Sections[sectionIndex].FileNameWithoutExtension}.html"">↑ Up</a>");
        if (nextUrl != "#")
            builder.AppendLine($@"    <a class=""right"" href=""{nextUrl}"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }
}
