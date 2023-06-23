using Microsoft.Extensions.DependencyInjection;
using ZenDays.Infra.Interfaces;
using ZenDays.Infra.Repositories;
using ZenDays.Service.Interfaces;
using ZenDays.Service.Services;

namespace ZenDays.IOC
{
    public static class Injection
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services)
        {
            //Departamento
            services.AddScoped(typeof(IDepartamentoRepository), typeof(DepartamentoRepository));
            services.AddScoped(typeof(IDepartamentoService), typeof(DepartamentoService));
            //Usuario
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            //Ferias
            services.AddScoped(typeof(IFeriasRepository), typeof(FeriasRepository));
            services.AddScoped(typeof(IFeriasService), typeof(FeriasService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            return services;
        }

    }
}