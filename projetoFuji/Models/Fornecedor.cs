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
        [len]
        [Display(Name = "Endereco do Produto")]
        public string? Endereco { get; set; }
        [Range(00000000000, 99999999999)]
        [Display(Name = "Telefone do fornecedor")]
        public int? Telefone { get; set; }
        [Display(Name ="Email do fornecedor")]
        public string? Email { get; set; }

    }
}