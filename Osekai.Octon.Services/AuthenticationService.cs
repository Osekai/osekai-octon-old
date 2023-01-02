using Osekai.Octon.Database;
using Osekai.Octon.Database.HelperTypes;
using Osekai.Octon.Exceptions;
using Osekai.Octon.Database.Dtos;
using Osekai.Octon.OsuApi;
using Osekai.Octon.OsuApi.Payloads;

namespace Osekai.Octon.Services;

public class AuthenticationService
{
    private readonly IDatabaseUnitOfWorkFactory _databaseUnitOfWorkFactory;
    private readonly OsuApiV2Interface _osuApiV2Interface;
    private readonly ITokenGenerator _tokenGenerator;
    
    public AuthenticationService(
        IDatabaseUnitOfWorkFactory databaseUnitOfWorkFactory,
        ITokenGenerator tokenGenerator,
        OsuApiV2Interface osuApiV2Interface)
    {
        _databaseUnitOfWorkFactory = databaseUnitOfWorkFactory;
        _osuApiV2Interface = osuApiV2Interface; 
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<SessionDto> LogInWithTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        await using IDatabaseTransactionalUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateTransactionalAsync();
        
        SessionDto? session = await unitOfWork.SessionRepository.GetSessionFromTokenAsync(token, cancellationToken);

        if (session == null)
            throw new InvalidSessionTokenException(token);
        
        await unitOfWork.CommitAsync(cancellationToken);
        return session;
    }

    public async Task<SessionDto> SignInWithCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        (OsuAuthenticationResultPayload payload, OsuUser user, DateTimeOffset responseDateTime) = await _osuApiV2Interface.AuthenticateWithCodeAsync(code, cancellationToken);

        if (user.IsDeleted)
            throw new OsuUserDeletedException(user.Id);

        if (user.IsDeleted)
            throw new OsuUserRestrictedException(user.Id);
        
        await using IDatabaseTransactionalUnitOfWork unitOfWork = await _databaseUnitOfWorkFactory.CreateTransactionalAsync();

        string generatedToken;
        do
        {
            generatedToken = _tokenGenerator.GenerateToken();
        } while (await unitOfWork.SessionRepository.SessionExists(generatedToken, cancellationToken));

        SessionDto session = await unitOfWork.SessionRepository.AddSessionAsync(
            generatedToken,
            new SessionDtoPayload(payload.Token, payload.RefreshToken,
                responseDateTime.AddSeconds(payload.ExpiresIn).UtcDateTime, user.Id), 
            DateTimeOffset.Now.AddSeconds(Specifications.SessionTokenMaxAgeInSeconds));
        
        await unitOfWork.SaveAsync(cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return session;
    }
}