using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS3
{
    public static class MoveFinder
    {
        private static List<Tuple<int, int>> _allMoves = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(0, 2),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(1, 1),
            new Tuple<int, int>(1, 2),
            new Tuple<int, int>(2, 0),
            new Tuple<int, int>(2, 1),
            new Tuple<int, int>(2, 2),
        };

        public static Tuple<int, int> GetRandomMove(List<Tuple<int,int>> movesMade)
        {
            var freeMoves = GetFreeMoves(movesMade, _allMoves);

            var r = new Random();

            var randomNumber = r.Next(freeMoves.Count);

            return freeMoves.ElementAt(randomNumber);
        }

        private static List<Tuple<int, int>> GetFreeMoves(List<Tuple<int, int>> movesMade,
            List<Tuple<int, int>> movesLeft)
        {
            if (movesMade.Count == 0)
            {
                return movesLeft;
            }
            var poppedMove = movesMade.First();
            var movesMadeLeft = movesMade.Where(o => !o.Equals(poppedMove)).ToList();
            var filteredMoves = movesLeft.Where(o => !o.Equals(poppedMove)).ToList();

            var allMovesLeft = GetFreeMoves(movesMadeLeft, filteredMoves);

            return allMovesLeft;
        }
    }
}
