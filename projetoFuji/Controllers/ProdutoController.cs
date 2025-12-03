using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using projetoFuji.Models;
using System.Data;

namespace projetoFuji.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto produto)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"INSERT INTO tbProduto 
                          (Codigo_de_barras, Preco, Custo, Nome, Pais_de_origem, Data_de_validade, Descricao, Qtd, Fornecedor) 
                          VALUES 
                          (@Codigo_de_barras, @Preco, @Custo, @Nome, @Pais_de_origem, @Data_de_validade, @Descricao, @Qtd, @Fornecedor)";

            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Codigo_de_barras", produto.Codigo_de_barras);
            command.Parameters.AddWithValue("@Preco", produto.Preco);
            command.Parameters.AddWithValue("@Custo", produto.Custo);
            command.Parameters.AddWithValue("@Nome", produto.Nome);
            command.Parameters.AddWithValue("@Pais_de_origem", produto.Pais_de_origem);

            if (produto.Data_de_validade.HasValue)
            {
                command.Parameters.AddWithValue("@Data_de_validade",
                    produto.Data_de_validade.Value.ToDateTime(TimeOnly.MinValue));
            }
            else
            {
                command.Parameters.AddWithValue("@Data_de_validade", DBNull.Value);
            }

            command.Parameters.AddWithValue("@Descricao", produto.Descricao);
            command.Parameters.AddWithValue("@Qtd", produto.Qtd);
            command.Parameters.AddWithValue("@Fornecedor", produto.Fornecedor);

            command.ExecuteNonQuery(); //executa

            return RedirectToAction("Listar", "Produto"); //volta pra lista
        }

        public IActionResult Listar()
        {
            List<Produto> produtos = new List<Produto>();
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                string sql = "Select * from tbProduto";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    produtos.Add(
                        new Produto
                        {
                            Codigo_de_barras = row["Codigo_de_barras"].ToString(),
                            Preco = row["Preco"].ToString(),
                            Custo = row["Custo"].ToString(),
                            Nome = row["Nome"].ToString(),
                            Pais_de_origem = row["Pais_de_origem"].ToString(),
                            Descricao = row["Descricao"].ToString(),
                            Qtd = row["Qtd"] != DBNull.Value ? Convert.ToInt32(row["Qtd"]) : (int?)null,
                            Fornecedor = row["Fornecedor"].ToString(),
                            Data_de_validade = row["Data_de_validade"] != DBNull.Value
                                ? DateOnly.FromDateTime(Convert.ToDateTime(row["Data_de_validade"]))
                                : (DateOnly?)null
                        });
                }

                return View(produtos);
            }
        }

        public IActionResult Editar(string Codigo_de_barras)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "select * from tbProduto where Codigo_de_barras=@Codigo_de_barras";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Codigo_de_barras", Codigo_de_barras);
            MySqlDataReader reader;
            Produto produto = new Produto();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                produto.Codigo_de_barras = Convert.ToString(reader["Codigo_de_barras"]);
                produto.Preco = Convert.ToString(reader["Preco"]);
                produto.Custo = Convert.ToString(reader["Custo"]);
                produto.Nome = Convert.ToString(reader["Nome"]);
                produto.Pais_de_origem = Convert.ToString(reader["Pais_de_origem"]);
                produto.Descricao = Convert.ToString(reader["Descricao"]);
                produto.Qtd = reader["Qtd"] != DBNull.Value ? Convert.ToInt32(reader["Qtd"]) : (int?)null;
                produto.Fornecedor = Convert.ToString(reader["Fornecedor"]);

                if (reader["Data_de_validade"] != DBNull.Value)
                {
                    produto.Data_de_validade = DateOnly.FromDateTime(
                        Convert.ToDateTime(reader["Data_de_validade"])
                    );
                }
            }

            return View(produto);
        }

        [HttpPost]
        public IActionResult Editar(Produto produto)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"update tbProduto 
                           set Preco=@Preco, 
                               Custo=@Custo, 
                               Nome=@Nome, 
                               Pais_de_origem=@Pais_de_origem, 
                               Data_de_validade=@Data_de_validade,
                               Descricao=@Descricao, 
                               Qtd=@Qtd, 
                               Fornecedor=@Fornecedor 
                           where Codigo_de_barras=@Codigo_de_barras";

            MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Codigo_de_barras", produto.Codigo_de_barras);
            command.Parameters.AddWithValue("@Preco", produto.Preco);
            command.Parameters.AddWithValue("@Custo", produto.Custo);
            command.Parameters.AddWithValue("@Nome", produto.Nome);
            command.Parameters.AddWithValue("@Pais_de_origem", produto.Pais_de_origem);

            if (produto.Data_de_validade.HasValue)
            {
                command.Parameters.AddWithValue("@Data_de_validade",
                    produto.Data_de_validade.Value.ToDateTime(TimeOnly.MinValue));
            }
            else
            {
                command.Parameters.AddWithValue("@Data_de_validade", DBNull.Value);
            }

            command.Parameters.AddWithValue("@Descricao", produto.Descricao);
            command.Parameters.AddWithValue("@Qtd", produto.Qtd);
            command.Parameters.AddWithValue("@Fornecedor", produto.Fornecedor);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Produto");
        }

        public IActionResult Deletar(string Codigo_de_barras)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "Delete from tbProduto where Codigo_de_barras = @Codigo_de_barras";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Codigo_de_barras", Codigo_de_barras);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Produto");
        }
    }
}