###Projeto fujiyama (loja de produtos japonês).


## nomes: Davi Okano e Gustavo Toyota


explicação da models: 

A ideia era simular algumas pequenas funcionalidades que uma loja online de produtos japoneses poderia ter, para isso, ultilizamos as classes:
cliente, categoria, forncedor, produto, pessoa funcionário, entre outras. No geral ambas apresentavam a mesma estrutura, ex: 

using System.ComponentModel.DataAnnotations;
```
namespace projetoFuji.Models
{
    public class Produto => classe pública
    {
        [Required] // campo obrigatório
        [Display(Name = "Codigo de barras")]  // Nome exibido no formulário será "Codigo de barras"
        [StringLength(13, MinimumLength =13)] // Deve ter exatamente 13 caracteres
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "O código de barras deve conter apenas números.")]  // Regex garante que só números são aceitos
        public string? Codigo_de_barras { get; set; } // Declara o tipo do Codigo_de_barras, além dos métodos get e set, get: devolve o valor da propriedade, set: atribui um valor
    }
}
```
No geral, todas às classes apresentavam a mesma lógica, mas diferentes de acordo com as suas necessidades


explicação controllers: 
