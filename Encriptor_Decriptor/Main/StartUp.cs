using Encryptor_Decryptor.Main.UserRepository;
using Encryptor_Decryptor.Main.Utilities.Messages;
using Encryptor_Decryptor.Main.Utilities;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace Encryptor_Decryptor.Main
{
    public class StartUp
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            UsersRepository userRepository = new UsersRepository();
            
            #region Main Menu

            while (true)
            {
                MenuDisplay.ShowMainMenu();
                var choice = InputHandler.GetValidatedKey(new[] {ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D0});

                switch (choice)
                {
                    case ConsoleKey.D1:
                        HandleSymmetricalEncryptionDecryption();
                        break;

                    case ConsoleKey.D2:
                        LoginOrCreateAccount(userRepository);
                        break;
                    case ConsoleKey.D0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }


        
        #endregion

        #region Symmetrical Encryption/Decryption
        private static void HandleSymmetricalEncryptionDecryption()
        {
            while (true)
            {
                MenuDisplay.ShowSymmetricalMenu();
                var choice = InputHandler.GetValidatedKey(new[] { ConsoleKey.D1, ConsoleKey.D2,ConsoleKey.D3 , ConsoleKey.D0 });

                switch (choice)
                {
                    case ConsoleKey.D1:
                        HandleSymmetricalEncryption();
                        break;

                    case ConsoleKey.D2:
                        HandleSymmetricalDecryption();
                        break;

                    case ConsoleKey.D3:
                        return;

                    case ConsoleKey.D0:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }


        private static void HandleSymmetricalEncryption()
        {
            MenuDisplay.ShowSymmetricalEncryptionOptions();

            var choice = InputHandler.GetValidatedKey(new[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3 ,ConsoleKey.D0 });
            switch (choice)
            {
                case ConsoleKey.D1:
                    DirectSymmetricalEncryption();
                    break;

                case ConsoleKey.D2:
                    FileSymmetricalEncryption();
                    break;

                case ConsoleKey.D3:
                    return;

                case ConsoleKey.D0:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private static void DirectSymmetricalEncryption()
        {
            Console.Clear();
            Console.WriteLine("Write message for Encryption:");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Message cannot be empty.");
                return;
            }

            var encryptor = new Symmetrical_Encryptor(message.ToCharArray());
            encryptor.EncryptMessage();

            Console.WriteLine($"Encryption Key: {encryptor.Key}");
            Console.WriteLine($"Encrypted Message: {encryptor.Encripted}");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }

        private static void FileSymmetricalEncryption()
        {
            Console.Clear();
            Console.WriteLine("Enter the file path for Encryption:");
            var filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please try again.");
                return;
            }

            var encryptor = new File_Encryptor();
            encryptor.EncryptFile(filePath);

            Console.WriteLine($"File encrypted successfully.");
            Console.WriteLine($"Encryption Key: {encryptor.Key}");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }

        private static void HandleSymmetricalDecryption()
        {
            MenuDisplay.ShowSymmetricalDecryptionOptions();
            var choice = InputHandler.GetValidatedKey(new[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D0 });
            switch (choice)
            {
                case ConsoleKey.D1:
                    DirectSymmetricalDecryption();
                    break;

                case ConsoleKey.D2:
                    FileSymmetricalDecryption();
                    break;

                case ConsoleKey.D3:
                    return;

                case ConsoleKey.D0:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private static void DirectSymmetricalDecryption()
        {
            Console.Clear();
            Console.WriteLine("Write message for Decryption:");
            var encryptedMessage = Console.ReadLine();
            Console.WriteLine("Write encryption key:");
            var key = Console.ReadLine();

            var decryptor = new Symmetrical_Decryptor(key, encryptedMessage.ToCharArray());
            decryptor.Decrypt();

            Console.WriteLine($"Decrypted Message: {decryptor.Message}");
            Console.WriteLine();
            Console.WriteLine("Press any button to go back");
            Console.ReadKey();
        }

        private static void FileSymmetricalDecryption()
        {
            Console.Clear();
            Console.WriteLine("Enter the file path for Decryption:");
            var filePath = Console.ReadLine();
            Console.WriteLine("Enter the encryption key:");
            var key = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please try again.");
                return;
            }

            var decryptor = new File_Decryptor();
            decryptor.DecryptFile(filePath, key);

            Console.WriteLine("File decrypted successfully.");
            Console.WriteLine();
            Console.WriteLine("Press any button to go back");
            Console.ReadKey();
        }
        #endregion

        #region Asymmetrical Encryption/Decryption
        private static void LoginOrCreateAccount(UsersRepository userRepository)
        {
            while (true)
            {
                while (true)
                {
                    MenuDisplay.ShowLoginOrCreateAccount();
                    var choice = InputHandler.GetValidatedKey(new[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D0 });
                    if (choice == ConsoleKey.D1)
                    {
                        Login(userRepository);
                        break;

                    }
                    else if (choice == ConsoleKey.D2)
                    {
                        CreateAccount(userRepository);
                        break;
                    }
                    else if (choice == ConsoleKey.D3)
                    {
                        return;
                    }
                    else if (choice == ConsoleKey.D0)
                    {
                        Environment.Exit(0);
                    }
                    
                }
            }

        }

        private static void Login(UsersRepository userRepository)
        {

            Console.Clear();
            Console.WriteLine("Username:");
            string username = Console.ReadLine()!;

            if (!userRepository.Exists(username))
            {
                Console.WriteLine(ExceptionMessages.UsernameNotFound);
                Console.WriteLine("Press 0 to return to the main menu");
                if (Console.ReadKey().Key == ConsoleKey.D0)
                    return;
            }

            Console.WriteLine("Password:");
            string password;
            while (true)
            {
                password = Console.ReadLine();
                if (userRepository.GetByName(username).IsPasswordTrue(password))
                    break;
                Console.WriteLine(ExceptionMessages.IncorrectPassword);
            }

            Console.Clear();
            Console.WriteLine($"Welcome {username}");
            ShowUserMenu(username, userRepository);
            return;
        }

        private static void CreateAccount(UsersRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Create a new account");

            Console.WriteLine("Enter a username:");
            string username = Console.ReadLine()!;
            Console.WriteLine("Enter a password:");
            string password = Console.ReadLine()!;
            if (userRepository != null)
                if (userRepository.Exists(username))
                {
                    Console.WriteLine("Username already exists. Try again.");
                    return;
                }

            User newUser = new User(username, password);
            userRepository.AddNew(newUser);

            Console.WriteLine($"Account created for {username}.");
            ShowUserMenu(username, userRepository);
            return;
        }

        private static void ShowUserMenu(string username, UsersRepository userRepository)
        {
            while (true)
            {
                Console.Clear();
                int unreadFiles = Directory.GetFiles(userRepository.GetByName(username).InboxPath).Length;
                MenuDisplay.ShowUserMenu(username, unreadFiles);

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.D1)
                {
                    ReadInbox(userRepository.GetByName(username), userRepository);
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    SendMessageOrFile(username, userRepository);
                }
                else if (key.Key == ConsoleKey.D0)
                {
                    return;  // Logout and go back to the start
                }
            }
        }

        private static void ReadInbox(User user, UsersRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine($"Inbox for {user.Username}");
            Console.WriteLine();
            string[] files = Directory.GetFiles(user.InboxPath);
            if (files.Length == 0)
            {
                Console.WriteLine("Your inbox is empty.");
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Files in your inbox:");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
            }
            Console.WriteLine();
            Console.WriteLine("Enter the name of the file you want to open (or type 'back' to return):");

            while (true)
            {
                string input = Console.ReadLine();
                if (string.Equals(input, "back", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                string filePath = files.FirstOrDefault(f => Path.GetFileName(f).Equals(input, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        if (Path.GetExtension(filePath) == ".txt")
                        {
                            string encryptedContent = File.ReadAllText(filePath, Encoding.UTF8);
                            string decryptedContent = user.DecryptMessage(encryptedContent);
                            File.WriteAllText(filePath, decryptedContent, Encoding.UTF8);
                        }
                        else
                        {
                            user.DecryptFile(filePath);
                        }

                        MonitorFileAndReencrypt(filePath, user);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file: {ex.Message}");
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("File not found. Please try again or type 'back' to return:");
                }
            }
        }


        private static void MonitorFileAndReencrypt(string filePath, User user)
        {
            Console.WriteLine("Monitoring the file. Close the file after viewing to re-encrypt it.");

            // Open the file with the associated application
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Opens the file with the default application
                }
            })
            {
                process.Start();
                process.WaitForExit(); // Wait until the application is closed
            }

            // Re-encrypt the file after closing
            try
            {
                if (Path.GetExtension(filePath) == ".txt")
                {
                    string content = File.ReadAllText(filePath, Encoding.UTF8);
                    string encrypted = user.EncryptMessage(content, user.PublicKey);
                    File.WriteAllText(filePath, encrypted, Encoding.UTF8);
                }
                else
                {
                    user.EncryptFile(filePath, user.PublicKey);
                }

                Console.WriteLine("File was re-encrypted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error re-encrypting file: {ex.Message}");
            }
        }



        private static void SendMessageOrFile(string username, UsersRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Enter the username of the recipient:");
            string recipient = Console.ReadLine()!;

            if (!userRepository.Exists(recipient))
            {
                Console.WriteLine("Recipient not found. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Choose an option:");
            Console.WriteLine("Press 1 to send a text message");
            Console.WriteLine("Press 2 to send a file");
            Console.WriteLine("Press 0 to return");

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
            {
                SendMessage(username, recipient, userRepository);
            }
            else if (key.Key == ConsoleKey.D2)
            {
                SendFile(username, recipient, userRepository);
            }
            else if (key.Key == ConsoleKey.D0)
            {
                return;
            }
        }

        private static void SendMessage(string senderUsername, string recipientUsername, UsersRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Enter the message you want to send:");
            string message = Console.ReadLine()!;
            int n = 0;

            // Add message to recipient's inbox
            string recipientInbox = userRepository.GetByName(recipientUsername).InboxPath;
            string inboxPath = userRepository.GetByName(senderUsername).InboxPath;
            var files = Directory.GetFiles(recipientInbox);
            foreach (var file in files.Select(x => Path.GetExtension(x) == ".txt"))
            {
                n++;
            }
            string messageFilePath = Path.Combine(recipientInbox, $"{files.Length}-{senderUsername}_message.txt");
            string encrypted = userRepository.GetByName(senderUsername).EncryptMessage(message, userRepository.GetByName(recipientUsername).PublicKey);
            File.WriteAllText(messageFilePath, encrypted, Encoding.UTF8);

            Console.WriteLine("Message sent successfully!");
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private static void SendFile(string senderUsername, string recipientUsername, UsersRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Enter the file path you want to send:");
            string filePath = Console.ReadLine()!;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Press any key to return.");
                Console.ReadKey();
                return;
            }

            // Add file to recipient's inbox
            string recipientInbox = userRepository.GetByName(recipientUsername).InboxPath;
            string destinationPath = Path.Combine(recipientInbox, Path.GetFileName(filePath));
            File.Copy(filePath, destinationPath);
            userRepository.GetByName(senderUsername).EncryptFile(destinationPath, userRepository.GetByName(recipientUsername).PublicKey);
            Console.WriteLine("File sent successfully!");
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }
        #endregion

        #region Utility Methods

        private static ConsoleKey GetKeyInput()
        {
            while (!Console.KeyAvailable) ;
            return Console.ReadKey(true).Key;
        }
        #endregion
    }
}
