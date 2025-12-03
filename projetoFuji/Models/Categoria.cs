using System.ComponentModel.DataAnnotations;

namespace projetoFuji.Models
{
    public class Categoria
    {
        [Required]
        [Display(Name = "Id")]
        public int? ID { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }
    }
}