using System.Text;

namespace DollCode;

class Program
{
    // Character set for encoding/decoding
    class CharacterSet
    {
        public string Char1 { get; set; }
        public string Char2 { get; set; }
        public string Char3 { get; set; }
        public string Separator { get; set; }

        public CharacterSet(string char1, string char2, string char3, string separator)
        {
            Char1 = char1;
            Char2 = char2;
            Char3 = char3;
            Separator = separator;
        }

        // Default dollcode characters
        public static CharacterSet Default => new CharacterSet(
            "\u2596", // ▖ (bottom left)
            "\u2598", // ▘ (top left)
            "\u258C", // ▌ (full left)
            "\u200D"  // Zero-width joiner
        );

        public void Display()
        {
            Console.WriteLine($"  Char1 (1): {Char1}");
            Console.WriteLine($"  Char2 (2): {Char2}");
            Console.WriteLine($"  Char3 (3): {Char3}");
            Console.WriteLine($"  Separator: {(Separator == "\u200D" ? "[zero-width joiner]" : Separator)}");
        }
    }

    static CharacterSet currentCharSet = CharacterSet.Default;

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            RunREPL();
            return;
        }

        string command = args[0].ToLower();

        switch (command)
        {
            case "encode":
            case "e":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: Please provide text to encode");
                    ShowUsage();
                    return;
                }
                string textToEncode = string.Join(" ", args.Skip(1));
                string encoded = Encode(textToEncode, currentCharSet);
                Console.WriteLine(encoded);
                break;

            case "decode":
            case "d":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: Please provide dollcode to decode");
                    ShowUsage();
                    return;
                }
                string textToDecode = string.Join(" ", args.Skip(1));
                string decoded = Decode(textToDecode, currentCharSet);
                Console.WriteLine(decoded);
                break;

            case "repl":
            case "interactive":
                RunREPL();
                break;

            case "help":
            case "-h":
            case "--help":
                ShowUsage();
                break;

            default:
                Console.WriteLine($"Error: Unknown command '{command}'");
                ShowUsage();
                break;
        }
    }

    static void RunREPL()
    {
        Console.WriteLine("╔═══════════════════════════════════════════╗");
        Console.WriteLine("║   DollCode Text Encoder/Decoder - REPL   ║");
        Console.WriteLine("╚═══════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  encode <text>  or  e <text>  - Encode text");
        Console.WriteLine("  decode <code>  or  d <code>  - Decode dollcode");
        Console.WriteLine("  set <1|2|3|sep> <char>       - Set character mapping");
        Console.WriteLine("  show                         - Show current character set");
        Console.WriteLine("  reset                        - Reset to default characters");
        Console.WriteLine("  help                         - Show this help");
        Console.WriteLine("  exit  or  quit               - Exit REPL");
        Console.WriteLine();

        while (true)
        {
            Console.Write("dollcode> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                continue;

            string command = parts[0].ToLower();

            switch (command)
            {
                case "encode":
                case "e":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Error: Please provide text to encode");
                        break;
                    }
                    try
                    {
                        string encoded = Encode(parts[1], currentCharSet);
                        Console.WriteLine(encoded);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                case "decode":
                case "d":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Error: Please provide dollcode to decode");
                        break;
                    }
                    try
                    {
                        string decoded = Decode(parts[1], currentCharSet);
                        Console.WriteLine(decoded);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                case "set":
                    HandleSetCommand(parts.Length > 1 ? parts[1] : "");
                    break;

                case "show":
                    Console.WriteLine("Current character set:");
                    currentCharSet.Display();
                    break;

                case "reset":
                    currentCharSet = CharacterSet.Default;
                    Console.WriteLine("Character set reset to default");
                    currentCharSet.Display();
                    break;

                case "help":
                case "?":
                    ShowREPLHelp();
                    break;

                case "exit":
                case "quit":
                case "q":
                    Console.WriteLine("Goodbye!");
                    return;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    Console.WriteLine("Type 'help' for available commands");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void HandleSetCommand(string args)
    {
        string[] parts = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            Console.WriteLine("Usage: set <1|2|3|sep> <character>");
            Console.WriteLine("Examples:");
            Console.WriteLine("  set 1 ⭐");
            Console.WriteLine("  set 2 ⚡");
            Console.WriteLine("  set 3 ✨");
            Console.WriteLine("  set sep |");
            return;
        }

        string target = parts[0].ToLower();
        string newChar = parts[1];

        switch (target)
        {
            case "1":
            case "char1":
                currentCharSet.Char1 = newChar;
                Console.WriteLine($"Char1 set to: {newChar}");
                break;

            case "2":
            case "char2":
                currentCharSet.Char2 = newChar;
                Console.WriteLine($"Char2 set to: {newChar}");
                break;

            case "3":
            case "char3":
                currentCharSet.Char3 = newChar;
                Console.WriteLine($"Char3 set to: {newChar}");
                break;

            case "sep":
            case "separator":
                currentCharSet.Separator = newChar;
                Console.WriteLine($"Separator set to: {newChar}");
                break;

            default:
                Console.WriteLine($"Unknown target: {target}");
                Console.WriteLine("Valid targets: 1, 2, 3, sep");
                break;
        }
    }

    static void ShowREPLHelp()
    {
        Console.WriteLine("DollCode REPL Commands:");
        Console.WriteLine();
        Console.WriteLine("Encoding/Decoding:");
        Console.WriteLine("  encode <text>  - Encode text to dollcode");
        Console.WriteLine("  e <text>       - Short form of encode");
        Console.WriteLine("  decode <code>  - Decode dollcode to text");
        Console.WriteLine("  d <code>       - Short form of decode");
        Console.WriteLine();
        Console.WriteLine("Character Set:");
        Console.WriteLine("  set 1 <char>   - Set character for digit 1 (default: ▖)");
        Console.WriteLine("  set 2 <char>   - Set character for digit 2 (default: ▘)");
        Console.WriteLine("  set 3 <char>   - Set character for digit 3 (default: ▌)");
        Console.WriteLine("  set sep <char> - Set separator character (default: zero-width joiner)");
        Console.WriteLine("  show           - Show current character mappings");
        Console.WriteLine("  reset          - Reset to default characters");
        Console.WriteLine();
        Console.WriteLine("Other:");
        Console.WriteLine("  help           - Show this help");
        Console.WriteLine("  exit, quit, q  - Exit REPL");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  set 1 🌟");
        Console.WriteLine("  set 2 ⭐");
        Console.WriteLine("  set 3 ✨");
        Console.WriteLine("  set sep |");
        Console.WriteLine("  encode Hello!");
    }

    static void ShowUsage()
    {
        Console.WriteLine("DollCode Text Encoder/Decoder");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dollcode                  - Start interactive REPL mode");
        Console.WriteLine("  dollcode encode <text>    - Encode text to dollcode");
        Console.WriteLine("  dollcode e <text>         - Short form of encode");
        Console.WriteLine("  dollcode decode <code>    - Decode dollcode to text");
        Console.WriteLine("  dollcode d <code>         - Short form of decode");
        Console.WriteLine("  dollcode help             - Show this help message");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  dollcode                  - Start REPL");
        Console.WriteLine("  dollcode encode \"Hello, World!\"");
        Console.WriteLine("  dollcode decode \"▌▘▖▘‍▖▘▖▖‍▌▖▖▖‍▌▖▖▖‍...\"");
    }

    /// <summary>
    /// Encodes text to dollcode using a base-3 system with digits {1,2,3}
    /// mapped to the provided character set. Each character's Unicode codepoint is
    /// encoded separately and groups are separated by the separator character.
    /// </summary>
    static string Encode(string text, CharacterSet charSet)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var encodedChars = new List<string>();

        foreach (char c in text)
        {
            int codepoint = (int)c;
            string encoded = EncodeNumber(codepoint, charSet);
            encodedChars.Add(encoded);
        }

        // Join with separator and add trailing separator
        return string.Join(charSet.Separator, encodedChars) + charSet.Separator;
    }

    /// <summary>
    /// Encodes a single number using base-3 with digits {1,2,3}
    /// The result is reversed to have MSB (most significant bit) first
    /// </summary>
    static string EncodeNumber(int number, CharacterSet charSet)
    {
        if (number == 0)
            return charSet.Char1; // Edge case: encode 0 as Char1

        var result = new StringBuilder();
        int window = number;

        while (window > 0)
        {
            int remainder = window % 3;
            int r = remainder == 0 ? 3 : remainder;
            window = (window - r) / 3;

            // Map: 1→Char1, 2→Char2, 3→Char3
            string digit = r switch
            {
                1 => charSet.Char1,
                2 => charSet.Char2,
                3 => charSet.Char3,
                _ => throw new InvalidOperationException($"Unexpected digit value: {r}")
            };

            result.Append(digit);
        }

        // Reverse the result for MSB-first encoding
        return new string(result.ToString().Reverse().ToArray());
    }

    /// <summary>
    /// Decodes dollcode string back to text using the provided character set
    /// </summary>
    static string Decode(string dollcode, CharacterSet charSet)
    {
        if (string.IsNullOrEmpty(dollcode))
            return string.Empty;

        // Split by separator and filter out empty groups
        var groups = dollcode.Split(new[] { charSet.Separator }, StringSplitOptions.RemoveEmptyEntries);

        var result = new StringBuilder();

        foreach (string group in groups)
        {
            int? codepoint = DecodeGroup(group, charSet);
            if (codepoint.HasValue && codepoint.Value >= 0 && codepoint.Value <= 0x10FFFF)
            {
                result.Append((char)codepoint.Value);
            }
            else
            {
                result.Append('?'); // Invalid encoding
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Decodes a single dollcode group to a number
    /// Reads left-to-right with MSB first
    /// </summary>
    static int? DecodeGroup(string group, CharacterSet charSet)
    {
        if (string.IsNullOrEmpty(group))
            return null;

        int accumulator = 0;

        // Handle multi-character symbols (like emojis)
        int i = 0;
        while (i < group.Length)
        {
            int digit = -1;

            // Try to match each character from the set
            if (group.Substring(i).StartsWith(charSet.Char3))
            {
                digit = 3;
                i += charSet.Char3.Length;
            }
            else if (group.Substring(i).StartsWith(charSet.Char2))
            {
                digit = 2;
                i += charSet.Char2.Length;
            }
            else if (group.Substring(i).StartsWith(charSet.Char1))
            {
                digit = 1;
                i += charSet.Char1.Length;
            }
            else
            {
                return null; // Invalid character
            }

            accumulator = accumulator * 3 + digit;
        }

        return accumulator;
    }
}
