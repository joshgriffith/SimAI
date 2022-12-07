using System;
using System.Threading.Tasks;
using SimAI.Core.Extensions;
using SimAI.Core.Runtime;

namespace SimAI.Core.ResponseHandlers {
    public class UnknownRequestHandler : IsQueryHandler {
        public const string DefaultMessage = "Unknown request.";
        private readonly string _message;

        public UnknownRequestHandler(string message = DefaultMessage) {
            _message = message;
        }

        public async Task Handle(QueryContext query) {
            if (query.Response.IsEmpty()) {
                query.Response = _message;
            }
        }
    }
}