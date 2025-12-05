using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class ItemProduto
    {
        [Required]

        public Produto Produto { get; set; } = new Produto();

        [Required]
        public Venda Venda { get; set; } = new Venda();

        [Required]
        [Range(0,32767)]
        public int Qtd {  get; set; }

        [Required]
        [Display(Name = "Preço")]
        [StringLength(9)]
        [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "O preço deve ser numérico (use ponto para decimais).")]
        public string? Preco { get; set; }
    }
}
