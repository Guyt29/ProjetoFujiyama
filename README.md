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

### ClienteController
A ClienteController possui 2 métodos únicos:

Login
```
[HttpPost]
public IActionResult Login(Cliente cliente)
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
    using var connection = new MySqlConnection(connectionString); // prepara a conxeão
    connection.Open(); //abre a conexão

    string sql = @"SELECT cpf from tbCliente where cpf = @Cpf && senha = @senha"; // Verifica se o cpf e a senha batem
    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
    command.Parameters.AddWithValue("@Cpf", cliente.Cpf); //adiciona os parametros
    command.Parameters.AddWithValue("@Senha", cliente.Senha);

    MySqlDataAdapter adapter = new MySqlDataAdapter(); // adaptador do mysql
    MySqlDataReader reader; // leitor do banco

    Cliente login = new Cliente(); // novo objeto do cliente
    reader = command.ExecuteReader();

    while (reader.Read())
    {
        if (reader["cpf"] != DBNull.Value) // se o registro retorna algo
        {
            cliente.Cpf = reader["cpf"]!.ToString(); // adiciona o cpf ao objeto (o ! para n dar aviso de null)
            TempData["Cpf"] = cliente.Cpf; //salva o cpf do cliente atráves das controllers
            TempData.Keep("Cpf"); // mantem ele após a próxima requisão
            TempData["LoginStatus"] = "Logado com sucesso"; //mensagem de logado (Alert)
        }
    }

    if(TempData.Peek("Cpf") == null) // se não retornar nada o tempdata estará vazio, peek faz não marcar para delação o cpf
    {
        TempData["LoginStatus"] = "Erro no login"; // mensagem de erro pro alert
    }
    return RedirectToAction("Index", "Cliente"); //volta para a index

}
```
Logout
```
public IActionResult Logout()
{
    if (TempData.Peek("Cpf") != null) //Peek pega o valor da tempdata mas n deleta ele, verifica se realmente está logado
    {
        TempData.Remove("cpf"); // remove o cpf do tempData
    }
    return RedirectToAction("Index", "Cliente"); //volta para a index

}
```

### FuncionarioController
A funcionário controller não pode deletar o registro de um funcionário, mas pode demiti-lo

```
public IActionResult Demitir(string cpf) // recebe a pk
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexão
    using var connection = new MySqlConnection(connectionString); // prepara a conexão
    connection.Open(); // abre a conexão

    string sql = "CALL sp_demitir_Funcionario(@Cpf)"; // chama uma stored procedure que adiciona a data de demissão
    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
    command.Parameters.AddWithValue("@Cpf", cpf); // adiciona os parametros

    command.ExecuteNonQuery(); // executa 

    return RedirectToAction("Listar", "Funcionario"); //volta para a lista
}
```

### ProdutoController

A ProdutoController também gerencia as ProdutoCategoria

Cadastro:
``` 
[HttpGet]
public IActionResult CadastrarCategoriaProduto()
{
    return View();
}

[HttpPost]
public IActionResult CadastrarCategoriaProduto(ProdutoCategoria produtoCategoria)
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // pega a string de conexão
    using var connection = new MySqlConnection(connectionString); // cria a conexão com o banco
    connection.Open(); // abre a conexão

    string sql = @"INSERT INTO tbProdutoCategoria   
                          VALUES(@Categoria, @Produto)"; // comando para inserir

    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
    command.Parameters.AddWithValue("@Categoria", produtoCategoria.Categoria.ID); // seta o id da categoria
    command.Parameters.AddWithValue("@Produto", produtoCategoria.Produto.Codigo_de_barras); // seta o código do produto

    command.ExecuteNonQuery(); // executa o insert

    return RedirectToAction("ListarCategoriaPorProduto", "Produto", new { codigo_de_barras = produtoCategoria.Produto.Codigo_de_barras }); // redireciona para a lista de categorias de um produto, passa também o codigo de barras do produto
}
```

Lista
``` 
public IActionResult ListarCategoriaPorProduto(string codigo_de_barras)
{
    List<ProdutoCategoria> produtoCategorias = new List<ProdutoCategoria>(); // lista que vai guardar as categorias ligadas ao produto

    using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"))) // cria a conexão
    {
        conn.Open(); // abre a conexão

        string sql = @"SELECT p.codigo_de_barras, p.Nome AS prodNome, 
                              c.Id, c.Nome AS cateNome, c.descricao
                       FROM tbProdutoCategoria pc
                       INNER JOIN tbProduto p ON p.codigo_de_barras = pc.codigo_de_barras
                       INNER JOIN tbCategoria c ON c.Id = pc.Categoria
                       WHERE pc.codigo_de_barras = @Codigo_de_Barras"; // query que pega o produto e suas categorias

        MySqlCommand command = new MySqlCommand(sql, conn); // prepara o comando SQL
        command.Parameters.AddWithValue("@Codigo_de_Barras", codigo_de_barras); // passa o código do produto

        MySqlDataAdapter adapter = new MySqlDataAdapter(command); // adaptador para converter o retorno
        DataTable dataTable = new DataTable(); // tabela temporária
        adapter.Fill(dataTable); // preenche com os dados do banco
        conn.Close(); // fecha a conexão

        // percorre cada registro retornado
        foreach (DataRow row in dataTable.Rows)
        {
            var viewModel = new ProdutoCategoria(); // cria o objeto que representa Produto + Categoria

            // Preenche a parte do produto
            viewModel.Produto.Codigo_de_barras = row["Codigo_de_Barras"].ToString();
            viewModel.Produto.Nome = row["prodNome"].ToString();

            // Preenche a parte da categoria
            viewModel.Categoria.ID = Convert.ToInt32(row["Id"]);
            viewModel.Categoria.Nome = row["cateNome"].ToString();
            viewModel.Categoria.Descricao = row["descricao"].ToString();

            produtoCategorias.Add(viewModel); // adiciona na lista
        }

        return View(produtoCategorias); // manda pra view a lista preenchida
    }
}

```

Deletar

``` 
public IActionResult DeletarProdutoCategoria(string codigo_de_barras, int id)
{
    string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // pega a connection string
    using var connection = new MySqlConnection(connectionString); // cria a conexão com o banco
    connection.Open(); // abre a conexão

    string sql = "DELETE FROM tbProdutoCategoria WHERE Codigo_de_barras = @Codigo_de_barras && Categoria = @Id"; // comando sql para remover os registros

    MySqlCommand command = new MySqlCommand(sql, connection); // prepara o comando
    command.Parameters.AddWithValue("@Codigo_de_barras", codigo_de_barras); // passa o código do produto
    command.Parameters.AddWithValue("@Id", id); // passa o ID da categoria

    command.ExecuteNonQuery(); // executa o delete

    // redireciona de volta para a tela que lista as categorias do produto passando o código de barras
    return RedirectToAction("ListarCategoriaPorProduto", "Produto", new { codigo_de_barras = codigo_de_barras });
}

```



