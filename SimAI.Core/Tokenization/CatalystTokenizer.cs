using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalyst;
using Catalyst.Models;
using Mosaik.Core;

namespace SimAI.Core.Tokenization {
    
    public class CatalystTokenizer : IsTokenizer {
        private bool _isInitialized;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly string _storageFolder;
        private Pipeline _pipeline;
        private Language _language;

        public CatalystTokenizer(string storageFolder = "catalyst-models") {
            _storageFolder = storageFolder;
            _language = Language.English;
        }
        
        private async Task Initialize() {

            if (_isInitialized)
                return;

            try {
                await _semaphore.WaitAsync();

                if (_isInitialized)
                    return;
                
                Mosaik.Core.Storage.Current = new OnlineRepositoryStorage(new DiskStorage(_storageFolder));

                _pipeline = await Pipeline.ForAsync(_language);
                _pipeline.Add(await AveragePerceptronEntityRecognizer.FromStoreAsync(_language, Version.Latest, "WikiNER"));
                
                var pattern = new PatternSpotter(Language.English, 0, "path", "Path");
                pattern.NewPattern(
                    "Path",
                    mp => mp.Add(
                        new PatternUnit(PatternUnitPrototype.Single().IsAlpha().WithLength(1, 1)),
                        new PatternUnit(PatternUnitPrototype.Single().WithChars(":").WithLength(1, 1)),
                        new PatternUnit(PatternUnitPrototype.Single().WithChars(@"\"))
                    ));
                
                _pipeline.Add(pattern);

                /*var document = new Document(@"C:\Code\test.txt", _language);
                _pipeline.ProcessSingle(document);

                var entities = document.EntityData;*/

                _isInitialized = true;
            }
            finally {
                _semaphore.Release();
            }
        }

        public async Task<TokenSequence> Tokenize(string input) {
            await Initialize();

            input = input.Trim();

            var document = new Document(input, _language);
            _pipeline.ProcessSingle(document);

            var sequence = new TokenSequence();
            var lastIndex = 0;

            sequence.Tokens = document.SelectMany(each => each.Tokens).Select(input => {
                string type;

                switch (input.POS) {
                    case PartOfSpeech.NUM:
                        type = TokenTypes.Number;
                        break;
                    case PartOfSpeech.SYM:
                    case PartOfSpeech.PUNCT:
                        type = TokenTypes.Symbol;
                        break;
                    default:
                        type = TokenTypes.Word;
                        break;
                }

                if (type != TokenTypes.Number && int.TryParse(input.Value, out int value))
                    type = TokenTypes.Number;

                var token = new Token(input.Value, type);

                if (input.Length == 1) {
                    if (type == TokenTypes.Number)
                        token.Tag("digit");
                    else if (type == TokenTypes.Word)
                        token.Tag("letter");
                }
                
                foreach (var entity in input.EntityTypes) {
                    token.Tag(entity.Type, entity.Tag.ToString());
                    sequence.AddEntity(entity.Type, token);
                }

                if (lastIndex + 1 < input.Begin)
                    token.HasSpace = true;

                lastIndex = input.End;
                token.Lemma = input.Lemma;

                return token;
            }).ToList();
                
            return sequence;
        }
    }
}