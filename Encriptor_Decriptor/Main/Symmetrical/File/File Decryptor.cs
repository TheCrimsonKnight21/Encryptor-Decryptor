using System;
using System.IO;

namespace Encryptor_Decryptor
{
    public class File_Decryptor
    {
        private const int KeySegmentLength = 3; // Length of each segment in the key

        public void DecryptFile(string path, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length % KeySegmentLength != 0)
            {
                throw new ArgumentException("Invalid key length. The key must be a multiple of " + KeySegmentLength + " characters.");
            }

            int n = 0;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (byte)(buffer[i] ^ int.Parse(key.Substring(n, KeySegmentLength)));
                        n += KeySegmentLength;

                        // Ensure n wraps around correctly
                        if (n >= key.Length)
                            n = 0;
                    }

                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting file: {ex.Message}");
            }
        }
    }
}
