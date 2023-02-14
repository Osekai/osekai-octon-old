using Osekai.Octon.HelperTypes;
using Osekai.Octon.Models;
using Osekai.Octon.Persistence;

namespace Osekai.Octon.RichModels;

public class Session: IReadOnlySession, ISavableEntity
{
    protected IUnitOfWork UnitOfWork { get; }
    
    public Session(string token, SessionPayload payload, DateTimeOffset expiresAt, IUnitOfWork unitOfWork)
    {
        Token = token;
        Payload = (SessionPayload) payload.Clone();
        ExpiresAt = expiresAt;
        UnitOfWork = unitOfWork;
    }
    
    public string Token { get; }
    public SessionPayload Payload { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    
    public Task PublishChangesAsync(CancellationToken cancellationToken = default)
        => UnitOfWork.SessionRepository.SaveSessionAsyncAsync(this, cancellationToken);
}