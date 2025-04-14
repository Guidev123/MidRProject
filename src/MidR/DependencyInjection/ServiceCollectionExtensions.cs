using Microsoft.Extensions.DependencyInjection;
using MidR.Implementation;
using MidR.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace MidR.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMidR(this IServiceCollection services, params object[] args)
        {
            var assemblies = ResolveAssemblies(args);

            services.AddSingleton<IMediator, Mediator>();

            RegisterHandlers(services, assemblies, typeof(IRequestHandler<,>));
            RegisterHandlers(services, assemblies, typeof(INotificationHandler<>));
        }

        private static Assembly[] ResolveAssemblies(object[] args)
        {
            if (args.All(a => a is string))
            {
                var prefixes = args.Cast<string>().ToArray();
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic
                    && !string.IsNullOrWhiteSpace(a.FullName)
                    && prefixes.Any(p => a.FullName.StartsWith(p)))
                    .ToArray();
            }

            throw new ArgumentException("Invalid arguments. Provide either string prefixes.");
        }

        private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies, Type handlerInterface)
        {
            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == handlerInterface);

                foreach (var @interface in interfaces)
                {
                    services.AddTransient(@interface, type);
                }
            }
        }
    }
}