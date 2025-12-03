using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Fornecedor
    {
        [Required]
        [StringLength(14, MinimumLength = 14)]
        [Display(Name = "CNPJ")]
        public string? CNPJ { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome do Fornecedor")]
        public string? Nome { get; set; }
        [StringLength(200)]

        [Display(Name = "Endereco do Produto")]
        public string? Endereco { get; set; }

        [Display(Name = "Telefone do fornecedor")]
        [StringLength(11, MinimumLength = 11)]
        public string? Telefone { get; set; }

        [StringLength(200)]
        [Display(Name ="Email do fornecedor")]
        public string? Email { get; set; }

    }
}