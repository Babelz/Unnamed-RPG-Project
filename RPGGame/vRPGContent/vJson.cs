using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent
{
    public static class vJson
    {
        public static readonly string Comment = "//";
        
        public static string[] RemoveComments(string[] lines)
        {
            return lines.Where(l => !l.Trim().StartsWith(Comment)).ToArray();
        }

        public static string Join(string[] lines)
        {
            var sb = new StringBuilder();

            for (var i = 0; i <lines.Length; i++) sb.Append(lines[i]);

            return sb.ToString();
        }
    }
}
