public class PicturesHelperFw
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

    #region Nevím proč to muselo být v fw ale způsobovalo to problémy - shared je .net standard, fw net48. Vracím to zpět do std a modlím se ať to dobře dopadne.
    /// <summary>
    /// A5 je instance originálního(velkého/původního) obrázku
    /// Nevím proč to muselo být v fw ale způsobovalo to problémy - shared je .net standard, fw net48. Vracím to zpět do std a modlím se ať to dobře dopadne.
    /// </summary>
    /// <param name="toFolderTempSlash"></param>
    /// <param name="fnwoe"></param>
    /// <param name="ext"></param>
    /// <param name="finalMin"></param>
    /// <param name="bmp"></param>
    /// <param name="borderColor"></param>
    public static void CreateThumbnail(string toFolderTempSlash, string fnwoe, string ext, string finalMin, System.Drawing.Image bmp, Color borderColor, int tnHeight, int tnWidth)
    {
        System.Drawing.Size sizeMin = bmp.Size;
        Point pointMin = new Point();
        if (bmp.Width > tnWidth || bmp.Height > tnHeight)
        {
            if (bmp.Height > bmp.Width)
            {
                //// Menší je šířka, vypočtu optimální velikost obrázku, kde specifikuji výšku 168
                //sizeMin = PicturesSunamo.CalculateOptimalSizeHeight(bmp.Width, bmp.Height, tnHeight).ToSystemDrawing();
                //bool vypocistIVysku = false;
                //while (sizeMin.Width > tnWidth)
                //{
                //    vypocistIVysku = true;
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //while (sizeMin.Height > tnHeight)
                //{
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //pointMin = new Point(Divide(tnWidth - sizeMin.Width), 0);
                //if (vypocistIVysku)
                //{
                //    pointMin = new Point(pointMin.X, Divide(tnHeight - sizeMin.Height));
                //}
            }
            else if (bmp.Width > bmp.Height)
            {
                //sizeMin = PicturesSunamo.CalculateOptimalSize(bmp.Width, bmp.Height, tnWidth).ToSystemDrawing();
                //bool vypocistISirku = false;
                //while (sizeMin.Height > tnHeight)
                //{
                //    vypocistISirku = true;
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //while (sizeMin.Width > tnWidth)
                //{
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //pointMin = new Point(0, Divide(tnHeight - sizeMin.Height));
                //if (vypocistISirku)
                //{
                //    pointMin = new Point(Divide(tnWidth - sizeMin.Width), pointMin.Y);
                //}
            }
        }
        else
        {
            // Buď výška nebo šířka byla menší než minimální, zjistím co a obrázek umístím na střed
            while (sizeMin.Height > tnHeight && sizeMin.Width > tnWidth)
            {
                sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
            }

            pointMin = new Point(Divide(tnWidth - sizeMin.Width), Divide(tnHeight - sizeMin.Height));
        }

        //bool zmensovatMiniaturu = true;
        if (true)
        {
            string tempMin = toFolderTempSlash + fnwoe + "_tn" + ext;
            FS.CreateUpfoldersPsysicallyUnlessThere(tempMin);
            TransformImage(bmp, sizeMin.Width, sizeMin.Height, tempMin);
            using (System.Drawing.Image bmpMin = Bitmap.FromFile(tempMin))
            {
                using (Bitmap b2 = new Bitmap(tnWidth, tnHeight))
                {
                    using (Graphics g = Graphics.FromImage(b2))
                    {
                        g.Clear(borderColor);
                        Rectangle rect = new Rectangle(pointMin, sizeMin);
                        g.DrawImage(bmpMin, rect);
                        using (MemoryStream mOutput = new MemoryStream())
                        {
                            b2.Save(mOutput, ImageFormat.Jpeg);
                            Byte[] array = mOutput.ToArray();
                            TF.WriteAllBytesArray(finalMin, array);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Po uložení obrázku jej i všechny ostatní prostředky zlikviduje.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="path"></param>
    public static void TransformImage(System.Drawing.Image image, int width, int height, string path)
    {
        #region Zakomentováno, z důvodu že mi to špatně zvětšovalo čtvercové obrázky na obdelníkové
        float scale = (float)width / (float)image.Width;
        using (Bitmap thumb = new Bitmap(width, height))
        {
            using (Graphics graphics = Graphics.FromImage(thumb))
            {
                using (System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix())
                {
                    transform.Scale(scale, scale, MatrixOrder.Append);
                    graphics.SetClip(new System.Drawing.Rectangle(0, 0, width, height));
                    graphics.Transform = transform;
                    graphics.DrawImage(image, 0, 0, image.Width, image.Height);


                    ImageCodecInfo Info = getEncoderInfo("image/jpeg");
                    EncoderParameters Params = new EncoderParameters(1);
                    Params.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 66L);
                    SaveImage(path, thumb, Info, Params);
                    #endregion
                }
            }
        }
    }

    /// <summary>        
    /// Zdrojová metoda musí zavolat A2.Dispose nebo vložit vytváření A2 do Using klazule
    /// </summary>
    /// <param name="path"></param>
    /// <param name="thumb"></param>
    /// <param name="Info"></param>
    /// <param name="Params"></param>
    private static void SaveImage(string path, Image thumb, ImageCodecInfo Info, EncoderParameters Params)
    {
        using (System.IO.MemoryStream mss = new System.IO.MemoryStream())
        {
            thumb.Save(mss, Info, Params);
            FS.SaveMemoryStream(mss, path);
            //thumb.Dispose();
        }
    }

    private static ImageCodecInfo getEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec
        for (int i = 0; i < codecs.Length; i++)
            if (codecs[i].MimeType == mimeType)
                return codecs[i];
        return null;
    }



    /// <summary>
    /// A5 je instance originálního(velkého/původního) obrázku
    /// </summary>
    /// <param name="toFolderTempSlash"></param>
    /// <param name="fnwoe"></param>
    /// <param name="ext"></param>
    /// <param name="finalMin"></param>
    /// <param name="bmp"></param>
    /// <param name="borderColor"></param>
    public static void CreateThumbnailOptimal(string toFolderTempSlash, string fnwoe, string ext, string finalMin, System.Drawing.Image bmp, int tnWidth, int tnHeight)
    {
        System.Drawing.Size sizeMin = bmp.Size;
        Point pointMin = new Point();
        if (bmp.Width > tnWidth || bmp.Height > tnHeight)
        {
            if (bmp.Height >= bmp.Width)
            {
                //// Menší je šířka, vypočtu optimální velikost obrázku, kde specifikuji výšku 168
                //sizeMin = PicturesSunamo.CalculateOptimalSizeHeight(bmp.Width, bmp.Height, tnHeight).ToSystemDrawing();
                //bool vypocistIVysku = false;
                //while (sizeMin.Width > tnWidth)
                //{
                //    vypocistIVysku = true;
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //while (sizeMin.Height > tnHeight)
                //{
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //pointMin = new Point(Divide(tnWidth - sizeMin.Width), 0);
                //if (vypocistIVysku)
                //{
                //    pointMin = new Point(pointMin.X, Divide(tnHeight - sizeMin.Height));
                //}
            }
            else if (bmp.Width > bmp.Height)
            {
                //sizeMin = PicturesSunamo.CalculateOptimalSize(bmp.Width, bmp.Height, tnWidth).ToSystemDrawing();
                //bool vypocistISirku = false;
                //while (sizeMin.Height > tnHeight)
                //{
                //    vypocistISirku = true;
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //while (sizeMin.Width > tnWidth)
                //{
                //    sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
                //}
                //pointMin = new Point(0, Divide(tnHeight - sizeMin.Height));
                //if (vypocistISirku)
                //{
                //    pointMin = new Point(Divide(tnWidth - sizeMin.Width), pointMin.Y);
                //}
            }
        }
        else
        {
            // Buď výška nebo šířka byla menší než minimální, zjistím co a obrázek umístím na střed
            while (sizeMin.Height > tnHeight && sizeMin.Width > tnWidth)
            {
                sizeMin = new Size(Multiple(sizeMin.Width), Multiple(sizeMin.Height));
            }

            pointMin = new Point(Divide(tnWidth - sizeMin.Width), Divide(tnHeight - sizeMin.Height));
        }

        //bool zmensovatMiniaturu = true;
        if (true)
        {
            string tempMin = toFolderTempSlash + fnwoe + "_tn" + ext;
            FS.CreateUpfoldersPsysicallyUnlessThere(tempMin);
            TransformImage(bmp, sizeMin.Width, sizeMin.Height, finalMin);
        }
    }

    public static void TransformImage(System.Drawing.Image image, double width, double height, string path)
    {
        TransformImage(image, (int)width, (int)height, path);
    }

    public static int Multiple(int p)
    {
        float f = (float)p;
        float f2 = (f * 0.9f);
        return (int)(f2);
    }



    public static int Divide(int p)
    {
        float f = (float)p;
        float f2 = (f / 2f);
        return (int)(f2);
    }
    #endregion

    #region Mohlo by být v shared ale museli by se dokopírovat metody getEncoderInfo a SaveImage. čiště v shared to být nemůže - fw by volalo nekompatibilní metody, shared je std, fw net48.
    /// <summary>
    /// Zdrojová metoda musí zavolat A2.Dispose nebo vložit vytváření A2 do Using klazule
    /// </summary>
    /// <param name="path"></param>
    /// <param name="thumb"></param>
    /// <param name="mime"></param>
    /// <param name="Params"></param>
    public static void SaveImage(string path, Image thumb, string mime, EncoderParameters Params)
    {
        SaveImage(path, thumb, getEncoderInfo(mime), Params);

    }

    public static void saveJpeg(string path, Image img, long quality)
    {
        path = FS.ChangeExtension(path, AllExtensions.jpg, false);
        try
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam =
               new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);

        }
        catch
        {

        }
    }
    #endregion
}
