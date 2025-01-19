using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor_Decryptor
{
    public class Symmetrical_Decryptor
    {
        private string _key;
        private Queue<char> _encrypted = new();
        private string _message;

        public Symmetrical_Decryptor(string key, char[] encrypted)
        {
            this._key = key;
            foreach (char c in encrypted)
            {
                _encrypted.Enqueue(c);
            }
        }

        public string Message => _message;

        public void Decrypt()
        {
            StringBuilder sb = new StringBuilder();
            while (_encrypted.Count > 0)
            {
                
                for (int i = 0; i < _key.Length; i += 3)
                {
                    if (_encrypted.Count == 0)
                        break;

                    if (((int)_encrypted.Peek()) == 32)
                    {
                        sb.Append(_encrypted.Dequeue().ToString());
                        continue;
                    }
                    int b = int.Parse(_key.Substring(i, 3));

                    char ch = Convert.ToChar((_encrypted.Dequeue()) - b);
                    sb.Append(ch.ToString());
                    
                }
            }
            _message = sb.ToString();
        }
    }
}