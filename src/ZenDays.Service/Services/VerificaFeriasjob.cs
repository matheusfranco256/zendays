using Newtonsoft.Json;
using Quartz;
using System.Globalization;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Service.Services
{

    public class VerificaFeriasjob : IJob
    {
        private readonly IUserRepository _userRepository;

        public VerificaFeriasjob(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataAtual = DateTime.Now;
            Console.WriteLine("Executou: " + dataAtual);

            var usuarios = await _userRepository.GetAll();


            foreach (var usuario in usuarios)
            {
                if (DateTime.TryParseExact(usuario.FinalPeriodoAquisitivo, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime finalPeriodoAquisitivo))
                {
                    if (dataAtual >= finalPeriodoAquisitivo)
                    {

                        int diasASomar = 31;
                        usuario.SaldoFerias += diasASomar;

                        usuario.FinalPeriodoAquisitivo = dataAtual.AddYears(1).ToString("dd/MM/yyyy");



                        Console.WriteLine($"Usuário {usuario.Nome} - Saldo de férias atualizado para {usuario.SaldoFerias} dias.");
                    }
                    usuario.UltimaVerificacao = dataAtual.ToString("dd/MM/yyyy");

                    var json = JsonConvert.SerializeObject(usuario);
                    var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (atualizaUsuario is not null) await _userRepository.Update(atualizaUsuario, usuario.Id);
                }
                else
                {

                    Console.WriteLine($"Erro: Data inválida para o usuário {usuario.Nome}");
                }
            }

        }
    }
}
