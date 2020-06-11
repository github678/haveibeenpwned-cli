using System;
using System.Security.Cryptography;
using System.Text;

namespace HaveIBeenPawned
{
    public static class PawnageHelper
    {
        /// <summary>
        /// Gets the one-line pawnage info from the haveibeenpwned.com API pertaining to the provided <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password for which we want to get the pawnage info.</param>
        /// <returns>A string which contains the SHA1 hash of the <paramref name="password"/> and the number of times 
        /// it's been known to be pawned, separated by a ':'.</returns>
        public static string GetPawnageInfo(string password)
        {
            // computing password hash and converting it to HEX string
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hexString = hash.ConvertToHexString();

            // we only send the first 5 letters of the password hash's HEX
            var url = "https://api.pwnedpasswords.com/range/" + hexString.Substring(0, 5);
            var webApiResponseContent = WebHelper.GetWebResponseContent(url);
            var lines = webApiResponseContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.ToLower().Contains(hexString.Substring(5).ToLower()))
                {
                    // we can return because hashes are supposed to be unique. If we found one, that's it.
                    return line;
                }
            }

            return null;
        }
    }
}
