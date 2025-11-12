# DollCode Text Encoder/Decoder

A C# implementation of the text-compatible dollcode encoding system, reverse-engineered from the now-defunct dollcode.v01dlabs.sh website.

## About

This project recreates the text encoding functionality from the original dollcode site that no longer exists. It was reverse-engineered using:
- The [dollcode.rs](https://codeberg.org/ember-ana/dollcode.rs) Rust crate (which only supports number encoding)
- Information from [noe.sh/dollcode](https://noe.sh/dollcode) about the base-3 encoding system
- Analysis of example encoded strings to determine the text encoding algorithm

Dollcode is a trinary (base-3) encoding system using Unicode block characters. The original site supported encoding both pure numbers and text, but used different encoding schemes for each. This implementation focuses on the **text-compatible version**.

### How It Works

- Each character is converted to its Unicode codepoint
- The codepoint is encoded using base-3 with digits {1,2,3}
- Digits are mapped to characters: 1→▖, 2→▘, 3→▌
- Character groups are separated by zero-width joiners (U+200D)
- Example: `Hello` → `▘▖▘▌‍▌▖▌▘‍▌▘▘▌‍▌▘▘▌‍▌▘▌▌‍`

## Usage

### Interactive REPL Mode

Run without arguments to start the interactive mode:

```bash
dotnet run
```

Commands available in REPL:
```
encode <text>  or  e <text>  - Encode text to dollcode
decode <code>  or  d <code>  - Decode dollcode to text
set <1|2|3|sep> <char>       - Set character mapping
show                         - Show current character set
reset                        - Reset to default characters
help                         - Show help
exit  or  quit               - Exit REPL
```

### Command-line Mode

Encode text:
```bash
dotnet run -- encode "Hello, World!"
dotnet run -- e "Hello, World!"
```

Decode dollcode:
```bash
dotnet run -- decode "▘▖▘▌‍▌▖▌▘‍▌▘▘▌‍▌▘▘▌‍▌▘▌▌‍"
dotnet run -- d "▘▖▘▌‍▌▖▌▘‍▌▘▘▌‍▌▘▘▌‍▌▘▌▌‍"
```

### Custom Character Sets

You can customize the characters used for encoding! This works with emojis and any Unicode characters:

```
dollcode> set 1 🌟
dollcode> set 2 ⭐
dollcode> set 3 ✨
dollcode> set sep |
dollcode> show
Current character set:
  Char1 (1): 🌟
  Char2 (2): ⭐
  Char3 (3): ✨
  Separator: |

dollcode> e Hello
⭐🌟⭐✨|✨🌟✨⭐|✨⭐⭐✨|✨⭐⭐✨|✨⭐✨✨|
```

## Examples

```bash
# Standard encoding
$ dotnet run -- encode "#364,998"
▌▘▘‍▖▘▖▌‍▖▘▘▌‍▖▘▘▖‍▖▖▘▘‍▖▘▌▌‍▖▘▌▌‍▖▘▌▘‍

# Decoding
$ dotnet run -- decode "▌▘▘‍▖▘▖▌‍▖▘▘▌‍▖▘▘▖‍▖▖▘▘‍▖▘▌▌‍▖▘▌▌‍▖▘▌▘‍"
#364,998

# Interactive mode
$ dotnet run
╔═══════════════════════════════════════════╗
║   DollCode Text Encoder/Decoder - REPL   ║
╚═══════════════════════════════════════════╝

dollcode> e dollcode
▌▖▌▖‍▌▘▌▌‍▌▘▘▌‍▌▘▘▌‍▌▖▘▌‍▌▘▌▌‍▌▖▌▖‍▌▖▌▘‍

dollcode> exit
```

## Building

Requires .NET 9.0 SDK:

```bash
dotnet build
dotnet run
```

## Technical Details

- **Default Characters**: ▖ (U+2596), ▘ (U+2598), ▌ (U+258C)
- **Default Separator**: Zero-width joiner (U+200D)
- **Encoding**: Base-3 system with digits {1,2,3} instead of {0,1,2}
- **Character Order**: Most significant bit first (MSB-first)

## Differences from Number-Only Encoding

The Rust crate at [dollcode.rs](https://codeberg.org/ember-ana/dollcode.rs) implements number-only encoding, which produces different output than text encoding. This implementation focuses on the text-compatible version that was available on the original dollcode.v01dlabs.sh site.

## License

This is a reverse-engineered implementation for educational and practical use.