using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Auth;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Auth
{
    public interface ITokenService
    {
        string GenerateJwtToken(AuthResponse user);
    }

    public class TokenService : ITokenService
    {
        private readonly AuthSettings _authSettings;
        private readonly byte[] _secret;
        private readonly int _timeout;

        public TokenService(AuthSettings authSettings)
        {
            _authSettings = authSettings;
            _secret = Encoding.ASCII.GetBytes(_authSettings.Secret);
            _timeout = _authSettings.TokenTimeout;
        }

        // From: https://dev.to/moe23/asp-net-core-5-rest-api-authentication-with-jwt-step-by-step-140d
        public string GenerateJwtToken(AuthResponse user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(_timeout),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
