using System;

namespace FS3.Types
{
    public class Move : Tuple<int, int>
    {
        public Move(int item1, int item2) : base(item1, item2)
        {
        }
    }
}