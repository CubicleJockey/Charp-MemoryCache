using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;

namespace MemoryCachePlayground
{
    [TestClass]
    public class CacheFiles
    {
        private readonly ObjectCache cache;

        public CacheFiles()
        {
            cache = MemoryCache.Default;
        }

        [TestMethod]
        public void FileReadNotInCache()
        {

        }

    }
}
