using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfIndexer.Data;

/// <summary>
/// A PDF writer that uses iTextSharp to write PDF files. ITextSharp is very popular but also quite complex to use.
/// Furthermore, the version used here is quite old, as the project has switched to a different license model.
///
/// From a technical perspective, iTextSharp does not offer an asynchroneous API, which means that the methods here
/// are actually executed synchronously. This is why it might be a good idea to use other libraries in the future.
/// </summary>
public class TextSharpPdfWriter : IPdfWriter
{
    public Task WriteIndex(Stream stream, WordIndex wordIndex)
    {
        using var doc = new Document(PageSize.A4, 25, 25, 30, 30);
        using var writer = PdfWriter.GetInstance(doc, stream);
        AddMetadata(doc);
        doc.Open();

        AppendIndex(wordIndex, doc);

        doc.Close();
        return Task.CompletedTask;
    }

    private Font IndexFont { get; } = FontFactory.GetFont(FontFactory.HELVETICA, 7, BaseColor.Black);

    public Task WriteOriginalWithIndex(Stream stream, Stream originalPdf, WordIndex wordIndex)
    {
        using var doc = new Document(PageSize.A4, 25, 25, 30, 30);
        using var writer = PdfWriter.GetInstance(doc, stream);
        AddMetadata(doc);

        doc.Open();
        using var reader = new PdfReader(originalPdf);
        AppendPdf(reader, writer, doc);

        AppendIndex(wordIndex, doc);

        doc.Close();
        return Task.CompletedTask;
    }

    public Task WriteCombined(Stream stream, IEnumerable<(Stream original, WordIndex wordIndex)> wordIndexes)
    {
        using var doc = new Document(PageSize.A4, 25, 25, 30, 30);
        using var writer = PdfWriter.GetInstance(doc, stream);
        AddMetadata(doc);
        doc.Open();

        foreach (var (original, wordIndex) in wordIndexes)
        {
            var reader = new PdfReader(original);
            AppendPdf(reader, writer, doc);
            AppendIndex(wordIndex, doc);
        }

        doc.Close();
        return Task.CompletedTask;
    }

    public Task WriteMerged(Stream stream, IEnumerable<(Stream original, WordIndex wordIndex)> wordIndexes)
    {
        using var doc = new Document(PageSize.A4, 25, 25, 30, 30);
        using var writer = PdfWriter.GetInstance(doc, stream);
        AddMetadata(doc);

        var totalIndex = new WordIndex();

        doc.Open();
        var offset = 0;
        foreach (var (file, wordIndex) in wordIndexes)
        {
            var reader = new PdfReader(file);
            AppendPdf(reader, writer, doc);
            offset += reader.NumberOfPages;
            totalIndex.Add(wordIndex, offset);
        }

        AppendIndex(totalIndex, doc);

        doc.Close();
        return Task.CompletedTask;
    }

    private void AppendIndex(WordIndex wordIndex, Document doc)
    {
        ArgumentNullException.ThrowIfNull(wordIndex);
        ArgumentNullException.ThrowIfNull(doc);
        doc.NewPage();
        var columns = new MultiColumnText();
        var numberOfColumns = doc.PageSize.Rotation is 90 or 270 ? 8 : 6;
        columns.AddRegularColumns(doc.Left, doc.Right, 2, numberOfColumns);
        columns.Alignment = Element.ALIGN_LEFT;
        doc.Add(new Paragraph("Index", new Font(Font.HELVETICA, 14, Font.BOLD)));
        foreach (var (word, locations) in wordIndex.Index)
        {
            var line = new Paragraph($"{word}: {string.Join(",", locations)}", IndexFont)
            {
                MultipliedLeading = 1.05f
            };
            columns.AddElement(line);
        }

        doc.Add(columns);
    }

    private static void AppendPdf(PdfReader reader, PdfWriter writer, Document doc)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(doc);
        var cb = writer.DirectContent;
        for (var i = 0; i < reader.NumberOfPages; i++)
        {
            var pageNumber = i + 1; // 1-based, but the number of pages is 0-based...
            var pageSize = reader.GetPageSizeWithRotation(pageNumber);
            doc.SetPageSize(pageSize);
            if (!doc.NewPage())
            {
                continue;
            }

            var importedPage = writer.GetImportedPage(reader, pageNumber);
            var rotation = reader.GetPageRotation(pageNumber);
            if (rotation is 90 or 270)
            {
                cb.AddTemplate(importedPage, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(pageNumber).Height);
            }
            else
            {
                cb.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
            }
        }
    }

    private static void AddMetadata(Document doc)
    {
        doc.AddAuthor("PdfIndexer");
        doc.AddCreator("PdfIndexer");
        doc.AddKeywords("Indexed PDF document");
        doc.AddTitle("Indexed PDF document");
        doc.AddCreationDate();
    }
}