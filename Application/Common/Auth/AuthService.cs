

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Caching;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

public class AuthService(IUserRepository _userRepo,
                            IConfiguration _conf,
                            ICacheService _redis) : IAuthService
{
    public async Task<Dictionary<string, string>> RegisterAsync(NewUserDTO nuDTO, Position position = Position.Student)
    {
        User user = UserMapper.DTOToUser(nuDTO);
        PasswordHasher<User> passwordHasher = new();
        user.PasswordHash = passwordHasher.HashPassword(user, nuDTO.Password == String.Empty ? nuDTO.Username : nuDTO.Password);
        user.Position = position;
        try
        {
            await _userRepo.AddUserAsync(user);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pgr)
        {
            if (pgr.ConstraintName == "IX_Users_Username")
            {
                throw new UserAlreadyExistsException("Username already exists.");
            }
            throw;
        }
        return await GenerateTokensAsync(user);

    }

    ClaimsIdentity CreateClaims(User user)
    {
        ClaimsIdentity claims = new();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Username!));
        claims.AddClaim(new Claim(ClaimTypes.Role, user.Position.ToString()!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return claims;
    }

    async Task<Dictionary<string, string>> GenerateTokensAsync(User user, bool expired = false)
    {
        Dictionary<string, string> tokens = [];
        JwtSecurityTokenHandler handler = new();
        byte[] key = Encoding.ASCII.GetBytes(_conf["Security:SecretKey"]!);
        SigningCredentials credentials = new
            (
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature
            );
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = CreateClaims(user),
            Issuer = _conf["API:Issuer"],
            Audience = _conf["API:Audience"],
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(expired ? TimeSpan.FromSeconds(0) : TimeSpan.FromMinutes(Int32.Parse(_conf["Security:AccessTokenExpirySpanMinutes"]!))),
            SigningCredentials = credentials
        };
        tokens.Add("AccessToken", handler.WriteToken(handler.CreateToken(tokenDescriptor)));
        byte[] randStringToken = new byte[128];
        RandomNumberGenerator.Create().GetBytes(randStringToken);
        var plainRefreshToken = Convert.ToBase64String(randStringToken)!;
        tokens.Add("RefreshToken", plainRefreshToken);
        var hasher = SHA512.Create();

        await _redis.SetAsync(Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(plainRefreshToken))!),
        user.Id.ToString(), TimeSpan.FromDays(Int32.Parse(_conf["Security:RefreshTokenExpirySpanDays"]!)));
        return tokens;
    }

    public async Task<Dictionary<string, string>> LoginAsync(LoginFormDTO lfDTO)
    {
        User? user = await _userRepo.GetUserByUsernameAsync(lfDTO.Username!)
            ?? throw new UserDoesntExistException("There's no such user");
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, lfDTO.Password!)
        == PasswordVerificationResult.Failed) throw new PasswordCheckFailedException("Password check failed.");
        return await GenerateTokensAsync(user);
    }



    public async Task<Dictionary<string, string>> AttemptRefreshAsync(string refreshToken)
    {
        var hasher = SHA512.Create();
        var token = Convert.ToBase64String(hasher.ComputeHash(
                    Encoding.ASCII.GetBytes(refreshToken)));
        string? Guid = await _redis.GetAsync(token)
            ?? throw new RefreshFailedException("Given refresh token is not valid");
        User? user = await _userRepo.GetUserByGuidAsync(Guid);
        return await GenerateTokensAsync(user!, true);
    }

    public async Task<User?> AuthenticateUserAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var claims = jwtSecurityToken.Claims;
        var UserId = claims.First(claim => claim.Type == "nameid").Value;
        return await _userRepo.GetUserByGuidAsync(UserId);
    }

    public async Task ClearTokensAsync(string accessToken, string refreshToken)
    {
        await _redis.RemoveAsync(refreshToken);
        var KeyName = $"Revoked_{accessToken}";
        await _redis.SetAsync(KeyName,
                                    accessToken,
                                    TimeSpan.FromMinutes(Int32.Parse(_conf["Security:AccessTokenExpirySpanMinutes"]!)));
    }



    public async Task<string[]?> GetAdministratorAsync()
    {
        if (await _userRepo.IsUsernameTaken("administrator")) return null;
        var Password = Guid.NewGuid().ToString();
        await RegisterAsync(new NewUserDTO()
        {
            FirstName = "Dean",
            LastName = "",
            Username = "administrator",
            Password = Password
        }, Position.Dean);
        return ["administrator", Password];
    }
}