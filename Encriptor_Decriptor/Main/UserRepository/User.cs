using Encryptor_Decryptor.Main.Utilities.Messages;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Encryptor_Decryptor.Main.UserRepository
{
    public class User
    {
        private string _username;
        private string _password;
        private string _inboxPath;
        private string _publicKey;
        private string _privateKey;

        public User(string username, string password)
        {
            Username = username;
            Password = password;

            // Define the root Users directory and user-specific directory
            string usersDirectory = "../../../Data/Users";
            string userDirectory = "../../../Data/Users/" + _username;

            // Ensure that the Users directory exists
            if (!Directory.Exists(usersDirectory))
            {
                Directory.CreateDirectory(usersDirectory);
            }

            // Ensure that the user-specific directory exists
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }
            (_publicKey, _privateKey) = GenerateKeys();
        
            _inboxPath = userDirectory + "/";  // Correct path to user's inbox directory
        }
        public User( string username, string password,string PublicKey, string privateKey )
        {
            Username = username;
            _password = password;

            // Define the root Users directory and user-specific directory
            string usersDirectory = "../../../Data/Users";
            string userDirectory = Path.Combine(usersDirectory, _username);

            // Ensure that the Users directory exists
            if (!Directory.Exists(usersDirectory))
            {
                Directory.CreateDirectory(usersDirectory);
            }

            // Ensure that the user-specific directory exists
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            _inboxPath = userDirectory + "/";  // Correct path to user's inbox directory
            _publicKey = PublicKey;
            _privateKey = privateKey;
        
        }

        public string Username
        {
            get => _username;
            private init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(string.Format(ExceptionMessages.UsernameRequired));
                if (value.Length < 3 || value.Length > 20)
                    throw new ArgumentException("Username must be between 3 and 20 characters.");
                // Additional validation rules could be added here
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            private init
            {
                if (value.Length < 6)
                    throw new ArgumentException(ExceptionMessages.PasswordError);
                _password = HashPassword(value); // Hash password on initialization
            }
        }

        public string InboxPath => _inboxPath;

        public string PublicKey => _publicKey;

        public string PrivateKey => _privateKey;

        // Method to hash the password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Use this method to compare entered passwords with the stored hashed password
        public bool IsPasswordTrue(string enteredPassword)
        {
            return UsersRepository.VerifyPassword(enteredPassword, _password);
        }
        private (string publicKey, string privateKey) GenerateKeys()
        {
            Random random = new Random();
            StringBuilder publicKeySb = new StringBuilder();
            StringBuilder privateKeySb = new StringBuilder();
            int l = 0;
            for (int i = 1; i < 6; i++) // Generate keys of length 15 (you can adjust the length)
            {
                int r = random.Next(105, 200); // Generate a random number for characters
                if (l == 0)
                    l = ((int)Username.Length % 2 + i); // Add some user-specific logic


                privateKeySb.Append(r + l); // Private key, interrelated with public key
                publicKeySb.Append(r - l); // Public key
            }
            publicKeySb.Append(l);
            privateKeySb.Append(l);



            // Return the generated keys as public/private pairs
            return (publicKeySb.ToString(), privateKeySb.ToString());
        }

        // Encrypt message using the public key
        public string EncryptMessage(string message, string publicKey)
        {
            StringBuilder encryptedMessage = new StringBuilder();
            int n = 0;
            int l = int.Parse(_privateKey.Substring(15));
            for (int i = 0; i < message.Length; i++)
            {
                if (n >= 15)
                    n = 0;
                if (message[i] == 32)
                {
                    encryptedMessage.Append((char)message[i]);
                    continue;
                }
                   
                encryptedMessage.Append((char)(message[i] + int.Parse(publicKey.Substring(n, 3)) + l));
                n += 3;
            }
            return encryptedMessage.ToString();
        }

        // Decrypt message using the private key
        public string DecryptMessage(string encryptedMessage)
        {
            StringBuilder decryptedMessage = new StringBuilder();
            int n = 0;
            int l = int.Parse(_privateKey.Substring(15));
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                if (n >= 15)
                    n = 0;
                if (encryptedMessage[i] == 32)
                {
                    decryptedMessage.Append((char)encryptedMessage[i]);
                    continue;
                }
                decryptedMessage.Append((char)(encryptedMessage[i] - int.Parse(_privateKey.Substring(n, 3)) + l));
                n += 3;
            }
            return decryptedMessage.ToString();
        }

        public void EncryptFile(string path, string publicKey)
        {
           
            int n = 0;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {

                byte[] buffer = new byte[fileStream.Length];
                int l = int.Parse(_privateKey.Substring(15));
                fileStream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i++)
                {

                    buffer[i] = (byte)(buffer[i] ^ (int.Parse(publicKey.Substring(n, 3))+ l));
                    n += 3;
                    if (n == 15)
                        n = 0;
                }
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
            }
        }
        public void DecryptFile(string path)
        {

            int n = 0;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    int l = int.Parse(_privateKey.Substring(15));
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (byte)(buffer[i] ^ (int.Parse(_privateKey.Substring(n, 3)) - l));
                        n += 3;

                        // Ensure n wraps around correctly
                        if (n >= _privateKey.Length - 1)
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
