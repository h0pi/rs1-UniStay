using Microsoft.AspNetCore.Authentication;
using System.Text.Json.Serialization;
using UniStay.API.Data;
using UniStay.API.Data.Models;
using UniStay.API.Helper;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Services;

public class MyAuthServiceHelper
//(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, MyTokenGenerator myTokenGenerator)
{

    // Generisanje novog tokena za korisnika
    public static async Task<MyAuthenticationToken> GenerateSaveAuthToken(string? IpAddress, ApplicationDbContext applicationDbContext, Users user, CancellationToken cancellationToken = default)
    {
        string randomToken = MyTokenGenerator.Generate(10);

        var authToken = new MyAuthenticationToken
        {
            IpAddress = IpAddress ?? string.Empty,
            Value = randomToken,
            User = user,
            RecordedAt = DateTime.Now
        };

        applicationDbContext.Add(authToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return authToken;
    }

    // Uklanjanje tokena iz baze podataka
    public static async Task<bool> RevokeAuthToken(ApplicationDbContext applicationDbContext, string tokenValue, CancellationToken cancellationToken = default)
    {
        var authToken = await applicationDbContext.MyAuthenticationTokens.FirstOrDefaultAsync(t => t.Value == tokenValue, cancellationToken);


        if (authToken == null)
            return false;

        applicationDbContext.Remove(authToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Dohvatanje informacija o autentifikaciji korisnika
    public static MyAuthInfo GetAuthInfoFromTokenString(ApplicationDbContext applicationDbContext, string? authToken)
    {
        if (string.IsNullOrEmpty(authToken))
        {
            return GetAuthInfoFromTokenModel(null);
        }

        MyAuthenticationToken? myAuthToken = applicationDbContext.MyAuthenticationTokens
            .IgnoreQueryFilters()
            .Include(x => x.User)
            .SingleOrDefault(x => x.Value == authToken);

        return GetAuthInfoFromTokenModel(myAuthToken);
    }


    // Dohvatanje informacija o autentifikaciji korisnika
    public static MyAuthInfo GetAuthInfoFromRequest(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
    {
        string? authToken = httpContextAccessor.HttpContext?.Request.Headers["my-auth-token"];
        return GetAuthInfoFromTokenString(applicationDbContext, authToken);
    }

    public static MyAuthInfo GetAuthInfoFromTokenModel(MyAuthenticationToken? myAuthToken)
    {
        if (myAuthToken == null)
        {
            return new MyAuthInfo
            {
                IsAdmin = false,
                IsStudent = false,
                IsEmployee=false,
                IsLoggedIn = false,
            };
        }

        return new MyAuthInfo
        {
            UserId = myAuthToken.UserID,
            Email = myAuthToken.User!.Email,
            FirstName = myAuthToken.User.FirstName,
            LastName = myAuthToken.User.LastName,
            IsAdmin = myAuthToken.User.IsAdmin,
            IsStudent = myAuthToken.User.IsStudent,
            IsEmployee = myAuthToken.User.IsEmployee,
            IsLoggedIn = true,
        };
    }
}

// DTO to hold authentication information
public class MyAuthInfo
{
    public int MyAuthID { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsEmployee { get; set; }
    public bool IsLoggedIn { get; set; }

}
