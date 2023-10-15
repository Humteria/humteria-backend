
using Data.DTOs.UserDTO;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Humteria.Helpers;

public class JwtTokens
{
    private static readonly IConfigurationRoot _configuration;
    private static readonly string _TokenSecret;
    private static readonly string _Audience;
    private static readonly string _Issuer;


    //TODO: Check if Static or should be changed to singleton
    static JwtTokens()
    {
        _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
        string? tempTokenHolder = _configuration.GetValue<string>("JWT:TokenSecret");
        string? tempAudienceHolder = _configuration.GetValue<string>("JWT:Audience");
        string? tempIssuerHolder = _configuration.GetValue<string>("JWT:Issuer");
        if (tempTokenHolder == null || tempAudienceHolder == null || tempIssuerHolder == null)
        { 
            throw new ArgumentNullException();
        } else { 
            _TokenSecret = tempTokenHolder; 
            _Audience = tempAudienceHolder; 
            _Issuer = tempIssuerHolder; 
        }
    }

    public static string GenerateToken(JWTUserForTokenDTO user, int daysToExpire = 5)
    {
        var key = Encoding.UTF8.GetBytes(_TokenSecret);
        var userData = JsonConvert.SerializeObject(user);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, userData), }),
            Expires = DateTime.UtcNow.AddDays(daysToExpire),
            Issuer = _Issuer,
            Audience = _Audience,
            SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
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

            var userString = decodedToken.Claims.Where(x => x.Type == ClaimTypes.UserData).FirstOrDefault();
            if (userString != null)
            {
                return Guid.Empty;
            }

            JWTUserForTokenDTO? user = JsonConvert.DeserializeObject<JWTUserForTokenDTO>(userString.Value);
            if(user != null)
            {
                return Guid.Empty;
            }
            return user.Id;
        }
        catch (Exception) 
        {
            return Guid.Empty; ;
        }
    }
}
