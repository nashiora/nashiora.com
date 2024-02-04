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
