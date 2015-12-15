using FsCheck;
using FS3.Types;

namespace FS3.Testing
{
    public static class MovesArbitrary
    {
        public static Arbitrary<Move> Move()
        {
            var genMove =   from x in Arb.Generate<int>()
                            from y in Arb.Generate<int>()
                            select new Move(x, y);

            return genMove.ToArbitrary();
        } 
    }
}