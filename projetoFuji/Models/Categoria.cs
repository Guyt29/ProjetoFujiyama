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
        [StringLength(200)]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(100)]
        public string? Nome { get; set; }
    }
}