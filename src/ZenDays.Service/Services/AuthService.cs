using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ZenDays.Core.Messages;
using ZenDays.Core.Models;
using ZenDays.Core.Options;
using ZenDays.Core.Utilities;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;
using ZenDays.Service.Interfaces;
using static ZenDays.Core.Models.UserAuthResponseViewModel;

namespace ZenDays.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenOptions _tokenConfig;
        private readonly IUserRepository _userRepository;
        public AuthService(IOptions<TokenOptions> tokenConfig, IUserRepository userRepository)
        {
            _tokenConfig = tokenConfig.Value;
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel> AuthenticateUser(LoginViewModel model)
        {
            var usuario = await _userRepository.GetByEmail(model.Email);

            if (usuario is null || !HashHelper.PasswordCompare(@usuario.Senha, model.Senha))
            {
                return new ResultViewModel(null, 401, false, "Sem Autorização");
            }

            return new ResultViewModel(await GenerateToken(usuario), 200, true, "Autorizado");
        }



        private async Task<UserAccessTokenModel> GenerateToken(Usuario usuario)
        {
            try
            {
                var request = new
                {
                    email = usuario.Email,
                    password = usuario.Senha,
                    returnSecureToken = true
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyCk8LcATWY9nDxQKkuExNV91eB3v37i-Bk", content);
                    var responseJson = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var claims = new List<Claim>();

                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
                        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email));
                        claims.Add(new Claim("id", usuario.Id.ToString()));
                        ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(usuario.Email, "Login"), claims);

                        DateTime dtCreation = DateTime.UtcNow;
                        DateTime dtExpiration = dtCreation + (TimeSpan.FromSeconds(_tokenConfig.Seconds != 0 ? _tokenConfig.Seconds : 432000));

                        if (string.IsNullOrEmpty(_tokenConfig.Key))
                            throw new Exception("Secure key for JWT token wasn't provided.");

                        var key = Encoding.ASCII.GetBytes(_tokenConfig.Key);
                        var handler = new JwtSecurityTokenHandler();

                        var securityToken = handler.CreateToken(new()
                        {
                            Issuer = _tokenConfig.Issuer,
                            Audience = _tokenConfig.Audience,
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                            Subject = identity,
                            NotBefore = dtCreation,
                            Expires = dtExpiration
                        });

                        var token = handler.WriteToken(securityToken);
                        return await Task.FromResult(new UserAccessTokenModel(usuario.Id, usuario.Email, claims, dtCreation, dtExpiration, token));
                    }
                    else
                    {
                        throw new Exception("Falha na autenticação");
                    }
                }
            }
            catch (Exception ex)
            {
                // Trate o erro adequadamente
                throw new Exception(ex.Message);
            }

        }

        public async Task<ResultViewModel> ResetPassword(string email, string senhaAntiga, string novaSenha)
        {
            var usuario = await _userRepository.GetByEmail(email);
            if (usuario is not null)
            {
                if (HashHelper.PasswordCompare(usuario.Senha, senhaAntiga))
                {
                    usuario.Senha = HashHelper.HashGeneration(novaSenha);
                    var json = JsonConvert.SerializeObject(usuario);
                    var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (atualizaUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
                    await _userRepository.Update(atualizaUsuario, usuario.Id);
                    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                    var getUserAuth = await auth.GetUserByEmailAsync(email);
                    await auth.UpdateUserAsync(new UserRecordArgs
                    {
                        Uid = getUserAuth.Uid,
                        Email = email,
                        Password = usuario.Senha
                    });
                    return new ResultViewModel(usuario, 200, true, "Senha Atualizada com sucesso");
                }
            }

            return new ResultViewModel(null, 404, false, "Usuario não encontrado");
        }
    }
}
