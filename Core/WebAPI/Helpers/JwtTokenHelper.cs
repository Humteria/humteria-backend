
using Humteria.Application.DTOs.UserDTO;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Humteria.WebAPI.Helpers;

public class JwtTokenHelper
{
    private static readonly IConfiguration _configuration;
    private static readonly string _TokenSecret;
    private static readonly string _Audience;
    private static readonly string _Issuer;


    //TODO: Check if Static or should be changed to singleton
    static JwtTokenHelper()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();

        _TokenSecret = _configuration.GetValue<string>("JWT:TokenSecret");
        _Audience = _configuration.GetValue<string>("JWT:Audience");
        _Issuer = _configuration.GetValue<string>("JWT:Issuer");

        if (_TokenSecret == null || _Audience == null || _Issuer == null)
        {
            throw new ArgumentNullException("JWT configuration values are missing.");
        }
    }

    public static string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5)
    {
        var key = Encoding.UTF8.GetBytes(_TokenSecret);
        var userData = JsonConvert.SerializeObject(user);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, userData) }),
            Expires = DateTime.UtcNow.AddDays(daysToExpire),
            Issuer = _Issuer,
            Audience = _Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
    public static Guid ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_TokenSecret);
            var decodedToken = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _Issuer,
                ValidAudience = _Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var userString = decodedToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;

            if (userString != null)
            {
                return Guid.Empty;
            }

            JWTUserForTokenDTO user = JsonConvert.DeserializeObject<JWTUserForTokenDTO>(userString);

            if (user != null)
            {
                return Guid.Empty;
            }

            return user.Id;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
}
