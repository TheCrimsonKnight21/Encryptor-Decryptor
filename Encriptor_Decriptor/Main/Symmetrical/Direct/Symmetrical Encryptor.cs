using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor_Decryptor
{
    public class Symmetrical_Encryptor
    {
        private Queue<char> message = new();
        private string encrypted;
        private string key;

        public Symmetrical_Encryptor(char[] arr)
        {
            foreach (char c in arr)
            {
                message.Enqueue(c);
            }
        }
        public string Key => key;

        public string Encripted => encrypted;
        public void EncryptMessage()
        {
            GenerateKey();
            StringBuilder sb = new StringBuilder();
            while (message.Count > 0)
            {
                
                for (int i = 0; i < key.Length; i += 3)
                {
                    if (message.Count == 0)
                        break;
                    if (((int)message.Peek()) == 32)
                    {
                        sb.Append(message.Dequeue().ToString());
                        continue;
                    }
                    int b = int.Parse(key.Substring(i, 3));

                    char ch = Convert.ToChar((message.Dequeue()) + b);
                    sb.Append(ch.ToString());
                    
                }
            }
            encrypted = sb.ToString();
        }

        private void GenerateKey()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int r = random.Next(97, 127);
                int l = ((int)message.Peek() % 2 + i);
                sb.Append(r + l);
            }
            key = sb.ToString();
        }
    }
}