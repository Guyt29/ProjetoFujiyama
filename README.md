# Projeto fujiyama (loja de produtos japonês).


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

### Cadastrar
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
### Listar

```
public IActionResult Listar()
{
    List<Categoria> categoria = new List<Categoria>(); // cria uma lista do tipo Categoria
    using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"))) // cria uma conexão com o banco de dados MySQL usando a connection string DefaultConnection
    {
        conn.Open(); // abre a conexão
        string sql = "Select * from tbCategoria"; // query sql seleciona tudo da tabela categoria
        MySqlCommand command = new MySqlCommand(sql, conn); // prepara o comando
        MySqlDataAdapter adapter = new MySqlDataAdapter(command); // adaptador do Mysql para o c#
        DataTable dataTable = new DataTable(); //Cria uma datatable para receber os valores do banco
        adapter.Fill(dataTable); // Preenche a datatable com os registros do banco
        conn.Close(); // fecha a conexão

        foreach (DataRow row in dataTable.Rows) //
        {
            //adiciona a lista
            categoria.Add( 
                new Categoria // novo objeto
                {
                    ID = Convert.ToInt32(row["ID"]), //converte o valor do registro
                    Descricao = row["descricao"].ToString(),
                    Nome = row["nome"].ToString()
                });
        }

        return View(categoria); // retorna a view passando a lista para exibir
    }
}
```

### Editar

```
//Esse primeiro editar popular a view (form) para editar
public IActionResult Editar(int Id) // recebe o id (pk) da tabela para alterar o registro
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexão
    using var connection = new MySqlConnection(connectionString); // prepara a conexão
    connection.Open(); // abre a conexão

    string sql = "select * from tbCategoria where id = @Id"; //pega o registro pelo id
    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
    command.Parameters.AddWithValue("Id", Id); // adiciona o parametro
    MySqlDataReader reader; // leitor do banco de dados
    Categoria categoria = new Categoria(); // novo objeto de classe Categoria
    reader = command.ExecuteReader(); // faz o leitor baseado na string query

    while (reader.Read()) //enquanto le
    {
        categoria.ID = Convert.ToInt32(reader["ID"]); //popula o objeto categoria e  mapeia
        categoria.Descricao = reader["descricao"].ToString();
        categoria.Nome = reader["Nome"].ToString();
    }

    return View(categoria); // retorna a view com essas informações
}

// Esse segundo editar faz a edição no banco
[HttpPost]
public IActionResult Editar(Categoria categoria) // recebe um objeto de classe Categoria(o objeto que vai ser alterado)
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexão
    using var connection = new MySqlConnection(connectionString); //prepara a conexão
    connection.Open(); //abre a conexão

    string sql = @"update tbCategoria 
                   set id = @Id,
                       Descricao = @Descricao,
                       nome = @Nome"; // string de update

    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando

    command.Parameters.AddWithValue("@Id", categoria.ID); //adiciona os parametros
    command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
    command.Parameters.AddWithValue("@Nome", categoria.Nome);

    command.ExecuteNonQuery(); // executa

    return RedirectToAction("Listar", "Categoria"); // volta para a lista
}


```

### Deletar

```
 public IActionResult Deletar(int Id) // recebe o id do registro que quer deletar
 {
     string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexão
     using var connection = new MySqlConnection(connectionString); // prepara a conexão
     connection.Open(); //abre a conexão

     string sql = "Delete from tbCategoria where id = @Id"; //query para deletar
     MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
     command.Parameters.AddWithValue("@Id", Id); // adiciona o parametro (pk)

     command.ExecuteNonQuery(); //executa

     return RedirectToAction("Listar", "Categoria"); //volta pra lista
 }
```

