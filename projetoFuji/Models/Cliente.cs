using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Cliente
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números

        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        public DateTime DataPrimeiraCompra {  get; set; }
        [StringLength(50)]
        [Required]
        public string? Senha {  get; set; }
    }
}
