using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using ZenDays.API.Configurations;
using ZenDays.API.Middlewares;
using ZenDays.IOC;
using ZenDays.Service.DTO.Config;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("*", build => build.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();



string webRootPath = app.Environment.WebRootPath;

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(webRootPath, $"{"zendays"}.json")),
});

app.UseSwagger();
app.UseSwaggerUI();


app.UseRouting();
app.UseCors("*");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware(typeof(HandlingMiddleware));



app.MapControllers();

app.Run();
