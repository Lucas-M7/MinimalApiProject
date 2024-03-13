using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal_Api_Project.Dominio.Interfaces;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestutura.Db;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Minimal_Api_Project.Dominio.ModelViews;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "1234567";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o TokenJWT aqui"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administradores
string GerarTokenJwt(Administrador administrador)
{
    if (string.IsNullOrEmpty(key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil)
        
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    var adm = administradorServico.Login(loginDTO);

    if (adm != null)
    {
        string token = GerarTokenJwt(adm);

        return Results.Ok(new AdmLogado
        {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    else
        return Results.Unauthorized();
}).AllowAnonymous().WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    var validacao = new ErrosDeValidacao()
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("Email inválido.");

    if (string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("Senha vazia.");

    if (administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil inválido.");

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var administrador = new Administrador()
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
    };

    administradorServico.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}", new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
    var adms = new List<AdministradorModelView>();
    var administradores = administradorServico.Todos(pagina);
    foreach (var adm in administradores)
    {
        adms.Add(new AdministradorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }

    return Results.Ok(adms);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administradores");

app.MapGet("/Administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscaPorId(id);

    if (administrador == null)
        return Results.NotFound();

    return Results.Ok(new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administradores");
#endregion

#region Veiculos
static ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("Nome inválido.");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("Marca inválida.");

    if (veiculoDTO.Ano <= 1950)
        validacao.Mensagens.Add("Veículo muito antigo. Somentes veículos superiores a 1950 são aceitos.");

    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var validacao = validaDTO(veiculoDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);


    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };

    veiculosServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" })
.WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" })
.WithTags("Veículos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);

    if (veiculo == null)
        return Results.NotFound();

    return Results.Ok(veiculo);
}).RequireAuthorization().WithTags("Veículos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculosServico.Atualizar(veiculo);

    return Results.Ok(veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Veículos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);

    if (veiculo == null)
        return Results.NotFound();

    veiculosServico.Apagar(veiculo);

    return Results.NoContent();
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Veículos");
#endregion

#region  App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion