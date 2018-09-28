using MemoryCachePlayground.Constants;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;

namespace MemoryCachePlayground.Files
{
    public class FileManager
    {
        private readonly string file;
        private readonly MemoryCache cache;
        private readonly ILogger log;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="file">File name or Full filename to be read from.</param>
        /// <param name="log">Logger</param>
        /// <param name="cache">Cache, IF null will default to Memory.Default, Optional</param>
        public FileManager(string file, ILogger log, MemoryCache cache = null)
        {
            if (string.IsNullOrWhiteSpace(file)) { throw new ArgumentNullException(nameof(file)); }

            this.file = file;
            this.cache = cache ?? MemoryCache.Default;
            this.log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public (string FileName, IEnumerable<string> Content) RetrieveFile()
        {
            var fileName = Path.GetFileName(file);
            log.Information("Checking if file: [{Name}] is cached.", fileName);
            if (cache.Contains(CacheKeys.Story))
            {
                log.Information("File: [{Name}] was found in the cache. Returning from cache.", fileName);
                return (fileName, cache.GetCacheItem(CacheKeys.Story)?.Value as IEnumerable<string>);
            }

            log.Information("File: [{Name}] was not found in cache. Reading file content.", fileName);

            if (!File.Exists(file)) { throw new FileNotFoundException("Check that file exists.", file); }

            var content = File.ReadAllLines(file);

            cache.Add(CacheKeys.Story, content, new CacheItemPolicy());

            return (fileName, content);
        }

        public void CreateFile(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) { throw new ArgumentException("Cannot be empty.", nameof(content)); }

            log.Information("Creating file: [{File}]...", file);
            log.Information("Saving content: [{Content}]...", content);

            File.WriteAllText(file, content);

            log.Information("File created with content!");
        }

        public void CreateFile(IEnumerable<string> content)
        {
            if (content == null || !content.Any()) { throw new ArgumentException("Cannot be empty.", nameof(content)); }

            log.Information("Creating file: [{File}]...", file);
            log.Information("Saving content: [{Content}]...", content);

            File.WriteAllLines(file, content);

            log.Information("File created with content!");
        }
    }
}
