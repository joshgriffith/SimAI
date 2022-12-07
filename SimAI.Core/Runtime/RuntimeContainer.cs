using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimAI.Core.Extensions;

namespace SimAI.Core.Runtime {
    public class RuntimeContainer : IDisposable {
        private IHost _serviceHost;
        private IServiceCollection _collection;
        private bool _isInitialized;
        private readonly List<ServiceDescriptor> _serviceDescriptors = new();

        public void Initialize() {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _serviceHost = new HostBuilder()
                .ConfigureLogging(builder => {
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .ConfigureServices((context, services) => {
                    _collection = services;

                    foreach (var descriptor in _serviceDescriptors)
                        services.Add(descriptor);
                })
                .Build();
        }

        public async Task Run(Func<IServiceScope, Task> action) {
            using var scope = _serviceHost.Services.CreateScope();
            await action(scope);
        }

        public void Dispose() {
            _serviceHost?.Dispose();
        }

        public T Get<T>() {
            Initialize();
            return _serviceHost.Services.GetService<T>();
        }

        public async Task With<T>(Func<T, Task> action) {
            Initialize();

            using var scope = _serviceHost.Services.CreateScope();
            var service = _serviceHost.Services.GetService<T>();
            await action(service);
        }

        public void Use(object service) {
            var descriptor = new ServiceDescriptor(service.GetType(), service);
            _serviceDescriptors.Add(descriptor);

            foreach (var each in GetTypes(service.GetType()))
                _serviceDescriptors.Add(new ServiceDescriptor(each, service));
        }

        public void Use<T>(ServiceLifetime lifetime) {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(T), typeof(T), lifetime));

            foreach (var each in GetTypes(typeof(T)))
                _serviceDescriptors.Add(new ServiceDescriptor(each, provider => provider.GetRequiredService(typeof(T)), lifetime));
        }
        
        private static IEnumerable<Type> GetTypes(Type type) {
            return type.GetBaseTypes().Where(each => each.Namespace != null && !each.Namespace.StartsWith("System"));
        }
    }
}