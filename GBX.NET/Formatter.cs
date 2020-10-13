using System.Text;
using System.Text.RegularExpressions;

namespace GBX.NET
{
    public static class Formatter
    {
        public static string Deformat(string str)
        {
            StringBuilder b = new StringBuilder(str);

            foreach (var f in new string[] { "$w", "$o", "$n", "$s", "$i", "$z", "$W", "$O", "$N", "$S", "$I", "$Z", "$g", "$t", "$<", "$>" })
                b.Replace(f, null);

            var final = Regex.Replace(b.ToString(), "$(l|h)[.*?](.*?)$(l|h)", "$2", RegexOptions.IgnoreCase);
            final = Regex.Replace(final, "$(l|h)[.*?]", "", RegexOptions.IgnoreCase);
            final = final.Replace("$l", "");
            final = final.Replace("$h", "");
            final = Regex.Replace(final, @"\$[a-fA-F0-9]{3}", "", RegexOptions.IgnoreCase);
            final = Regex.Replace(final, @"\$[a-fA-F0-9]{2}", "", RegexOptions.IgnoreCase);
            final = Regex.Replace(final, @"\$[a-fA-F0-9]", "", RegexOptions.IgnoreCase);

            return final;
        }
    }
}
