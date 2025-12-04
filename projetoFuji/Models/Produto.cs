using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Produto
    {
        [Required]
        [Display(Name = "Codigo de barras")]
        [StringLength(13, MinimumLength =13)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "O código de barras deve conter apenas números.")]
        public string? Codigo_de_barras { get; set; }

        [Required]
        [Display(Name = "Preço")]
        [StringLength(9)]
        [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "O preço deve ser numérico (use ponto para decimais).")]
        public string? Preco { get; set; }

        [Required]
        [Display(Name = "Custo")]
        [StringLength(9)]
        [RegularExpression(@"^[0-9]+([.][0-9]+)?$", ErrorMessage = "O custo deve ser numérico (use  ponto para decimais).")]
        public string? Custo { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(150)]

        public string? Nome { get; set; }

        [StringLength(50)]
        [Display(Name = "País de origem")]
        public string? Pais_de_origem { get; set; }

        [Display(Name = "Data de Validade")]
        public DateOnly? Data_de_validade { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Quantidade")]
        public int? Qtd { get; set; }

        [Required]
        [Display(Name = "Código do fornecedor")]
        public string? Fornecedor { get; set; }
    }
}