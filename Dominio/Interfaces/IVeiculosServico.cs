using MinimalApi.Dominio.Entidades;

namespace Minimal_Api_Project.Dominio.Interfaces;
    public interface IVeiculosServico
    {
        List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);

        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }