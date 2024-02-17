using SiteGenerator;
using SiteGenerator.BlogPosts;
using SiteGenerator.CompilerBook;

using System.Diagnostics;

if (Debugger.IsAttached)
{
    var cwd = new DirectoryInfo(Environment.CurrentDirectory);
    while (cwd is not null && !cwd.ChildDirectory("src").Exists)
        cwd = cwd.Parent;

    if (cwd is not null)
        Environment.CurrentDirectory = cwd.FullName;
    
    Debug.Assert(new DirectoryInfo(Environment.CurrentDirectory).ChildDirectory("src").Exists, "Where tf is the `src` directory");
}

var sourceDir = new DirectoryInfo("src");
var targetDir = new DirectoryInfo("public_html");

targetDir.Delete(true);
targetDir.Create();

sourceDir.CopyTo(targetDir, false);
targetDir.ChildFile("template.html").Delete();

sourceDir.ChildDirectory("js").CopyTo(targetDir.ChildDirectory("js"));
sourceDir.ChildDirectory("css").CopyTo(targetDir.ChildDirectory("css"));
sourceDir.ChildDirectory("img").CopyTo(targetDir.ChildDirectory("img"));

BuildBlog();
BuildCompilerBook();

void BuildBlog() {
    var blogDir = sourceDir.ChildDirectory("blog");

    var blogBuilder = new BlogBuilder(blogDir);
    blogBuilder.Build(targetDir.ChildDirectory("blog"));
}

void BuildCompilerBook() {
    var bookDir = sourceDir.ChildDirectory("compiler-book");
    var bookParser = new CompilerBookChapterParser();
    var bookTree = new CompilerBookTree(bookDir, bookParser);

    bookTree.AddUnit("introduction.md", [
        "prerequisite-knowledge.md",
        "hibiku-reference.md",
    ]);

    bookTree.AddUnit("foundations.md", [
        "conventions.md",
        "dynamic-arrays.md",
        "pool-allocators.md",
        "strings.md",
        "compilation-state.md",
    ]);

    bookTree.AddUnit("from-source-to-syntax.md", [
        "cartography.md",
        "lexical-analysis.md",
        "syntax-tree.md",
        "debug-utilities.md",
        "syntactic-analysis.md",
    ]);

    bookTree.AddUnit("the-middle-end.md", [
        "semantic-analysis.md",
        "designing-a-register-bytecode.md",
        "bytecode-generation.md",
        "the-virtual-machine.md",
        "compile-time-execution.md",
        "optimization-passes.md",
    ]);

    bookTree.AddUnit("code-generation.md", [
        "generating-llvm.md",
        "machine-ir.md",
        "register-allocation.md",
        "generating-asm.md",
        "generating-wasm.md",
    ]);

    var bookBuilder = new CompilerBookBuilder(bookDir);
    bookBuilder.Build(bookTree, targetDir.ChildDirectory("compiler-book"));
}
