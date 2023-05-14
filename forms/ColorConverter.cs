namespace sunamo.ConvertersSimple
{
    public static class ColorConverter //: ISimpleConverter<Color, string>
    {
        public static Color ConvertTo(string u)
        {
            var d2 = SF.GetAllElementsLine(u);
            var d = CA.ToInt(d2);
            return Color.FromArgb((byte)d[0], (byte)d[1], (byte)d[2], (byte)d[3]);
        }

        public static string ConvertFrom(Color t)
        {
            return SF.PrepareToSerialization2(CA.ToListString(t.A, t.R, t.G, t.B));
        }
    }
}
