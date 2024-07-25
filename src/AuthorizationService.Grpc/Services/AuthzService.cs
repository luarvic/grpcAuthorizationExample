using Grpc.Core;
using AuthorizationService.Grpc;

namespace AuthorizationService.Grpc.Services;

public class AuthzService : Authorization.AuthorizationBase
{
    private readonly ILogger<AuthzService> _logger;
    public AuthzService(ILogger<AuthzService> logger)
    {
        _logger = logger;
    }

    public override Task<AuthorizationReply> Authorize(AuthorizationRequest request, ServerCallContext context)
    {
        return Task.FromResult(new AuthorizationReply
        {
            IsAuthorized = ValidateToken(request.Token)
        });
    }

    private static bool ValidateToken(string token)
    {
        // Implement your custom token validation logic here.
        return token.Contains("valid");
    }
}
