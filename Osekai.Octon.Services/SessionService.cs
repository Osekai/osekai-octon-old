using Osekai.Octon.Database;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.Services;

public class SessionService
{
    private readonly IDatabaseUnitOfWorkFactory _unitOfWorkFactory;
    private readonly OsuApiV2Interface _osuApiV2Interface;
    
    public SessionService(IDatabaseUnitOfWorkFactory unitOfWorkFactory, OsuApiV2Interface osuApiV2Interface)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _osuApiV2Interface = osuApiV2Interface;
    }
    
    public async Task RefreshSessionAsync(string token, CancellationToken cancellationToken)
    { 
        IDatabaseUnitOfWork unitOfWork = await _unitOfWorkFactory.CreateAsync();
        
        bool sessionExists = await unitOfWork.SessionRepository.UpdateExpirationDateTimeAsync(
            token, DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds), cancellationToken);

        if (!sessionExists)
            throw new InvalidSessionTokenException(token);
        
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<SessionDtoPayload> RefreshOsuSessionAsync(string token, CancellationToken cancellationToken)
    {
        await using IDatabaseUnitOfWork unitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken);

        SessionDto? session = await unitOfWork.SessionRepository.GetSessionFromTokenAsync(token);

        if (session == null)
            throw new InvalidSessionTokenException(token);

        OsuAuthenticationResultPayload result;
        OsuUser user;
        DateTimeOffset responseDateTime;
    
        try
        {
            (result, user, responseDateTime) =
                await _osuApiV2Interface.RefreshTokenAsync(session.Payload.OsuApiV2RefreshToken, cancellationToken);
        }
        catch
        {
            await unitOfWork.DisposeAsync();
            
            IDatabaseUnitOfWork recoveryUnitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken);
            await recoveryUnitOfWork.SessionRepository.DeleteSessionAsync(token, cancellationToken);
            
            throw;
        }

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);
        
        if (user.IsRestricted)
            throw new OsuUserRestrictedException(user.Id);
        
        SessionDtoPayload payload = session.Payload;

        payload.ExpiresAt = responseDateTime.UtcDateTime;
        payload.OsuApiV2RefreshToken = result.RefreshToken;
        payload.OsuApiV2Token = result.Token;

        await unitOfWork.SessionRepository.UpdateSessionPayloadAsync(session.Token, payload, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        
        return payload;
    }
}