using System;
using System.IO;
using System.Text;

namespace StreamDemo
{
    // C# 6.0 in a Nutshell. Joseph Albahari, Ben Albahari. O'Reilly Media. 2015
    // Chapter 15: Streams and I/O
    // Chapter 6: Framework Fundamentals - Text Encodings and Unicode
    // https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=netcore-3.0
    // https://docs.microsoft.com/en-us/dotnet/api/system.io?view=netcore-3.0

    public static class StreamsExtension
    {
        #region Public members

        #region TODO: Implement by byte copy logic using class FileStream as a backing store stream .

        public static int ByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            int bytesCounter = 0;
            using FileStream fileRead = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using FileStream fileWrite = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.Write);
            int nextByte = fileRead.ReadByte();
            while (nextByte != -1)
            {
                fileWrite.WriteByte((byte)nextByte);
                bytesCounter++;
                nextByte = fileRead.ReadByte();
            }

            return bytesCounter;
        }

        #endregion

        #region TODO: Implement by byte copy logic using class MemoryStream as a backing store stream.

        #region Another variant
        //public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        //{
        //    InputValidation(sourcePath, destinationPath);

        //    string source;

        //    // TODO: step 1. Use StreamReader to read entire file in string
        //    using (TextReader sr = new StreamReader(sourcePath))
        //    {
        //        source = sr.ReadToEnd();
        //    }

        //    //  Create byte array on base string content - use  System.Text.Encoding class

        //    var encoding = new UTF8Encoding(true);
        //    byte[] byteArray = encoding.GetBytes(source);

        //    // TODO: step 3. Use MemoryStream instance to read from byte array (from step 2)

        //    var memoryStream = new MemoryStream();

        //    int bytesCounter = 0;

        //    int i = 0;
        //    while (i < byteArray.Length)
        //    {
        //        memoryStream.WriteByte(byteArray[i++]);
        //        bytesCounter++;
        //    }

        //    // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array

        //    byte[] newByteArray = new byte[memoryStream.Length];

        //    bytesCounter = 0;

        //    memoryStream.Seek(0, SeekOrigin.Begin);

        //    while (bytesCounter < memoryStream.Length)
        //    {
        //        newByteArray[bytesCounter++] = Convert.ToByte(memoryStream.ReadByte());
        //    }

        //    // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content

        //    char[] result = encoding.GetChars(newByteArray, 0, bytesCounter);

        //    // TODO: step 6. Use StreamWriter here to write char array content in new file
        //    string newContent = new string(result);
        //    using (StreamWriter sw = new StreamWriter(destinationPath, true, encoding))
        //    {
        //        sw.Write(newContent);
        //    }

        //    return bytesCounter;
        //}

        #endregion
        public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            // TODO: step 1. Use StreamReader to read entire file in string

            using StreamReader streamReader = new StreamReader(sourcePath, false);
            string sourceString = streamReader.ReadToEnd();

            // TODO: step 2. Create byte array on base string content - use  System.Text.Encoding class

            Encoding encoding = Encoding.UTF8;
            byte[] byteArray = encoding.GetBytes(sourceString);

            // TODO: step 3. Use MemoryStream instance to read from byte array (from step 2)
            using MemoryStream memoryStream = new MemoryStream(byteArray);

            // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array
            byte[] newArray = memoryStream.ToArray();

            // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content
            char[] charArray = encoding.GetChars(newArray);

            // TODO: step 6. Use StreamWriter here to write char array content in new file
            int countOfWroteBytes = 0;

            using (var streamWriter = new StreamWriter(destinationPath, false, encoding))
            {
                foreach (var item in charArray)
                {
                    streamWriter.Write(item);
                    countOfWroteBytes++;
                }
                //streamWriter.Write(charArray);
                //countOfWroteBytes++;
            }

            return countOfWroteBytes;
        }

        #endregion

        #region TODO: Implement by block copy logic using FileStream buffer.

        public static int ByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            int bytesCounter = 0;

            using (var sourceStream = new FileStream(sourcePath, FileMode.Open))
            {
                using var destinationStream = new FileStream(destinationPath, FileMode.OpenOrCreate);
                byte[] buffer = new byte[1024];

                int chunkSize = 1;
                while (chunkSize > 0)
                {
                    chunkSize = sourceStream.Read(buffer, 0, buffer.Length);
                    destinationStream.Write(buffer, 0, chunkSize);
                    bytesCounter += chunkSize;
                }
            }

            return bytesCounter;
        }

        #endregion

        #region TODO: Implement by block copy logic using MemoryStream.

        public static int InMemoryByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            char[] buffer = new char[1024];
            Encoding encoding = Encoding.UTF8;

            using var sourceStream = new StreamReader(sourcePath, false);
            using var destinationSource = new StreamWriter(destinationPath, false, encoding);

            int bytesCounter = 0;
            int chunkSize = 1;

            while (chunkSize > 0)
            {
                chunkSize = sourceStream.Read(buffer, 0, buffer.Length);
                byte[] byteArray = encoding.GetBytes(buffer);

                using var memoryStream = new MemoryStream(byteArray);
                byte[] newByteArray = memoryStream.ToArray();
                char[] charArray = encoding.GetChars(newByteArray, 0, newByteArray.Length);

                destinationSource.Write(charArray, 0, chunkSize);
                bytesCounter += chunkSize;
            }
            return bytesCounter;

        }

        #endregion

        #region TODO: Implement by block copy logic using class-decorator BufferedStream.

        public static int BufferedCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            int bufferSize = 1024;

            using var sourceStream = new FileStream(sourcePath, FileMode.Open);
            using var bufferedSourceStream = new BufferedStream(sourceStream, bufferSize);
            using var destinationStream = new FileStream(destinationPath, FileMode.Create);
            using var bufferedDestinationStream = new BufferedStream(destinationStream, bufferSize);

            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            int totalBytesReaden = bytesRead;

            do
            {
                bytesRead = bufferedSourceStream.Read(buffer, 0, bufferSize);
                bufferedDestinationStream.Write(buffer, 0, bytesRead);
                totalBytesReaden += bytesRead;
            } while (bytesRead == buffer.Length);

            return totalBytesReaden;
        }

        #endregion

        #region TODO: Implement by line copy logic using FileStream and classes text-adapters StreamReader/StreamWriter

        public static int ByLineCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using FileStream fs1 = new FileStream(sourcePath, FileMode.Open,FileAccess.Read);
            using FileStream fs2 = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

            using StreamReader sourceStream = new StreamReader(fs1, Encoding.UTF8, true);
            using StreamWriter destinationStream = new StreamWriter(fs2, Encoding.UTF8);

            string buffer = sourceStream.ReadLine();
            int countOfLines = 0;

            destinationStream.NewLine = "\n";

            while (!(buffer is null))
            {
                destinationStream.Write(buffer);
                buffer = sourceStream.ReadLine();
                if (buffer != null) destinationStream.WriteLine();
                countOfLines++;
            }

            return countOfLines;
        }

        #endregion

        #endregion

        #region Private members

        #region TODO: Implement validation logic

        private static void InputValidation(string sourcePath, string destinationPath)
        {
            if (sourcePath is null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (destinationPath == null)
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException(
                    $"File '{sourcePath}' not found. Parameter name: {nameof(sourcePath)}.");
            }

            //if (!File.Exists(destinationPath))
            //{
            //    try
            //    {
            //        File.Create(destinationPath);
            //    }
            //    catch
            //    {
            //        throw new FileNotFoundException(
            //            $"File '{destinationPath}' not found and can not be created. Parameter name: {nameof(destinationPath)}.");
            //    }
            //}

            //if (new FileInfo(destinationPath).IsReadOnly)
            //{
            //    throw new FieldAccessException(
            //        $"File '{destinationPath}' is readonly. Parameter name: {nameof(destinationPath)}.");
            //}
        }

        #endregion

        #endregion
    }
}
