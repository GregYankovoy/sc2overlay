using System.Drawing;

namespace Overlay.Data.Tests;

public class OCRClientTests
{
    [Fact]
    public void Double_MMR_2K()
    {
        var bmp = new Bitmap("./data/2560x1440_double_mmr.png");
        var team1sourceX = Convert.ToInt16(bmp.Width * 0.1311987504880906);
        var team2sourceX = Convert.ToInt16(bmp.Width * 0.7294025771183132);
        var sourceY = Convert.ToInt16(bmp.Height * 0.4319444444444444);

        var ocrWidth = Convert.ToInt16(bmp.Width * 0.1421319796954315);
        var ocrHeight = Convert.ToInt16(bmp.Height * 0.0145833333333333);

        var client = new OCRClient();
        var text1 = client.GetText(bmp, team1sourceX, sourceY, ocrWidth, ocrHeight, true);
        var text2 = client.GetText(bmp, team2sourceX, sourceY, ocrWidth, ocrHeight, true);

        Assert.Equal("Team1 4478\n", text1);
        Assert.Equal("4524 Team2\n", text2);
    }

    [Fact]
    public void Single_MMR_1K()
    {
        var bmp = new Bitmap("./data/1920x1080_single_mmr.png");
        var team1sourceX = Convert.ToInt16(bmp.Width * 0.1311987504880906);
        var team2sourceX = Convert.ToInt16(bmp.Width * 0.7294025771183132);
        var sourceY = Convert.ToInt16(bmp.Height * 0.4319444444444444);

        var ocrWidth = Convert.ToInt16(bmp.Width * 0.1421319796954315);
        var ocrHeight = Convert.ToInt16(bmp.Height * 0.0145833333333333);

        var client = new OCRClient();
        var text1 = client.GetText(bmp, team1sourceX, sourceY, ocrWidth, ocrHeight, true);
        var text2 = client.GetText(bmp, team2sourceX, sourceY, ocrWidth, ocrHeight, true);

        Assert.Equal("Team7\n", text1);
        Assert.Equal("4488 Teame2\n", text2);
    }
}