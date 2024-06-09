using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PdfIndexer.Data;

public class WordIndex
{
    private readonly SortedDictionary<string, ISet<int>> _index = new();
    
    [JsonInclude]
    public IImmutableDictionary<string, ISet<int>> Index => _index.ToImmutableSortedDictionary();

    public void Add(int pageNumber, IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            _index.TryAdd(word, new SortedSet<int>());
            _index[word].Add(pageNumber);
        }
    }

    public Task SerializeJson(Stream stream)
    {
        return JsonSerializer.SerializeAsync(stream, Index);
    }

    public void Add(WordIndex wordIndex, int offset)
    {
        foreach (var (word, pageNumbers) in wordIndex.Index)
        {
            _index.TryAdd(word, new SortedSet<int>());
            foreach (var pageNumber in pageNumbers)
            {
                _index[word].Add(pageNumber + offset);
            }
        }
    }
};