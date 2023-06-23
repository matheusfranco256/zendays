using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ZenDays.Core.Options;

namespace ZenDays.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenOptions _tokenOptions;

        public JwtMiddleware(RequestDelegate next, IOptions<TokenOptions> tokenOptions)
        {
            this._next = next;
            this._tokenOptions = tokenOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var teste = context.Request.Headers;
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(this._tokenOptions.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this._tokenOptions.Issuer,
                    ValidAudience = this._tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);


                var jwtToken = (JwtSecurityToken)validatedToken;

                var id = jwtToken.Claims.First(x => x.Type == "id").Value;
                var email = jwtToken.Claims.First(x => x.Type == "unique_name").Value;



                context.Items["email"] = email;
                context.Items["id"] = id;

                await Task.CompletedTask;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
