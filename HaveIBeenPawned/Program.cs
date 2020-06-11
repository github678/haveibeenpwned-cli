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
using System.Text;

namespace HaveIBeenPawned
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    DisplayHelp();
                }
                else if (args.Length == 1)
                {
                    var pawnageInfo = PawnageHelper.GetPawnageInfo(args[0]);
                    if (pawnageInfo is null)
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
            catch (Exception exception)
            {
                Console.WriteLine("Error occurred.");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Displays the help message for the app on the console.
        /// </summary>
        private static void DisplayHelp()
        {
            var helpText = new StringBuilder();
            helpText.AppendLine("This tool calls the haveibeenpwned.com web API to find out if the provided");
            helpText.AppendLine("password has been known to be pawned. (The password should contain no whitespace");
            helpText.AppendLine("characters.) Don't worry, this is safe, your password will not be sent out to the");
            helpText.AppendLine("internet.");
            helpText.AppendLine();
            helpText.AppendLine("Usage: haveibeenpwned MyPassword");
            Console.WriteLine(helpText.ToString());
        }
    }
}