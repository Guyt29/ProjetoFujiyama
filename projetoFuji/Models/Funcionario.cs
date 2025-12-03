using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoFuji.Models
{
    public class Funcionario
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }


        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números
        [Display(Name = "Supervisor (CPF)")]
        public string? Supervisor { get; set; } //FK Para cpf (self)



        [Required]
        [StringLength(75)]
        [Display(Name = "Função")]
        public string? Funcao { get; set; }


        [Required]
        [Display(Name = "Salário")]
        public decimal Salario { get; set; }


        [Required]
        [Display(Name = "Data de admissão")]
        [DataType(DataType.Date)] // para mostrar só a data
        public DateTime DataDeAdmissao { get; set; }


        [Display(Name = "Data de demissão")]
        [DataType(DataType.Date)]

        public DateTime? DataDemissao { get; set; }

    }
}
