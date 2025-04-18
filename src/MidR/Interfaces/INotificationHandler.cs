﻿using System.Threading;
using System.Threading.Tasks;

namespace MidR.Interfaces
{
    public interface INotificationHandler<TNotification>
        where TNotification : INotification
    {
        Task ExecuteAsync(TNotification notification, CancellationToken cancellationToken);
    }
}