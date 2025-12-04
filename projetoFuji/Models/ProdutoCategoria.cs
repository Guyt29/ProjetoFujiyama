using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class ProdutoCategoria
    {
        public Produto Produto { get; set; } = new Produto(); // recebe um objeto de pessoa para não retornar nulo
        public Categoria Categoria { get; set; } = new Categoria();

    }
}
