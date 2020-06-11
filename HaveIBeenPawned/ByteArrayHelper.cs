using System.Text;

namespace HaveIBeenPawned
{
    public static class ByteArrayHelper
    {
        /// <summary>
        /// Converts the specified <see cref="byte"/>[] to a HEX string.
        /// </summary>
        /// <param name="array">The <see cref="byte"/> array to convert to a HEX string.</param>
        /// <returns>The <see cref="byte"/>[] as a HEX string.</returns>
        public static string ConvertToHexString(this byte[] array)
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
