using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS3.Enum;

namespace FS3
{
    public static class WinnerDeterminator
    {
        public static MarkType FindWinner(Dictionary<string, Dictionary<string, string>> externalDictionary)
        {
            var winner = CheckColumns(externalDictionary, 0, Criteria.Column);

            if (winner == MarkType.Nothing)
            {
                return CheckDiagonals(externalDictionary, Criteria.Diagonal);
            }

            return winner;
        }

        private static MarkType CheckDiagonals(Dictionary<string, Dictionary<string, string>> externalDictionary,
            Criteria criteria)
        {
            var diagonal =
                externalDictionary.Where(
                    o =>
                        criteria == Criteria.Diagonal
                            ? o.Value["x"].Equals(o.Value["y"])
                            : int.Parse(o.Value["x"]) + int.Parse(o.Value["y"]) == 2).Select(o => o.Value).ToList();

            var mark = Check(diagonal, MarkType.Nothing, 0);

            if (mark != MarkType.Nothing)
            {
                return mark;
            }

            if (criteria == Criteria.Diagonal2)
            {
                return MarkType.Nothing;
            }

            return CheckDiagonals(externalDictionary, Criteria.Diagonal2);
        }

        private static MarkType CheckColumns(Dictionary<string, Dictionary<string, string>> externalDictionary, int columnNo, Criteria criteria)
        {
            if (columnNo > 2 && criteria == Criteria.Column)
            {
                return CheckColumns(externalDictionary, 0, Criteria.Row);
            }
            if (columnNo > 2 && criteria == Criteria.Row)
            {
                return MarkType.Nothing;
            }

            var column =
                externalDictionary.Where(o => o.Value[criteria == Criteria.Column ? "y" : "x"].Equals(columnNo.ToString()))
                    .Select(o => o.Value)
                    .ToList();

            var mark = Check(column, MarkType.Nothing, 0);

            if (mark != MarkType.Nothing)
            {
                return mark;
            }
            
            return CheckColumns(externalDictionary, columnNo + 1, criteria);
        }

        private static MarkType Check(List<Dictionary<string, string>> externalDictinary, MarkType markToFollow, int marksCount)
        {
            if (marksCount == 3)
            {
                return markToFollow;
            }

            if (externalDictinary.Count == 0)
            {
                return MarkType.Nothing;
            }

            var singleDictionary = PopInternalDictionary(externalDictinary);
            var mark = MarkConverter.Convert(singleDictionary.Item1["v"]);

            if (mark != markToFollow && markToFollow != MarkType.Nothing)
            {
                return MarkType.Nothing;
            }

            return Check(singleDictionary.Item2, mark, marksCount + 1);
        }

        private static Tuple<Dictionary<string, string>, List<Dictionary<string, string>>> PopInternalDictionary(
            List<Dictionary<string, string>> externalDictinary)
        {
            var first = externalDictinary.First();
            var left = externalDictinary.Where(o => o != first).ToList();

            return new Tuple<Dictionary<string, string>, List<Dictionary<string, string>>>(first, left);
        }
    }
}
