using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Pessoa
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números

        [Display(Name ="CPF")]
        public string? Cpf { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Nome")]

        public string? Nome { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Email")]

        public string? Email { get; set; }

        [Required]
        [Display(Name = "Genêro")]
        [StringLength(1, MinimumLength = 1)]
        public string? Genero {get; set; }

        [Required]
        [Range(0, 120)]
        [Display(Name = "Idade")]
        public int Idade {  get; set; }

        [Display(Name = "Telefone")]
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo deve conter apenas números.")] // regex toda a string deve ser composta apenas de números

        public string? Telefone {  get; set; }

    }
}
