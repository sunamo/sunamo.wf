namespace forms
{
    public class PicturesForms
    {
        public static string InfoAbout(Bitmap bmp)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Width: " + bmp.Width);
            sb.AppendLine("Height: " + bmp.Height);
            return sb.ToString();
        }
    }
}
