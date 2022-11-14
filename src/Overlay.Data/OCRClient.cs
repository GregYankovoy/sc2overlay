using System.Drawing;
using System.Reflection;
using Overlay.Data.Interfaces;
using Tesseract;

namespace Overlay.Data;

public class OCRClient : IOCRClient, IDisposable
{
    private readonly TesseractEngine _tessaract;

    public OCRClient()
    {
        var tessFlags = new Dictionary<string, object> {
            { "tessedit_char_whitelist", "Team0123456789" },
            { "tessedit_pageseg_mode", 7 }
        };

        _tessaract = new TesseractEngine($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/tessdata", "eng", EngineMode.Default, Enumerable.Empty<string>(), tessFlags, false);
    }

    public void Dispose()
    {
        _tessaract.Dispose();
    }

    private static void Invert(Bitmap bmp)
    {
        for (int y = 0; (y <= (bmp.Height - 1)); y++)
        {
            for (int x = 0; (x <= (bmp.Width - 1)); x++)
            {
                Color inv = bmp.GetPixel(x, y);
                inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                bmp.SetPixel(x, y, inv);
            }
        }
    }

    public string GetText(Bitmap bmp, int? sourceX = null, int? sourceY = null, int? width = null, int? height = null, bool invertColors = false)
    {
        Bitmap bitmap = bmp.Clone(new Rectangle(sourceX ?? 0, sourceY ?? 0, width ?? bmp.Width, height ?? bmp.Height), bmp.PixelFormat);

        if (invertColors)
        {
            Invert(bitmap);
        }

        lock (_tessaract)
        {
            using (var page = _tessaract.Process(bitmap))
            {
                return page.GetText();
            }
        }
    }
}
