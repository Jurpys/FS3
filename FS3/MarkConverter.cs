using FS3.Enum;

namespace FS3
{
    public static class MarkConverter
    {
        public static MarkType Convert(string stringValue)
        {
            switch (stringValue)
            {
                case "x":
                case "X":
                    return MarkType.X;
                case "o":
                case "O":
                case "0":
                    return MarkType.O;
                default:
                    return MarkType.Nothing;
            }
        }
    }
}