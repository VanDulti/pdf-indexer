using iTextSharp.text.pdf;

namespace PdfIndexer.Data;

public class TextSharpPdfReader : IPdfReader
{
    public IEnumerable<(int, string)> Read(Stream stream)
    {
        using var reader = new PdfReader(stream);
        for (var pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
        {
            var contentBytes = reader.GetPageContent(pageNumber);
            var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(contentBytes));
            var tokens = EnumerateTokens(tokenizer);
            var text = string.Join("", tokens);
            yield return (pageNumber, text);
        }
    }

    private static IEnumerable<string> EnumerateTokens(PrTokeniser tokeniser)
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