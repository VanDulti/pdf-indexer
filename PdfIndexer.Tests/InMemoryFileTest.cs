using System.IO.Compression;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
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
    public void OpenReadStream_ReturnsSameStreamMultipleTimes()
    {
        // Arrange
        var content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var file = new InMemoryFile("test.txt", DateTimeOffset.Now, 5, "text/plain", content);

        // Act
        var stream1 = file.OpenReadStream();
        var buffer1 = new byte[5];
        stream1.ReadExactly(buffer1, 0, 5);
        var stream2 = file.OpenReadStream();
        var buffer2 = new byte[5];
        stream2.ReadExactly(buffer2, 0, 5);

        // Assert
        Assert.Equal(content, buffer1);
        Assert.Equal(content, buffer2);
    }

    [Fact]
    public void OpenReadStream_SupportsSeek()
    {
        // Arrange
        var content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var file = new InMemoryFile("test.txt", DateTimeOffset.Now, 5, "text/plain", content);

        // Act
        var stream = file.OpenReadStream();
        stream.Seek(2, SeekOrigin.Begin);
        var buffer = new byte[3];
        stream.ReadExactly(buffer, 0, 3);

        // Assert
        Assert.Equal(new byte[] { 0x03, 0x04, 0x05 }, buffer);
    }

    [Fact]
    public void CopyOf_ReturnsInMemoryFileWithSameContent()
    {
        // Arrange
        var content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var file = new InMemoryFile("test.txt", DateTimeOffset.Now, 5, "text/plain", content);

        // Act
        var copy = InMemoryFile.CopyOf(file).Result;

        // Assert
        Assert.Equal(file.Name, copy.Name);
        Assert.Equal(file.LastModified, copy.LastModified);
        Assert.Equal(file.Size, copy.Size);
        Assert.Equal(file.ContentType, copy.ContentType);
        Assert.Equal(file.Content, copy.Content);
    }
}