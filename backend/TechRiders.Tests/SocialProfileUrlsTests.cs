using TechRiders.Application.Social;

namespace TechRiders.Tests;

public sealed class SocialProfileUrlsTests
{
    [Fact]
    public void Normalize_EmptyValue_ReturnsNull()
    {
        var result = SocialProfileIdentifier.Normalize("  ");

        Assert.Null(result);
    }

    [Fact]
    public void Normalize_AtPrefixedIdentifier_RemovesPrefix()
    {
        var result = SocialProfileIdentifier.Normalize(" @techriders ");

        Assert.Equal("techriders", result);
    }

    [Fact]
    public void Normalize_CompleteUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => SocialProfileIdentifier.Normalize("https://github.com/techriders"));
    }

    [Fact]
    public void Build_NullIdentifier_ReturnsNull()
    {
        var result = SocialProfileUrls.Build(SocialProfileUrls.LinkedIn, null);

        Assert.Null(result);
    }

    [Fact]
    public void Build_Identifier_ReturnsComposedUrl()
    {
        var result = SocialProfileUrls.Build(SocialProfileUrls.GitHub, "techriders");

        Assert.Equal("https://github.com/techriders", result);
    }
}
