using System.Threading.Tasks;

namespace SimAI.Core.Tokenization {
    public interface IsTokenizer {
        Task<TokenSequence> Tokenize(string input);
    }
}