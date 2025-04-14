namespace MidR.Interfaces
{
    public interface IMediator
    {
        Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

        Task NotifyAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification;
    }
}