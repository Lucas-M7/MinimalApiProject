using MinimalApi.Dominio.Entidades;

namespace Teste.Domain.Entidades;


[TestClass]
public class VeiculoTeste
{
    [TestMethod]
    public void TestarPropriedadesVeiculos()
    {
        // Arrange
        var veiculo = new Veiculo();

        // Act
        veiculo.Id = 1;
        veiculo.Nome = "Fusca";
        veiculo.Marca = "Volkswagen";
        veiculo.Ano = 1999;


        // Assert
        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("Fusca", veiculo.Nome);
        Assert.AreEqual("Volkswagen", veiculo.Marca);
        Assert.AreEqual(1999, veiculo.Ano);
    }
}