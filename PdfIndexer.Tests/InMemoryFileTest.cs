using PdfIndexer.Data;

namespace PdfIndexer.Tests;

public class InMemoryFileTest
{
    [Fact]
    public void OpenReadStream_ReturnsContentBytes()
    {
        // Arrange
        var content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var file = new InMemoryFile("test.txt", DateTimeOffset.Now, 5, "text/plain", content);

        // Act
        var stream = file.OpenReadStream();

        // Assert
        var buffer = new byte[5];
        stream.ReadExactly(buffer, 0, 5);
        Assert.Equal(content, buffer);
    }
    
    [Fact]
    public void OpenReadStream_MultipleReads_ReturnsSameContentBytes()
    {
        // Arrange
        var content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var file = new InMemoryFile("test.txt", DateTimeOffset.Now, 5, "text/plain", content);

        // Act
        var stream = file.OpenReadStream();
        var buffer1 = new byte[2];
        var buffer2 = new byte[3];
        stream.ReadExactly(buffer1, 0, 2);
        stream.ReadExactly(buffer2, 0, 3);

        // Assert
        Assert.Equal(new byte[] { 0x01, 0x02 }, buffer1);
        Assert.Equal(new byte[] { 0x03, 0x04, 0x05 }, buffer2);
    }
}