/* This is the command line utility "haveibeenpwned" which may be used to determine if a given password
 has ever been pawned - according to haveibeenpwned.com.
 
 Copyright 2019, Mátyás Seress

 Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
 and associated documentation files (the "Software"), to deal in the Software without restriction, 
 including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
 subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
 TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HaveIBeenPawned
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayHelp();
            }
            else if (args.Length == 1)
            {
                var pawnageInfo = GetPawnageInfo(args[0]);
                if (pawnageInfo == null)
                {
                    Console.WriteLine("You haven't been pawned - according to haveibeenpwned.com.");
                }
                else
                {
                    Console.WriteLine("According to haveibeenpwned.com you have been pawned.");
                    Console.WriteLine("SHA1 hash of pawned password: {0}", pawnageInfo.Split(':')[0]);
                    Console.WriteLine("The given password has been pawned {0} times.", pawnageInfo.Split(':')[1].Trim());
                }
            }
            else
            {
                Console.WriteLine("You provided too many arguments.");
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Gets the one-line pawnage info from the haveibeenpwned.com API pertaining to the provided <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password for which we want to get the pawnage info.</param>
        /// <returns>A string which contains the SHA1 hash of the <paramref name="password"/> and the number of times 
        /// it's been known to be pawned, separated by a ':'.</returns>
        private static string GetPawnageInfo(string password)
        {
            // computing password hash and converting it to HEX string
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hexString = ConvertToHexString(hash);

            // we only send the first 5 letters of the password hash's HEX
            var url = "https://api.pwnedpasswords.com/range/" + hexString.Substring(0, 5);
            var webApiResponseContent = GetWebResponseContent(url);
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

        /// <summary>
        /// Gets the content of the response from the web server.
        /// </summary>
        /// <param name="url">The URL to call the web server.</param>
        /// <returns>The response content.</returns>
        private static string GetWebResponseContent(string url)
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

        /// <summary>
        /// Displays the help message for the app on the console.
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("This tool calls the haveibeenpwned.com web API to find out if the provided password has been known to be pawned." +
                " Don't worry, this is safe, your password will not be sent out to the internet.");
            Console.WriteLine("Usage: haveibeenpwned MyPassword");
        }

        /// <summary>
        /// Converts the specified <see cref="byte"/>[] to a HEX string.
        /// </summary>
        /// <param name="array">The <see cref="byte"/> array to convert to a HEX string.</param>
        /// <returns>The <see cref="byte"/>[] as a HEX string.</returns>
        private static string ConvertToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
                
            return hex.ToString();
        }
    }
}