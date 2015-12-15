using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS3;
using FS3;
using FS3.Enum;

namespace FS3
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine(Game.Play("opa_opa44"));
            Console.ReadKey();

            //var message = Maybe<string>.Bind("d1:0d1:v1:x1:xi2e1:yi1ee1:1d1:v1:o1:xi1e1:yi2ee1:2d1:v1:x1:xi0e1:yi0ee1:3d1:v1:o1:xi0e1:yi2ee1:4d1:v1:x1:xi2e1:yi0ee1:5d1:v1:o1:xi2e1:yi2eee");

            //Func<string, Dictionary<string, Dictionary<string, string>>> decoderToExternalMap = MessageDecoder.Execute;
            //Func<Dictionary<string, Dictionary<string, string>>, List<Tuple<int, int>>> decoderToCoordinates =
            //    MessageDecoder.DecodeToCoordinates;
            //Func<List<Tuple<int, int>>, Tuple<int, int>> randomFinder = MoveFinder.GetRandomMove;

            //var winner = message.FMap(decoderToExternalMap).FMap(decoderToCoordinates).FMap(randomFinder);

            //Console.WriteLine(winner.ToString());
            //Console.ReadLine();
        }
    }
}
