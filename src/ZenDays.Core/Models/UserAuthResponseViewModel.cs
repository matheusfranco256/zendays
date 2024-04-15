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
			public string TipoUsuario { get; set; }
			public string IdDepartamento { get; set; }

			public string CreatedAt { get; }
			public string ExpiresAt { get; }

			public string Token { get; } = null!;

			public UserAccessTokenModel(string id, string email, string tipoUsuario, string idDepartamento, DateTime createdAt, DateTime expiresAt, string token)
			{
				Id = id;
				Email = email;
				TipoUsuario = tipoUsuario;
				IdDepartamento = idDepartamento;
				CreatedAt = createdAt.ToString("yyyy-MM-ddHH:mm:ss");
				ExpiresAt = expiresAt.ToString("yyyy-MM-ddHH:mm:ss");
				Token = token;
			}
		}
	}
}
