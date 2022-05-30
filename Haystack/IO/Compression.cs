using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Haystack.IO;

/// <summary>
/// Extension methods for the System.IO.Compression namespace
/// </summary>
public static class Compression
{
    /// <summary>
    /// Compresses a byte array using Gzip compression.
    /// </summary>
    public static byte[] CompressGzip(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var compressor = new GZipStream(destination, CompressionMode.Compress))
            {
                source.CopyTo(compressor);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Decompresses a byte array that has been compressed using Gzip compression.
    /// </summary>
    public static byte[] DecompressGzip(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var decompressor = new GZipStream(source, CompressionMode.Decompress))
            {
                decompressor.CopyTo(destination);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Compresses a byte array using Gzip compression. Does not block threads while waiting for IO to complete.
    /// </summary>
    public static async Task<byte[]> CompressGzipAsync(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var compressor = new GZipStream(destination, CompressionMode.Compress))
            {
                await source.CopyToAsync(compressor);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Decompresses a byte array that has been compressed using Gzip compression. Does not block threads while waiting for IO to complete.
    /// </summary>
    public static async Task<byte[]> DecompressGzipAsync(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var decompressor = new GZipStream(source, CompressionMode.Decompress))
            {
                await decompressor.CopyToAsync(destination);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Compresses a byte array using DEFLATE compression.
    /// </summary>
    public static byte[] CompressDeflate(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var compressor = new DeflateStream(destination, CompressionMode.Compress))
            {
                source.CopyTo(compressor);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Compresses a byte array using DEFLATE compression. Does not block threads while waiting for IO to complete.
    /// </summary>
    public static async Task<byte[]> CompressDeflateAsync(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var compressor = new DeflateStream(destination, CompressionMode.Compress))
            {
                await source.CopyToAsync(compressor);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Decompresses a byte array that has been compressed using DEFLATE compression.
    /// </summary>
    public static byte[] DecompressDeflate(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var decompressor = new DeflateStream(source, CompressionMode.Decompress))
            {
                decompressor.CopyTo(destination);
            }
            return destination.ToArray();
        }
    }

    /// <summary>
    /// Decompresses a byte array that has been compressed using DEFLATE compression. Does not block threads while waiting for IO to complete.
    /// </summary>
    public static async Task<byte[]> DecompressDeflateAsync(this byte[] b)
    {
        using (var source = new MemoryStream(b))
        using (var destination = new MemoryStream())
        {
            using (var decompressor = new DeflateStream(source, CompressionMode.Decompress))
            {
                await decompressor.CopyToAsync(destination);
            }
            return destination.ToArray();
        }
    }
}