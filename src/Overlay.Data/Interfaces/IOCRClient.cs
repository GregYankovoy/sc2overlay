using System.Drawing;

namespace Overlay.Data.Interfaces;

public interface IOCRClient
{
    public string GetText(Bitmap bmp, int? sourceX = null, int? sourceY = null, int? width = null, int? height = null, bool invertColors = false);
}
