using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class FolhaDePagamento
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Salário")]
        public decimal Salario { get; set; }

        [Required]
        [Display(Name = "Valor de ajustes(bonûs reduções)")]
        public decimal ValorAjuste { get; set; }

        [Required]
        [DataType(DataType.Date)] // para mostrar só a data
        public DateTime DataPagamento {  get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        public decimal ValorPago { get; set; }
    }
}
