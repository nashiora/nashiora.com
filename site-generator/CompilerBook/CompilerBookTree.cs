using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace SiteGenerator.CompilerBook;

public sealed class CompilerBookTree(DirectoryInfo rootDirectory, CompilerBookChapterParser parser)
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

    public DirectoryInfo RootDirectory { get; } = rootDirectory;
    public CompilerBookChapterParser ChapterParser { get; } = parser;
    public CompilerBookSection[] Sections => [.. _sections];

    private readonly List<CompilerBookSection> _sections = [];
    private int _chapterCount = 0;

    public void AddSection(string sectionFileName, string[] chapterFileNames)
    {

        var sectionSourceFile = RootDirectory.ChildFile(sectionFileName);
        int sectionNumber = _sections.Count + 1;

        var chapters = new List<CompilerBookChapter>();
        foreach (string chapterFileName in chapterFileNames)
        {
            var chapterSourceFile = RootDirectory.ChildFile(chapterFileName);
            int chapterNumber = ++_chapterCount;

            MarkdownDocument chapterDoc = ChapterParser.Parse(chapterSourceFile, [
                new("$chapter$", chapterNumber.ToString()),
                new("$section$", sectionNumber.ToString()),
                new("$section-roman$", roman[sectionNumber - 1]),
            ]);

            var chapter = new CompilerBookChapter()
            {
                ChapterNumber = chapterNumber,
                SourceFile = chapterSourceFile,
                MarkdownDocument = chapterDoc,
            };

            chapters.Add(chapter);
        }

        var sectionDoc = ChapterParser.Parse(sectionSourceFile, [
            new("$chapter$", "0"),
            new("$section$", sectionNumber.ToString()),
            new("$section-roman$", roman[sectionNumber - 1]),
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

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(SourceFile.Name);

    public string SectionTitle
    {
        get
        {
            var firstHeader = (HeadingBlock)MarkdownDocument.First(x => x is HeadingBlock);
            string headerText = ((LiteralInline)firstHeader.Inline!.FirstChild!).Content.ToString();
            return headerText;
        }
    }
}

public sealed class CompilerBookChapter
{
    public required int ChapterNumber { get; init; }
    public required FileInfo SourceFile { get; init; }

    public required MarkdownDocument MarkdownDocument { get; init; }

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(SourceFile.Name);

    public string ChapterTitle
    {
        get
        {
            var firstHeader = (HeadingBlock)MarkdownDocument.First(x => x is HeadingBlock);
            string headerText = ((LiteralInline)firstHeader.Inline!.FirstChild!).Content.ToString();
            return headerText;
        }
    }
}
