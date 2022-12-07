using System;
using System.Threading.Tasks;

namespace SimAI.Core.Runtime {
    public interface IsQueryHandler {
        Task Handle(QueryContext query);
    }
}