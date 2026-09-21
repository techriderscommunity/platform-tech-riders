namespace TechRiders.Application.Social;

public static class SocialProfileUrls
{
    public const string LinkedIn = "https://www.linkedin.com/in/";
    public const string LinkedInCompany = "https://www.linkedin.com/company/";
    public const string LinkedInSchool = "https://www.linkedin.com/school/";
    public const string Instagram = "https://www.instagram.com/";
    public const string X = "https://x.com/";
    public const string YouTube = "https://www.youtube.com/@";
    public const string GitHub = "https://github.com/";

    public static string? Build(string? baseUrl, string? identifier)
    {
        return string.IsNullOrWhiteSpace(identifier)
            ? null
            : $"{baseUrl}{identifier.Trim().TrimStart('@').TrimEnd('/')}";
    }
}

public static class SocialProfileIdentifier
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var identifier = value.Trim().TrimStart('@').Trim('/');
        if (identifier.IndexOfAny(['/', '\\', '?', '#', ':']) >= 0)
        {
            throw new ArgumentException("Use only the social profile identifier, not the complete URL.");
        }

        return identifier.Length == 0 ? null : identifier;
    }
}
