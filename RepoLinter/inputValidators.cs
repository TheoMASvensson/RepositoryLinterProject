public class URLValidator
{
    public static bool IsValidURL(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class pathValidator
{
    
    public static bool isValidPath(string path)
    {
        if(Directory.Exists(path))
        {
            return true;
        }
        return false;
    }
}