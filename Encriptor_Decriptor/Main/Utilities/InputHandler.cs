namespace Encryptor_Decryptor.Main.Utilities
{
    public static class InputHandler
    {
        public static ConsoleKey GetValidatedKey(ConsoleKey[] validKeys)
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (validKeys.Contains(key))
                {
                    return key;
                }

                MenuDisplay.ShowError("Invalid choice. Please try again.");
            }
        }

        public static string GetNonEmptyInput(string prompt)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine()?.Trim()!;
                if (string.IsNullOrWhiteSpace(input))
                {
                    MenuDisplay.ShowError("Input cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }
    }
}
