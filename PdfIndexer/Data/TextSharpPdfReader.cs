using iTextSharp.text.pdf;

namespace PdfIndexer.Data;

public class TextSharpPdfReader : IPdfReader
{
    /// <summary>
    /// Lazily reads the content of a PDF file from the given stream. The content is returned as a sequence of page numbers
    /// and text, that is, each element in the sequence is a tuple of an integer (the page number) and a string (the text on that page).
    /// </summary>
    /// <param name="stream">The stream containing the document</param>
    /// <exception cref="IOException">if the pdf document couldn't be read</exception>
    /// <returns>A lazily evaluated sequence of pages</returns>
    public IEnumerable<(int, string)> Read(Stream stream)
    {
        using var reader = new PdfReader(stream);
        for (var pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
        {
            var contentBytes = reader.GetPageContent(pageNumber);
            var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(contentBytes));
            var tokens = tokenizer.EnumerateTokens();
            var text = string.Join("", tokens);
            yield return (pageNumber, text);
        }
    }
}

public static class TextSharpPdfReaderExtensions
{
    internal static IEnumerable<string> EnumerateTokens(this PrTokeniser tokeniser)
    {
        while (tokeniser.NextToken())
        {
            if (tokeniser.TokenType == PrTokeniser.TK_STRING)
            {
                yield return tokeniser.StringValue;
            }
        }
    }
}