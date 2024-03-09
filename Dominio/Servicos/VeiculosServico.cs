using Microsoft.EntityFrameworkCore;
using Minimal_Api_Project.Dominio.Interfaces;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Infraestutura.Db;

namespace MinimalApi.Dominio.Servicos;

public class VeiculoServico : IVeiculosServico
{
    private readonly DbContexto _contexto;
    public VeiculoServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public void Apagar(Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
    }

    public void Atualizar(Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public Veiculo? BuscaPorId(int id)
    {
        return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Incluir(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.Veiculos.AsQueryable(); // Converter coleção de elemento em uma consulta LinQ

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome.ToLower()}%"));
        }

        int itemsPorPagina = 10;

        query = query.Skip((pagina - 1) * itemsPorPagina).Take(itemsPorPagina);
        return query.ToList();
    }
}