using Microsoft.Extensions.DependencyInjection;
using MidR.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MidR.Implementation
{
    public sealed class Mediator : IMediator
    {
        private const string EXECUTE_METHOD = "ExecuteAsync";

        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType)
                ?? throw new InvalidOperationException($"No handler registered for request type {request.GetType()}");

            return await (Task<TResponse>)handlerType
                .GetMethod(EXECUTE_METHOD)!
                .Invoke(handler, new object[] { request, cancellationToken })!;
        }

        public async Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                await (Task)handlerType
                    .GetMethod(EXECUTE_METHOD)!
                    .Invoke(handler, new object[] { notification, cancellationToken })!;
            }
        }
    }
}