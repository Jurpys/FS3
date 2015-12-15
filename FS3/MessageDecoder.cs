using System;
using System.Collections.Generic;
using System.Linq;
using FS3.Types;

namespace FS3
{
    //returns decoded string and rest of the message
    public delegate Tuple<string, string> StringDecoder(string s);

    public static class MessageDecoder
    {
        private static StringDecoder DecodeInt;
        private static StringDecoder DecodeString;

        public static Dictionary<string, Dictionary<string, string>> Execute(string message)
        {
            DecodeInt = o =>
            {
                o = o.Substring(1);
                var splitString = o.Split(new[] { 'e' }, 2);

                return new Tuple<string, string>(splitString[0], splitString[1]);
            };

            DecodeString = o =>
            {
                var splitString = o.Split(new[] { ':' }, 2);
                var charsQuantity = int.Parse(splitString[0]);

                return new Tuple<string, string>(splitString[1].Substring(0, charsQuantity), splitString[1].Substring(charsQuantity));
            };

            return DecodeFullMessage(message, new Dictionary<string, Dictionary<string, string>>()).Item2;
        }

        private static Tuple<string, Dictionary<string, Dictionary<string, string>>> DecodeFullMessage(string message, 
            Dictionary<string, Dictionary<string, string>> externalDictionary)
        {
            if (message.Length == 0)
            {
                return new Tuple<string, Dictionary<string, Dictionary<string, string>>>(message, externalDictionary);
            }
            switch (message.Substring(0, 1))
            {
                case "d":
                case "e":
                    DecodeFullMessage(message.Substring(1), externalDictionary);
                    break;
                default:
                    var pair = DecodeString(message);
                    var innerPair = DecodeInnerDictionary(pair.Item2, new Dictionary<string, string>());

                    externalDictionary.Add(pair.Item1, innerPair.Item2);
                    DecodeFullMessage(innerPair.Item1, externalDictionary);
                    break;
            }

            return new Tuple<string, Dictionary<string, Dictionary<string, string>>>("", externalDictionary);
        }

        private static Tuple<string, Dictionary<string, string>> DecodeInnerDictionary(string message,
            Dictionary<string, string> innerDictionary)
        {
            switch (message.Substring(0, 1))
            {
                case "e":
                    return new Tuple<string, Dictionary<string, string>>(message, innerDictionary);
                case "d":
                    return DecodeInnerDictionary(message.Substring(1), innerDictionary);
                default:
                    var KeyPair = DecodeElement(message);
                    var ValuePair = DecodeElement(KeyPair.Item2);

                    innerDictionary.Add(KeyPair.Item1, ValuePair.Item1);

                    return DecodeInnerDictionary(ValuePair.Item2, innerDictionary);
            }
        }

        private static Tuple<string, string> DecodeElement(string message)
        {
            switch (message.Substring(0, 1))
            {
                case "i":
                    return DecodeInt(message);
                default:
                    return DecodeString(message);
            }
        }

        public static List<Move> DecodeToCoordinates(Dictionary<string, Dictionary<string, string>> externalDictionary)
        {
            return DecodeMoves(new List<Move>(), externalDictionary);
        }

        private static List<Move> DecodeMoves(
            List<Move> movesList,
            Dictionary<string, Dictionary<string, string>> externalDictionary)
        {
            if (externalDictionary.Count == 0)
            {
                return movesList;
            }
            var poppedDictionary = externalDictionary.First();
            var x = int.Parse(poppedDictionary.Value["x"]);
            var y = int.Parse(poppedDictionary.Value["y"]);

            movesList.Add(new Move(x, y));
            var filteredDictionary =
                externalDictionary.Where(o => o.Key != poppedDictionary.Key).ToDictionary(v => v.Key, v => v.Value);

            var allMoves = DecodeMoves(movesList, filteredDictionary);

            return movesList;
        }
    }
}