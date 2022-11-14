using System.Drawing;
using System.Text.RegularExpressions;
using Overlay.Data.Interfaces;

namespace Overlay.Data;
public class MMRClient : IMMRClient
{
    private readonly IOCRClient _ocrClient;

    public MMRClient(IOCRClient ocrClient)
    {
        _ocrClient = ocrClient;
        Console.WriteLine("MMR CLIENT INITIALIZED");
    }

    public Dictionary<int, int> GetFromBitmap(Bitmap bmp, int players = 2)
    {
        var team1sourceX = Convert.ToInt16(bmp.Width * 0.1311987504880906);
        var team2sourceX = Convert.ToInt16(bmp.Width * 0.7294025771183132);
        var sourceY = Convert.ToInt16(bmp.Height * 0.4319444444444444);

        var ocrWidth = Convert.ToInt16(bmp.Width * 0.1421319796954315);
        var ocrHeight = Convert.ToInt16(bmp.Height * 0.0145833333333333);

        var team1 = _ocrClient.GetText(bmp, team1sourceX, sourceY, ocrWidth, ocrHeight, true);
        var team2 = _ocrClient.GetText(bmp, team2sourceX, sourceY, ocrWidth, ocrHeight, true);

        Int16.TryParse(Regex.Match(team1, @"\d{2,}").Value, out var team1mmr);
        Int16.TryParse(Regex.Match(team2, @"\d{2,}").Value, out var team2mmr);

        return new Dictionary<int, int>
        {
            { 1, team1mmr },
            { 2, team2mmr }
        };
    }
}