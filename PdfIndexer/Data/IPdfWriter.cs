using UglyToad.PdfPig.Content;

namespace PdfIndexer.Data;

/// <summary>
/// This interface is used to write a word index to pdf. Writing a pdf file can be painful, as the actual content might
/// be stored in various ways. This interface abstracts away the details of writing a PDF file, and provides a simple way
/// to write to a stream.
/// </summary>
public interface IPdfWriter
{
    /// <summary>
    /// Writes the given word index to the stream as pdf document. 
    /// </summary>
    /// <param name="stream">The stream to write the PDF file to.</param>
    /// <param name="wordIndex">The word index to write to the PDF file.</param>
    Task WriteIndex(Stream stream, WordIndex wordIndex);

    /// <summary>
    /// Appends the given word index to the original pdf and writes the resulting pdf file to the stream.
    /// </summary>
    /// <param name="stream">The stream to write the PDF file to.</param>
    /// <param name="originalPdf">The original pdf file to append the word index to.</param>
    /// <param name="wordIndex">The word index to append to the original pdf file.</param>
    Task WriteOriginalWithIndex(Stream stream, Stream originalPdf, WordIndex wordIndex);

    /// <summary>
    /// Writes the given word indices to the stream as a combined pdf document, where each word index is appended to its
    /// respective original pdf.
    /// </summary>
    /// <param name="stream">The stream to write the PDF file to.</param>
    /// <param name="wordIndexes">The word indexes to write to the PDF file.</param>
    Task WriteCombined(Stream stream, IEnumerable<(Stream original, WordIndex wordIndex)> wordIndexes);

    /// <summary>
    /// Writes the given word indexes to the stream as a merged pdf document, where the word indices are combined into a
    /// single index over all the original pdfs.
    /// </summary>
    /// <param name="stream">The stream to write the PDF file to.</param>
    /// <param name="wordIndexes">The word indexes to write to the PDF file.</param>
    Task WriteMerged(Stream stream, IEnumerable<(Stream original, WordIndex wordIndex)> wordIndexes);
}