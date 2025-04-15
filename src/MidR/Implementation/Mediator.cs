using Microsoft.Extensions.DependencyInjection;
using MidR.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MidR.Implementation
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> DispatchAsync <TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

            return await (Task<TResponse>)handlerType
                .GetMethod("ExecuteAsync")!
                .Invoke(handler, new object[] { request, cancellationToken })!;
        }

        public async Task NotifyAsync <TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
            var handlers = _provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                await (Task)handlerType
                    .GetMethod("ExecuteAsync")!
                    .Invoke(handler, new object[] { notification, cancellationToken })!;
            }
        }
    }
}