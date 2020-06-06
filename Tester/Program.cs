using System;
using System.Text;
using Haystack.IO;

namespace Tester
{
    class Program
    {
        private static readonly string _someText = DateTime.Now.ToString("F");
        private static readonly byte[] _someBytes = Encoding.UTF8.GetBytes(_someText);
        
        static void Main(string[] args)
        {
            var gz = _someBytes.CompressGzip();
            var decompGz = gz.DecompressGzip();
            var decompGzString = Encoding.UTF8.GetString(decompGz);

            var deflate = _someBytes.CompressDeflate();
            var decompDeflate = deflate.DecompressDeflate();
            var decompDeflateString = Encoding.UTF8.GetString(decompDeflate);
        }
    }
}