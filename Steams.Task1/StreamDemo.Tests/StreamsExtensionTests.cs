using System.IO;
using System.Text;
using NUnit.Framework;

namespace StreamDemo.Tests
{
    [TestFixture]
    public class StreamsExtensionTests
    {
        private const string SourceFileName = @"SourceText.txt";

        private const string DestinationFileName = @"DestinationText.txt";
        
        [SetUp]
        public void Init()
        {
            File.Delete(DestinationFileName);
        }

        [Test]
        public void ByByteCopy_Successfully()
        {
            StreamsExtension.ByByteCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByBytes(SourceFileName, DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }

        [Test]
        public void InMemoryByByteCopy_Successfully()
        {
            StreamsExtension.InMemoryByByteCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByBytes(SourceFileName, DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }

        [Test]
        public void ByBlockCopy_Successfully()
        {
            StreamsExtension.ByBlockCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByBytes(SourceFileName, DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }

        [Test]
        public void InMemoryByBlockCopy_Successfully()
        {
            StreamsExtension.InMemoryByBlockCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            //Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            //ssert.IsTrue(AreEqualByBytes(SourceFileName,DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }

        [Test]
        public void BufferedCopy_Successfully()
        {
            StreamsExtension.BufferedCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByBytes(SourceFileName, DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }

        [Test]
        public void ByLineCopy_Successfully()
        {
            StreamsExtension.ByLineCopy(SourceFileName, DestinationFileName);

            Assert.IsTrue(AreEqualByLength(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByContent(SourceFileName, DestinationFileName));
            Assert.IsTrue(AreEqualByBytes(SourceFileName, DestinationFileName));

            CheckFileIsClosed(SourceFileName);
            CheckFileIsClosed(DestinationFileName);
        }
        
        private bool AreEqualByLength(string sourceFile, string destinationFile)
        {
            FileInfo sourceFileInfo = new FileInfo(sourceFile);
            FileInfo destinationFileInfo = new FileInfo(destinationFile);

            return sourceFileInfo.Length.Equals(destinationFileInfo.Length);
        }
        private bool AreEqualByContent(string sourceFile, string destinationFile)
        {
            string source = null;
            using (TextReader sourceReader = new StreamReader(sourceFile, Encoding.UTF8))
            {
                source = sourceReader.ReadToEnd();
            }

            string destination = null;
            using (TextReader destinationReader = new StreamReader(destinationFile, Encoding.UTF8))
            {
                destination = destinationReader.ReadToEnd();
            }

            return source.Equals(destination);
        }
        private bool AreEqualByBytes(string sourcePath, string destinationPath)
        {
            using FileStream firstStream = new FileStream(sourcePath, FileMode.Open);
            using FileStream secondStream = new FileStream(destinationPath, FileMode.Open);
            
            int nextByte;

            while ((nextByte = firstStream.ReadByte()) > -1)
            {
                if (nextByte != secondStream.ReadByte())
                {
                    return false;
                }
            }

            return secondStream.ReadByte() == -1;
        }
        private void CheckFileIsClosed(string fileName)
        {
            try
            {
                using var stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                Assert.Fail("Source stream is not closed! Please use 'using' statement.");
            }
        }
    }
}