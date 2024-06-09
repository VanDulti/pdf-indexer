using Microsoft.AspNetCore.Components.Forms;

namespace PdfIndexer.Data;

/// <summary>
/// An InMemoryFile is a representation of a browser file that is stored in memory and can therefore be read multiple times.
/// This comes in handy when you want to remember the content of a file that was uploaded by the user (for re-opening, for example).
/// Of course, this will consume lots of memory if the files are large.
/// </summary>
/// <param name="Name"><see cref="IBrowserFile.Name"/></param>
/// <param name="LastModified"><see cref="IBrowserFile.LastModified"/></param>
/// <param name="Size"><see cref="IBrowserFile.Size"/></param>
/// <param name="ContentType"><see cref="IBrowserFile.ContentType"/></param>
/// <param name="Content">The content of the file as a byte array.</param>
public record InMemoryFile(string Name, DateTimeOffset LastModified, long Size, string ContentType, byte[] Content)
    : IBrowserFile
{
    /// <summary>
    /// Creates a new InMemoryFile from the given browser file. This will read the entire file into memory, up to the given maximum size.
    /// 
    /// </summary>
    /// <param name="file">The file to load into memory.</param>
    /// <param name="maxAllowedSize">The maximum size of the file to read. This will fall back to the original IBrowserFile.Size if omitted.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A new InMemoryFile with the content of the given file.</returns>
    /// <exception cref="InvalidOperationException">If the file could not be read in its entirety.</exception>
    public static async Task<InMemoryFile> CopyOf(
        IBrowserFile file,
        long? maxAllowedSize = null,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = file.OpenReadStream(
            maxAllowedSize: maxAllowedSize ?? file.Size,
            cancellationToken: cancellationToken
        );
        var content = new byte[stream.Length];
        var result = await stream.ReadAsync(content, cancellationToken);
        if (result != content.Length)
        {
            throw new InvalidOperationException("Failed to read the entire file");
        }

        return new InMemoryFile(file.Name, file.LastModified, file.Size, file.ContentType, content);
    }

    /// <summary>
    /// Opens the backing array as a new memory stream.
    /// </summary>
    /// <param name="maxAllowedSize">Does not have any effect on this implementation.</param>
    /// <param name="cancellationToken">Does not have any effect on this implementation.</param>
    /// <returns>A new memory stream with the content of the backing array.</returns>
    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = new())
    {
        return new MemoryStream(Content);
    }
}