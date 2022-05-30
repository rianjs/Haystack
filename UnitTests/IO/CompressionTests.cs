using System;
using System.Text;
using System.Threading.Tasks;
using Haystack.IO;
using NUnit.Framework;

namespace UnitTests.IO;

public class CompressionTests
{
    private static readonly string _someText = DateTime.Now.ToString("F");
    private static readonly byte[] _someBytes = Encoding.UTF8.GetBytes(_someText);
        
    [Test]
    public void TestCompressionSymmetry()
    {
        var gz = _someBytes.CompressGzip();
        var decompGz = gz.DecompressGzip();
        var decompGzString = Encoding.UTF8.GetString(decompGz);

        var deflate = _someBytes.CompressDeflate();
        var decompDeflate = deflate.DecompressDeflate();
        var decompDeflateString = Encoding.UTF8.GetString(decompDeflate);
            
        Assert.AreEqual(decompDeflateString, decompGzString);
        Assert.AreEqual(decompGzString, _someText);
    }

    [Test]
    public async Task TestCompressionSymmetryAsync()
    {
        var gz = await _someBytes.CompressGzipAsync();
        var decompGz = await gz.DecompressGzipAsync();
        var decompGzString = Encoding.UTF8.GetString(decompGz);

        var deflate = await _someBytes.CompressDeflateAsync();
        var decompDeflate = await deflate.DecompressDeflateAsync();
        var decompDeflateString = Encoding.UTF8.GetString(decompDeflate);

        Assert.AreEqual(decompDeflateString, decompGzString);
        Assert.AreEqual(decompGzString, _someText);
    }
}