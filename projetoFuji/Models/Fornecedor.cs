using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Fornecedor
    {
        [Key]
        [Display(Name = "CNPJ")]
        public string? CNPJ { get; set; }
        [Required]
        [Display(Name = "Nome do Fornecedor")]
        public string? Nome { get; set; }
        [Display(Name = "Endereco do Produto")]
        public string? Endereco { get; set; }

        [Display(Name = "Telefone do fornecedor")]
        [Length(11, 11)]
        public string? Telefone { get; set; }
        [Display(Name ="Email do fornecedor")]
        public string? Email { get; set; }

    }
}