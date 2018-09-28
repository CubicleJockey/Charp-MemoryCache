using MemoryCachePlayground.Constants;
using System;
using System.IO;
using System.Runtime.Caching;

namespace MemoryCachePlayground.Files
{
    public class FileManager
    {
        private readonly FileInfo File;
        private readonly MemoryCache cache;


        public FileManager(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) { throw new ArgumentNullException(nameof(file)); }

            File = new FileInfo(file);
            File.Refresh();

            if (!File.Exists) { throw new FileNotFoundException("Check that file exists.", file); }

            cache = MemoryCache.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public (string File, string Content) RetrieveFile()
        {
            if (cache.Contains(CacheKeys.Story))
            {
                return (File.Name, (string)cache.GetCacheItem(CacheKeys.Story)?.Value);
            }

            string content;
            using (var streamReader = File.OpenText())
            {
                content = streamReader.ReadToEnd();
            }
            return (File.Name, content);
        }
    }
}
