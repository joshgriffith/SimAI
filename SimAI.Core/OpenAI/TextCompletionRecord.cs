using System;
using SimAI.Core.Database;

namespace SimAI.Core.OpenAI {
    public class TextCompletionRecord : CosmosDocument {
        
        public long Time { get; set; }
        public int Latency { get; set; }
        public string Response { get; set; }
        public string FinishReason { get; set; }
        public OpenAICompletionQuery Query { get; set; }

        public TextCompletionRecord() {
            Id = Guid.NewGuid().ToString();
        }

        public TextCompletionRecord(OpenAICompletionResult result) : this() {
            Query = result.Query;
            Time = result.QueryTime;
            Response = result.Text;
            Latency = result.Latency;
            FinishReason = result.FinishReason;
            Partition = result.Query.GetPartitionKey();
        }
    }
}