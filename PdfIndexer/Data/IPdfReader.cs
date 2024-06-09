namespace PdfIndexer.Data;

/// <summary>
/// This interface is used to read a PDF file. Reading a pdf file can be painful, as the actual content might be stored in various ways.
/// This interface abstracts away the details of reading a PDF file, and provides a simple way to get the content of a PDF file.
/// </summary>
public interface IPdfReader
{
    /// <summary>
    /// Reads the content of a PDF file from the given stream. The content is returned as a sequence of page numbers and text,
    /// that is, each element in the sequence is a tuple of an integer (the page number) and a string (the text on that page).
    /// This can be used to (depending on the implementation lazily) go through the pages of a pdf file using foreach/linq 
    /// </summary>
    /// <param name="stream">The stream to read the PDF file from.</param>
    /// <returns>A sequence of page numbers and text, where each element is a tuple of an integer and a string.</returns>
    IEnumerable<(int, string)> Read(Stream stream);
}