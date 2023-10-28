using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Humteria.Application.DTOs.UserDTO;

namespace Humteria.WebAPI.Helpers;

public interface IJwtGenerator
{
    public string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5);
    public Guid ValidateToken(string token);
}

public class JwtTokenHelper : IJwtGenerator
{
    private readonly string _tokenSecret;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenHelper(IConfiguration configuration)
    {
        IConfigurationSection jwt = configuration.GetRequiredSection("JWT");

        _tokenSecret = jwt.GetRequiredSection("TokenSecret").Get<string>()!;
        _issuer = jwt.GetRequiredSection("Issuer").Get<string>()!;
        _audience = jwt.GetRequiredSection("Audience").Get<string>()!;
    }    

    public string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5)
    {
        byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);
        string userData = JsonConvert.SerializeObject(user);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, userData) }),
            Expires = DateTime.UtcNow.AddDays(daysToExpire),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
       
        return tokenHandler.WriteToken(token);
    }

    public Guid ValidateToken(string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

            ClaimsPrincipal decodedToken = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            Claim? userClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);

            if (userClaim == null)
                return Guid.Empty;

            JWTUserForTokenDTO? user = JsonConvert.DeserializeObject<JWTUserForTokenDTO>(userClaim.Value);

            return user?.Id ?? Guid.Empty;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
}

