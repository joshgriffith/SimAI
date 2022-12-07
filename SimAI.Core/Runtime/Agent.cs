using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimAI.Core.Runtime {
    public class Agent : IDisposable {
        private readonly RuntimeContainer _runtime;

        public Agent() {
            _runtime = new RuntimeContainer();
        }

        public void Dispose() {
            _runtime.Dispose();
        }

        public void Use<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped) {
            _runtime.Use<T>(lifetime);
        }

        public void Use(object service) {
            _runtime.Use(service);
        }

        public async Task With<T>(Func<T, Task> action) {
            await _runtime.With(action);
        }
        
        public async Task<string> Execute(string request) {
            _runtime.Initialize();

            var query = new QueryContext();
            query.Request = request;

            await _runtime.Run(async scope => {
                var handlers = scope.ServiceProvider.GetServices<IsQueryHandler>();

                foreach (var handler in handlers)
                    await handler.Handle(query);
            });

            return query.Response;
        }
    }
}