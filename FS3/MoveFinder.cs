using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS3.Types;

namespace FS3
{
    public static class MoveFinder
    {
        private static List<Move> _allMoves = new List<Move>()
        {
            new Move(0, 0),
            new Move(0, 1),
            new Move(0, 2),
            new Move(1, 0),
            new Move(1, 1),
            new Move(1, 2),
            new Move(2, 0),
            new Move(2, 1),
            new Move(2, 2),
        };

        public static Move GetRandomMove(List<Move> movesMade)
        {
            var freeMoves = GetFreeMoves(movesMade, _allMoves);

            var r = new Random();

            var randomNumber = r.Next(freeMoves.Count);

            return freeMoves.ElementAt(randomNumber);
        }

        private static List<Move> GetFreeMoves(List<Move> movesMade,
            List<Move> movesLeft)
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
