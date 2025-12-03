using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Produto
    {
        [Required]
        [Display(Name = "Codigo de barras")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "O código de barras deve conter apenas números.")]
        public string? Codigo_de_barras { get; set; }

        [Required]
        [Display(Name = "Preço")]
        [RegularExpression(@"^[0-9]+([.,][0-9]+)?$", ErrorMessage = "O preço deve ser numérico (use vírgula ou ponto para decimais).")]
        public string? Preco { get; set; }

        [Required]
        [Display(Name = "Custo")]
        [RegularExpression(@"^[0-9]+([.,][0-9]+)?$", ErrorMessage = "O custo deve ser numérico (use vírgula ou ponto para decimais).")]
        public string? Custo { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Required]
        [Display(Name = "País de origem")]
        public string? Pais_de_origem { get; set; }

        [Required]
        [Display(Name = "Data de Validade")]
        public DateOnly? Data_de_validade { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Quantidade")]
        public int? Qtd { get; set; }

        [Required]
        [Display(Name = "Código do fornecedor")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "O código do fornecedor deve conter apenas números.")]
        public string? Fornecedor { get; set; }
    }
}