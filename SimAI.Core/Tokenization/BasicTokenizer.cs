using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimAI.Core.Tokenization {
    public class BasicTokenizer : IsTokenizer {
        private static readonly Regex LetterRegex = new(@"^[a-zA-Z]");
        private static readonly Regex DigitRegex = new(@"^[0-9]");

        public async Task<TokenSequence> Tokenize(string input) {
            var sequence = new TokenSequence();
            Token token = null;

            Token AddToken(string type) {
                var newToken = new Token(string.Empty, type);
                sequence.Tokens.Add(newToken);
                return newToken;
            }

            foreach (var character in input) {
                if (character is ' ') {
                    token = null;
                    continue;
                }

                if (DigitRegex.IsMatch(character.ToString())) {
                    token ??= AddToken("number");
                }
                else if (LetterRegex.IsMatch(character.ToString())) {
                    token ??= AddToken("word");
                    token.Type = "word";
                }
                else if (character == '.' && token != null && token.Type == "number") {
                    token.Value += character;
                    continue;
                }
                else {
                    AddToken("symbol").Value += character;
                    token = null;
                    continue;
                }

                token.Value += character;
            }

            return sequence;
        }
    }
}