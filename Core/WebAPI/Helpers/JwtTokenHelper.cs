using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Humteria.WebAPI.Helpers;

public interface IJwtGenerator
{
    public string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5);
    public Guid ValidateToken(string token);
}

public class JwtTokenHelper : IJwtGenerator
{
    private readonly string m_tokenSecret;
    private readonly string m_issuer;
    private readonly string m_audience;

    public JwtTokenHelper(IConfiguration configuration)
    {
        IConfigurationSection jwt = configuration.GetRequiredSection("JWT");

        m_tokenSecret = jwt.GetRequiredSection("TokenSecret").Get<string>()!;
        m_issuer = jwt.GetRequiredSection("Issuer").Get<string>()!;
        m_audience = jwt.GetRequiredSection("Audience").Get<string>()!;
    }    

    public string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5)
    {
        byte[] key = Encoding.UTF8.GetBytes(m_tokenSecret);
        string userData = JsonConvert.SerializeObject(user);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, userData) }),
            Expires = DateTime.UtcNow.AddDays(daysToExpire),
            Issuer = m_issuer,
            Audience = m_audience,
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
            byte[] key = Encoding.UTF8.GetBytes(m_tokenSecret);

            ClaimsPrincipal decodedToken = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = m_issuer,
                ValidAudience = m_audience,
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

