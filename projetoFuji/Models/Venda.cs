using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Venda
    {
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "Nota fiscal")]
        public string? Nf { get; set; }

        [Required]
        [Display(Name = "Data e Hora da Compra")]

        public DateTime DataHora { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "CPF do Funcionário")]
        public string? Funcionario { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "CPF do Cliente")]
        public string? Cliente { get; set; }

        public decimal TotalVenda { get; set; }
    }
}
