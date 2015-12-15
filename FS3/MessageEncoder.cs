using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS3
{
    public static class MessageEncoder
    {
        public static string UpdateMessage(string message, string key, Tuple<int, int> move)
        {
            return message.Substring(0, message.Length - 1) + "1:"+ key + "d1:v1:o1:xi"+ move.Item1 + "e1:yi" + move.Item2 + "eee";
        }
    }
}
