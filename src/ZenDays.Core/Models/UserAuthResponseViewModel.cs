using System.Security.Claims;

namespace ZenDays.Core.Models
{
    public class UserAuthResponseViewModel
    {
        UserAccessTokenModel? AccessToken { get; }

        public UserAuthResponseViewModel(UserAccessTokenModel? accessToken)
        {
            AccessToken = accessToken;
        }

        public class UserAccessTokenModel
        {
            public string Id { get; set; }
            public string Email { get; set; } = null!;
            public IEnumerable<Claim> Claims { get; }

            public string CreatedAt { get; }
            public string ExpiresAt { get; }

            public string Token { get; } = null!;

            public UserAccessTokenModel(string id, string email, IEnumerable<Claim> claims, DateTime createdAt, DateTime expiresAt, string token)
            {
                Id = id;
                Email = email;
                Claims = claims;
                CreatedAt = createdAt.ToString("yyyy-MM-ddHH:mm:ss");
                ExpiresAt = expiresAt.ToString("yyyy-MM-ddHH:mm:ss");
                Token = token;
            }
        }
    }
}
