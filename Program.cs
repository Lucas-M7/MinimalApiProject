using MinimalApi.DTOs;

var builder = WebApplication.CreateBuilder(args);
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