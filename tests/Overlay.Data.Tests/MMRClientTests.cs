using System.Drawing;
using Moq;
using Overlay.Data.Interfaces;

namespace Overlay.Data.Tests;

public class MMRClientTests
{
    [Fact]
    public void Double_MMR_2K()
    {
        var mockOcr = new Mock<IOCRClient>();

        mockOcr.SetupSequence(x => x.GetText(It.IsAny<Bitmap>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), true))
               .Returns("Team1 4478\n")
               .Returns("4524 Team2\n");

        var client = new MMRClient(mockOcr.Object);
        var bitmap = new Bitmap("./data/2560x1440_double_mmr.png");
        var mmrs = client.GetFromBitmap(bitmap, 2);

        Assert.Equal(new Dictionary<int, int> { { 1, 4478 }, { 2, 4524 } }, mmrs);
    }

    [Fact]
    public void Single_MMR_1K()
    {
        var mockOcr = new Mock<IOCRClient>();

        mockOcr.SetupSequence(x => x.GetText(It.IsAny<Bitmap>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), true))
               .Returns("Team7\n")
               .Returns("4488 Teame2\n");

        var client = new MMRClient(mockOcr.Object);
        var bitmap = new Bitmap("./data/1920x1080_single_mmr.png");
        var mmrs = client.GetFromBitmap(bitmap, 2);

        Assert.Equal(new Dictionary<int, int> { { 1, 0 }, { 2, 4488 } }, mmrs);
    }
}