using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Topdev.OpenSubtitles.Client.Tests
{
    [TestClass]
    public class MovieHasherTests
    {
        [TestMethod]
        public void TestValidComputeMovieHash()
        {
            var content = "[0518/172433.716:ERROR:crash_report_database_win.cc(429)] unexpected header";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var ms = new MemoryStream(contentBytes);
            
            var hash = MovieHasher.ComputeMovieHash(ms);
            var hashHex = MovieHasher.ToHexadecimal(hash);
            
            Assert.AreEqual(hashHex, "c4a8fd66878c7903");
        }
    }
}
