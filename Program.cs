using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.DTOs;
using MinimalApi.Infraestutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "1234567")
        return Results.Ok("Login feito com sucesso!");
    else
        return Results.Unauthorized();
});

app.Run();