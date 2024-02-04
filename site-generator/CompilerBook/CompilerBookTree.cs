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
    public CompilerBookUnit[] Units => [.. _units];

    private readonly List<CompilerBookUnit> _units = [];
    private int _chapterCount = 0;

    public void AddUnit(string unitFileName, string[] chapterFileNames)
    {

        var unitSourceFile = RootDirectory.ChildFile(unitFileName);
        int unitNumber = _units.Count + 1;

        var chapters = new List<CompilerBookChapter>();
        foreach (string chapterFileName in chapterFileNames)
        {
            var chapterSourceFile = RootDirectory.ChildFile(chapterFileName);
            int chapterNumber = ++_chapterCount;

            MarkdownDocument chapterDoc = ChapterParser.Parse(chapterSourceFile, [
                new("$chapter$", chapterNumber.ToString()),
                new("$unit$", unitNumber.ToString()),
                new("$unit-roman$", roman[unitNumber - 1]),
            ]);

            var chapter = new CompilerBookChapter()
            {
                ChapterNumber = chapterNumber,
                SourceFile = chapterSourceFile,
                MarkdownDocument = chapterDoc,
            };

            chapters.Add(chapter);
        }

        var unitDoc = ChapterParser.Parse(unitSourceFile, [
            new("$chapter$", "0"),
            new("$unit$", unitNumber.ToString()),
            new("$unit-roman$", roman[unitNumber - 1]),
        ]);

        var unit = new CompilerBookUnit()
        {
            UnitNumber = unitNumber,
            SourceFile = unitSourceFile,
            Chapters = [.. chapters],
            MarkdownDocument = unitDoc,
        };

        _units.Add(unit);
    }
}

public sealed class CompilerBookUnit
{
    public required int UnitNumber { get; init; }
    public required FileInfo SourceFile { get; init; }

    public required CompilerBookChapter[] Chapters { get; init; }

    public required MarkdownDocument MarkdownDocument { get; init; }

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(SourceFile.Name);

    public string UnitTitle
    {
        get
        {
            var firstHeader = (HeadingBlock?)MarkdownDocument.FirstOrDefault(x => x is HeadingBlock);
            string headerText = ((LiteralInline)firstHeader?.Inline!.FirstChild!)?.Content.ToString() ?? "MissingTitle";
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
            var firstHeader = (HeadingBlock?)MarkdownDocument.FirstOrDefault(x => x is HeadingBlock);
            string headerText = ((LiteralInline)firstHeader?.Inline!.FirstChild!)?.Content.ToString() ?? "MissingTitle";
            return headerText;
        }
    }
}
