using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Pokušava naći NameIdentifier, ako ne uspije, traži obični "id"
        var user = connection.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? user?.FindFirst("id")?.Value
               ?? user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    }
}