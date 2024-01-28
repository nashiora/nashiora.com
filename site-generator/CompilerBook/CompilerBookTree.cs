using Markdig;
using Markdig.Syntax;

namespace SiteGenerator.CompilerBook;

public sealed class CompilerBookTree(DirectoryInfo rootDirectory, CompilerBookChapterParser parser)
{
    public DirectoryInfo RootDirectory { get; } = rootDirectory;
    public CompilerBookChapterParser ChapterParser { get; } = parser;
    public CompilerBookSection[] Sections => [.. _sections];

    private readonly List<CompilerBookSection> _sections = [];
    private int _chapterCount = 0;

    public void AddSection(string sectionFileName, string[] chapterFileNames)
    {
        var chapters = new List<CompilerBookChapter>();
        foreach (string chapterFileName in chapterFileNames)
        {
            var chapterSourceFile = RootDirectory.ChildFile(chapterFileName);
            int chapterNumber = ++_chapterCount;

            MarkdownDocument chapterDoc = ChapterParser.Parse(chapterSourceFile, [
                new("$chapter$", chapterNumber.ToString())
            ]);

            var chapter = new CompilerBookChapter()
            {
                ChapterNumber = chapterNumber,
                SourceFile = chapterSourceFile,
                MarkdownDocument = chapterDoc,
            };

            chapters.Add(chapter);
        }

        var sectionSourceFile = RootDirectory.ChildFile(sectionFileName);
        int sectionNumber = _sections.Count + 1;

        var sectionDoc = ChapterParser.Parse(sectionSourceFile, [
            new("$section$", sectionNumber.ToString())
        ]);

        var section = new CompilerBookSection()
        {
            SectionNumber = sectionNumber,
            SourceFile = sectionSourceFile,
            Chapters = [.. chapters],
            MarkdownDocument = sectionDoc,
        };

        _sections.Add(section);
    }
}

public sealed class CompilerBookSection
{
    public required int SectionNumber { get; init; }
    public required FileInfo SourceFile { get; init; }

    public required CompilerBookChapter[] Chapters { get; init; }

    public required MarkdownDocument MarkdownDocument { get; init; }
}

public sealed class CompilerBookChapter
{
    public required int ChapterNumber { get; init; }
    public required FileInfo SourceFile { get; init; }

    public required MarkdownDocument MarkdownDocument { get; init; }
}
