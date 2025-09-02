namespace OF.ConsentManagement.Common.Helpers;

public static class UrlHelper
{
    public static string CombineUrl(string baseUrl, string relativePath)
    {
        return $"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }
}
