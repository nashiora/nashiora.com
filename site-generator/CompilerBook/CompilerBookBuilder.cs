using System.Diagnostics;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace SiteGenerator.CompilerBook;

public sealed class CompilerBookBuilder(DirectoryInfo bookDir)
{
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
        builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>II</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>III</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>IV</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"">← Previous</a>");
        builder.AppendLine($@"    <a>↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForSection(CompilerBookTree tree, int sectionIndex)
    {
        var section = tree.Sections[sectionIndex];
        string url = $"/compiler-book/{Path.GetFileNameWithoutExtension(section.SourceFile.Name)}.html";
        var firstHeader = (HeadingBlock)section.MarkdownDocument.First(x => x is HeadingBlock);
        string headerText = ((LiteralInline)firstHeader.Inline!.FirstChild!).Content.ToString();

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, headerText);

        builder.AppendLine($@"<ul>");
        builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>II</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>III</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>IV</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"">← Previous</a>");
        builder.AppendLine($@"    <a>↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }

    private string BuildNavHtmlForChatper(CompilerBookTree tree, int sectionIndex, int chapterIndex)
    {
        var chapter = tree.Sections[sectionIndex].Chapters[chapterIndex];
        string url = $"/compiler-book/{Path.GetFileNameWithoutExtension(chapter.SourceFile.Name)}.html";
        var firstHeader = (HeadingBlock)chapter.MarkdownDocument.First(x => x is HeadingBlock);
        string headerText = ((LiteralInline)firstHeader.Inline!.FirstChild!).Content.ToString();

        var builder = new StringBuilder();
        BuildNavHtmlHeader(tree, builder, url, headerText);

        builder.AppendLine($@"<ul>");
        builder.AppendLine($@"    <li><a><small>I</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>II</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>III</small>Something</a></li>");
        builder.AppendLine($@"    <li><a><small>IV</small>Something</a></li>");
        builder.AppendLine($@"</ul>");
        builder.AppendLine($@"<div class=""prev-next"">");
        builder.AppendLine($@"    <a class=""left"">← Previous</a>");
        builder.AppendLine($@"    <a>↑ Up</a>");
        builder.AppendLine($@"    <a class=""right"">Next →</a>");
        builder.AppendLine($@"</div>");

        return builder.ToString();
    }
}
