using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Encryptor_Decryptor.Main.UserRepository
{
    public class UsersRepository
    {
        private List<User> _users = new();
        private string _repositoryFilePath = "../../../Data/UserRepository/UserRepository.txt";
        private string _usersDirectory = "../../../Data/Users";

        public UsersRepository()
        {
            // Ensure that the UserRepository and Users directories exist
            EnsureDirectoryExists("../../../Data/UserRepository");
            EnsureDirectoryExists(_usersDirectory);

            // Read existing users from the repository file if it exists
            if (File.Exists(_repositoryFilePath))
            {
                using (StreamReader tr = new StreamReader(_repositoryFilePath))
                {

                    string[] rows = File.ReadAllText(_repositoryFilePath, Encoding.UTF8).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows != null)
                    foreach (var row in rows)
                    {
                        string[] data = row.Split(' ');
                        if (data.Length >= 2)
                        {
                            string username = data[0];
                            string password = data[1];
                            string publicKey = data[3];
                            string privateKey = data[4];
                            User user = new User(username, password, publicKey, privateKey);
                            _users.Add(user);
                        }
                    }
                }
            }
        }

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        public void AddNew(User user)
        {
            _users.Add(user);
            File.AppendAllText(_repositoryFilePath, $"{user.Username} {user.Password} {user.InboxPath} {user.PublicKey} {user.PrivateKey}\n", Encoding.UTF8);
        }

        public User GetByName(string name)
        {
            return _users.FirstOrDefault(x => x.Username == name);
        }

        public bool Exists(string name)
        {
            return _users.Any(x => x.Username == name);
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword);
            return hashedEnteredPassword == storedHashedPassword;
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
