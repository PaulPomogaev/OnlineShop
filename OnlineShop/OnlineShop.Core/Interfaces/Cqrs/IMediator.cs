namespace OnlineShop.Core.Interfaces.Cqrs
{
    public interface IMediator
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
