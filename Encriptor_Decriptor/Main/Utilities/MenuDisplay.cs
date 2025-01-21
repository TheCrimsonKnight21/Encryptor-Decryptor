using System.Text;

namespace Encryptor_Decryptor.Main.Utilities
{
    public static class MenuDisplay
    {
        public static void ShowMainMenu()
        {
            Console.Clear();
            ShowHeader("Main Menu");
            Console.WriteLine("1. Symmetrical Encrypting/Decrypting");
            Console.WriteLine("2. Asymmetric Encrypting/Decrypting");
            Console.WriteLine("0. Quit");
        }

        public static void ShowHeader(string title)
        {
            Console.Clear();
            
            Console.WriteLine("================================================================================");
            Console.SetCursorPosition(40 - title.Length / 2, 1);
            Console.WriteLine($"{title.ToUpper()}");
            Console.WriteLine("================================================================================");
        }

        public static void ShowError(string errorMessage)
        {
            Console.WriteLine("\n[Error]: " + errorMessage);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ShowMessage(string message)
        {
            Console.WriteLine("\n[Message]: " + message);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ShowSymmetricalMenu()
        {
            ShowHeader("Symmetrical Encryption/Decryption");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Back");
            Console.WriteLine("0. Quit");
        }

        public static void ShowSymmetricalEncryptionOptions()
        {
            ShowHeader("Encryption Options");
            Console.WriteLine("1. Direct Encrypting");
            Console.WriteLine("2. File Encrypting");
            Console.WriteLine("3. Back");
            Console.WriteLine("0. Quit");
        }

        public static void ShowSymmetricalDecryptionOptions()
        {
            ShowHeader("Decryption Options");
            Console.WriteLine("1. Direct Decrypting");
            Console.WriteLine("2. File Decrypting");
            Console.WriteLine("3. Back");
            Console.WriteLine("0. Quit");
        }

        public static void ShowLoginOrCreateAccount()
        {
            ShowHeader("You must be logged in to send encrypted messages/files!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Create Account");
            Console.WriteLine("3. Back");
            Console.WriteLine("0. Quit");
        }

        public static void ShowUserMenu(string user, int unread)
        {
            ShowHeader($"Welcome {user}");
            Console.WriteLine($"You have {unread} file(s) in your inbox.");
            Console.WriteLine("1. Read inbox");
            Console.WriteLine("2. Send a message/file to another user");
            Console.WriteLine("0. Log Out");
        }
    }
}
