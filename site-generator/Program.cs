using System.Diagnostics;
using SiteGenerator;
using SiteGenerator.CompilerBook;

if (Debugger.IsAttached)
{
    var cwd = new DirectoryInfo(Environment.CurrentDirectory);
    while (!cwd.ChildDirectory("src").Exists)
        cwd = cwd.Parent;
    Environment.CurrentDirectory = cwd.FullName;
}

var sourceDir = new DirectoryInfo("src");
var targetDir = new DirectoryInfo("public_html");

var bookDir = sourceDir.ChildDirectory("compiler-book");

targetDir.Delete(true);
targetDir.Create();

sourceDir.CopyTo(targetDir, false);
targetDir.ChildFile("template.html").Delete();

sourceDir.ChildDirectory("js").CopyTo(targetDir.ChildDirectory("js"));
sourceDir.ChildDirectory("css").CopyTo(targetDir.ChildDirectory("css"));
sourceDir.ChildDirectory("img").CopyTo(targetDir.ChildDirectory("img"));

var bookParser = new CompilerBookChapterParser();

var bookTree = new CompilerBookTree(bookDir, bookParser);

bookTree.AddSection("lexical-analysis.md", []);

var bookBuilder = new CompilerBookBuilder(bookDir);
bookBuilder.Build(bookTree, targetDir.ChildDirectory("compiler-book"));
