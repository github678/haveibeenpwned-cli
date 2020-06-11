using System.IO;
using System.Net;

namespace HaveIBeenPawned
{
    public static class WebHelper
    {
        /// <summary>
        /// Gets the content of the response from the web server.
        /// </summary>
        /// <param name="url">The URL to call the web server.</param>
        /// <returns>The response content.</returns>
        public static string GetWebResponseContent(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            using (var dataStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
