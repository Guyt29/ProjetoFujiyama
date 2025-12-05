### Projeto fujiyama (loja de produtos japonês).


## nomes: Davi Okano e Gustavo Toyota


## explicação da models: 

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

As models *ProdutoCategoria e ItemProduto* servme como uma entidade associativa, associando duas tabelas: Produto e Categoria e Produto e Venda
As models *CadastroFuncionarioViewModel e ClientePessoa* servem para auxiliar na hora de criar uma view para seu cadastro.


## explicação controllers: 

No geral, todas as controllers possuim pelo menos 4 funcionalidades básicas: Cadastrar, Listar, Editar e Deletar.

# Cadastrar
Exemplo da categoria
```
        [HttpPost]
        public IActionResult Cadastrar(Categoria categoria)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString); // cria o objeto de conexão com o banco passando a string de conexão
            connection.Open(); // abre a conexão

            string sql = @"INSERT INTO tbCategoria
                           VALUES (@Id, @Descricao, @Nome)"; // query sql

            MySqlCommand command = new MySqlCommand(sql, connection); // cria o objeto do comando passando a query e a conexão
            command.Parameters.AddWithValue("@Id", categoria.ID); //adiciona os parametros
            command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
            command.Parameters.AddWithValue("@Nome", categoria.Nome);

            command.ExecuteNonQuery(); //executa

            return RedirectToAction("Listar", "Categoria"); //volta pra lista
        }

```
