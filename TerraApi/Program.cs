using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TerraApi.Controllers;
using TerraApi.Dao.Usuario;
using TerraApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Accede a la cadena de conexión desde las variables de entorno
var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

// Agrega servicios de controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura DatabaseHelper como un servicio
builder.Services.AddSingleton<DatabaseHelper>(provider =>
{
    return new DatabaseHelper(connectionString);
});

// Registro del DAO y controlador
builder.Services.AddScoped<UsuarioDao>(provider =>
{
    return new UsuarioDao(connectionString, provider.GetRequiredService<DatabaseHelper>());
});

builder.Services.AddScoped<AuthController>();

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configuración de Swagger y autenticación en el pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
