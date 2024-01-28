using Markdig;
using Markdig.Syntax;
using SiteGenerator;

var sourceDir = new DirectoryInfo("src");
var targetDir = new DirectoryInfo("public_html");

targetDir.Delete(true);
targetDir.Create();

sourceDir.CopyTo(targetDir, false);
targetDir.ChildFile("template.html").Delete();

sourceDir.ChildDirectory("css").CopyTo(targetDir.ChildDirectory("css"));
sourceDir.ChildDirectory("img").CopyTo(targetDir.ChildDirectory("img"));

BuildZ2C();

void CompileChapterMarkdown(string htmlTemplateText, FileInfo mdFile, FileInfo targetFile)
{
    var mdDoc = Markdown.Parse(mdFile.ReadAllText());
    string mdHtml = mdDoc.ToHtml();
    string htmlText = htmlTemplateText.Replace("$chapter$", mdHtml);
    targetFile.WriteAllText(htmlText);
}

void BuildZ2C()
{
    var z2cDir = sourceDir.ChildDirectory("zero2compiler");
    var templateFile = z2cDir.ChildFile("template.html");
    //templateFile.CopyTo(targetDir.ChildFile("contents.html"));

    string htmlTemplateText = templateFile.ReadAllText();
    CompileChapterMarkdown(htmlTemplateText, z2cDir.ChildDirectory("1-introduction").ChildFile("chapter1.md"), targetDir.ChildFile("chapter1.html"));
}
