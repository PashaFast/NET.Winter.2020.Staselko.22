using System;
using System.Configuration;
using static StreamDemo.StreamsExtension;


namespace ConsoleClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            var source = ConfigurationManager.AppSettings["sourceFilePath"];

            var destination = ConfigurationManager.AppSettings["destinationFiePath"];

            Console.WriteLine($"ByteCopy() done. Total bytes: {ByByteCopy(source, destination)}");

            Console.WriteLine($"InMemoryByteCopy() done. Total bytes: {InMemoryByByteCopy(source, destination)}");
            
            Console.WriteLine($"BlockCopy() done. Total bytes: {ByBlockCopy(source, destination)}");

            //Console.WriteLine($"InMemoryBlockCopy. Total bytes: { InMemoryByBlockCopy(source, destination)}");

            Console.WriteLine($"BufferedCopyCopy. Total bytes: { BufferedCopy(source, destination)}");
            
            Console.WriteLine($"LineCopy. Total lines: { ByLineCopy(source, destination)}");
        }
    }
}
