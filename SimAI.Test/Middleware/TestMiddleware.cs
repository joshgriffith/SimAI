using System;
using System.Threading.Tasks;
using SimAI.Core.Runtime;

namespace SimAI.Test.Middleware {
    public class TestMiddleware : IsQueryHandler {
        public async Task Handle(QueryContext query) {
            if (query.Request == "ping")
                query.Response = "pong";
        }
    }
}