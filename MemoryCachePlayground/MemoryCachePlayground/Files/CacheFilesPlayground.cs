using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System.Diagnostics;
using System.IO;

namespace MemoryCachePlayground.Files
{
    [TestClass]
    public class CacheFilesPlayground
    {
        private const string FILE = "story.txt";

        private static ILogger log;
        private FileManager fileManager;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            log = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Files", FILE);
            log.Information("File to open: {File}", file);
            fileManager = new FileManager(file, log);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            fileManager = null;
        }

        [TestMethod]
        public void FileReadFirstNotInCacheThenInCache()
        {
            //Original Read not in cache
            ReadFileRun();

            //Second attempted read, return is from cache
            ReadFileRun();

            void ReadFileRun()
            {
                var stopWatch = new Stopwatch();

                log.Information("Reading file timer started...");
                stopWatch.Start();

                var file = fileManager.RetrieveFile();

                file.FileName.Should().NotBeNullOrWhiteSpace();
                file.FileName.Should().Be(FILE);

                file.Content.Should().NotBeNullOrEmpty();

                log.Information("Reading file timer stopped.");
                stopWatch.Stop();

                log.Information("Read time {Time}", stopWatch.Elapsed);
            }
        }
    }
}
