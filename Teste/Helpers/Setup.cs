using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Infraestutura.Db;

namespace Api.Teste.Helpers;

public class Setup
{
    public const string PORT = "5001";
    public static TestContext testContext = default!;
    public static HttpClient client = default!;

    
}