using System.Drawing;

namespace Overlay.Data.Interfaces;

public interface IMMRClient
{
    public Dictionary<int, int> GetFromBitmap(Bitmap bmp, int players = 2);
}
