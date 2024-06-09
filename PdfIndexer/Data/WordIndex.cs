using System.Collections.Immutable;
using System.Text.Json;

namespace PdfIndexer.Data;

public class WordIndex
{
    private readonly SortedDictionary<string, ISet<int>> _index = new();

    public IImmutableDictionary<string, ISet<int>> Index => _index.ToImmutableSortedDictionary();

    public void Add(int pageNumber, IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            _index.TryAdd(word, new SortedSet<int>());
            _index[word].Add(pageNumber);
        }
    }

    public string? ToJson()
    {
        return JsonSerializer.Serialize(_index);
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