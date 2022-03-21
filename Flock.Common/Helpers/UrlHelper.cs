using System.Collections.Specialized;

namespace Flock.Common.Helpers
{
    public static class UrlHelper
    {
        public static string BuildUrl(string baseUrl, string section, NameValueCollection queryParams)
        {
            var url = baseUrl + section;
            if (queryParams != null && queryParams.Count > 0)
            {
                return url + "?" + queryParams.ToString();
            }
            return url;
        }
    }
}
