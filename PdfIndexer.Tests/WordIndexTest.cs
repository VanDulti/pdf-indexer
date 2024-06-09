using PdfIndexer.Data;

namespace PdfIndexer.Tests;

public class WordIndexTest
{
    [Fact]
    public void Add_AddsWordToIndex()
    {
        // Arrange
        var index = new WordIndex();

        // Act
        index.Add(1, ["hello"]);

        // Assert
        Assert.Equal([1], index.Index["hello"]);
    }
    
    [Fact]
    public void Add_AddsMultipleWordsToIndex()
    {
        // Arrange
        var index = new WordIndex();

        // Act
        index.Add(1, ["hello", "world"]);

        // Assert
        Assert.Equal([1], index.Index["hello"]);
        Assert.Equal([1], index.Index["world"]);
    }
    
    [Fact]
    public void Add_AddsWordToExistingIndex()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello"]);

        // Act
        index.Add(2, ["world"]);

        // Assert
        Assert.Equal([1], index.Index["hello"]);
        Assert.Equal([2], index.Index["world"]);
    }
    
    [Fact]
    public void Add_AddsMultipleWordsToExistingIndex()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello"]);

        // Act
        index.Add(2, ["world", "hello"]);

        // Assert
        Assert.Equal([1, 2], index.Index["hello"]);
        Assert.Equal([2], index.Index["world"]);
    }
    
    [Fact]
    public void Add_AddsWordToExistingIndexWithOffset()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello"]);

        // Act
        var otherIndex = new WordIndex();
        otherIndex.Add(1, ["world"]);
        index.Add(otherIndex, 1);

        // Assert
        Assert.Equal([1], index.Index["hello"]);
        Assert.Equal([2], index.Index["world"]);
    }
    
    [Fact]
    public void Add_AddsMultipleWordsToExistingIndexWithOffset()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello"]);

        // Act
        var otherIndex = new WordIndex();
        otherIndex.Add(1, ["world", "hello"]);
        index.Add(otherIndex, 1);

        // Assert
        Assert.Equal([1, 2], index.Index["hello"]);
        Assert.Equal([2], index.Index["world"]);
    }
    
    [Fact]
    public void SerializeJson_SerializesIndex()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello"]);

        // Act
        var stream = new MemoryStream();
        index.SerializeJson(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        // Assert
        Assert.Equal("{\"hello\":[1]}", json);
    }
    
    [Fact]
    public void SerializeJson_SerializesMultipleWordsOnMultiplePages()
    {
        // Arrange
        var index = new WordIndex();
        index.Add(1, ["hello", "world"]);
        index.Add(2, ["world", "hello"]);

        // Act
        var stream = new MemoryStream();
        index.SerializeJson(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        // Assert
        Assert.Equal("{\"hello\":[1,2],\"world\":[1,2]}", json);
    }
}