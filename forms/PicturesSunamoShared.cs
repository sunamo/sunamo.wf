public partial class PicturesSunamo
{


    public static string ExtensionFromImage(Image mg)
    {
        var imf2 = mg.RawFormat;
        var imf = imf2.Guid;
        if (imf == ImageFormat.Jpeg.Guid)
        {
            return AllExtensions.jpg;
        }
        else if (imf == ImageFormat.Gif.Guid)
        {
            return AllExtensions.gif;
        }
        else if (imf == ImageFormat.Bmp.Guid)
        {
            return AllExtensions.bmp;
        }
        else if (imf == ImageFormat.Icon.Guid)
        {
            return AllExtensions.ico;
        }
        else if (imf == ImageFormat.Tiff.Guid)
        {
            return AllExtensions.tiff;
        }
        else if (imf == ImageFormat.Wmf.Guid)
        {
            return AllExtensions.wmf;
        }
        else if (imf == ImageFormat.Emf.Guid)
        {
            return AllExtensions.emf;
        }
        else if (imf == ImageFormat.Exif.Guid)
        {
            return AllExtensions.exif;
        }
        else if (imf == ImageFormat.MemoryBmp.Guid)
        {
            return AllExtensions.bmp;
        }
        else
        {
            ThrowEx.NotImplementedCase(imf);
        }
        return null;
    }











}
