using System.Threading.Tasks;
using SimAI.Core.Extensions;
using SimAI.Core.OpenAI;

namespace SimAI.Knowledge.CodeGeneration {
    public class NaturalLanguageQuery {
        private readonly OpenAIProvider _provider;

        public NaturalLanguageQuery(OpenAIProvider provider) {
            _provider = provider;
        }

        public async Task<string> Prompt<T>(string query) {
            
            var implementation = await typeof(T).GetSourceCodeAsync();
            
            var prompt = string.Format(
@"{0}

public class {1}Repository : BaseRepository {{

    public IQueryable<{1}> GetAll() {{
        return Get();
    }}
    
    // Get records with an id of 100
    public IQueryable<{1}> GetById100() {{
        return Get().Where(x => x.Id == 100);
    }}

    // {2}
    public
", implementation, typeof(T).Name, query);
            
            var code = await _provider.CompleteMethod(prompt, stops: "}");
            code = code.Replace("{", "").Replace("return ", "").Replace("Get().", "").Trim();
            return code;
        }
    }
}