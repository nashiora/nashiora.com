namespace SiteGenerator.BlogPosts;

public sealed class BlogBuilder(DirectoryInfo blogDir) {
    public DirectoryInfo BlogDirectory { get; } = blogDir;

    public void Build(DirectoryInfo targetDir) {
        var blogPosts = BlogDirectory.EnumerateFiles().ToArray();

        // put the blog listing here woot
        var indexFile = targetDir.ChildFile("index.html");
    }
}
