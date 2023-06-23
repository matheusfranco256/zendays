using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using ZenDays.API.Configurations;
using ZenDays.API.Middlewares;
using ZenDays.IOC;
using ZenDays.Service.DTO.Config;

var builder = WebApplication.CreateBuilder(args);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("C:\\Users\\Matheus\\Source\\ZenDays\\zendays.json"),
});
// Add services to the container.

builder.Services.InjectDependencies();
#region AutoMapper
var mapConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region JWT
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration();
builder.Services.AddSwaggerConfiguration();
#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());


app.UseMiddleware<JwtMiddleware>();

app.UseMiddleware(typeof(HandlingMiddleware));

app.UseAuthorization();

app.MapControllers();

app.Run();
