using Microsoft.AspNetCore.Mvc;
using projetoFuji.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace projetoFuji.Controllers
{
    public class FornecedorController : Controller
    {

        private readonly IConfiguration _configuration;
        public FornecedorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Cadastrar(Fornecedor fornecedor)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString); //
            connection.Open(); 

            string sql = "INSERT INTO tbFornecedor (CNPJ,nome, endereco, telefone, email) VALUES (@CNPJ, @nome, @endereco, @telefone, @email)";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@CNPJ", fornecedor.CNPJ);
            command.Parameters.AddWithValue("@nome", fornecedor.Nome);
            command.Parameters.AddWithValue("@endereco", fornecedor.Endereco);
            command.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
            command.Parameters.AddWithValue("@email", fornecedor.Email);


            
            command.ExecuteNonQuery(); //executa


            return RedirectToAction("Listar", "Fornecedor"); //volta pra lista

        }
        public IActionResult Listar()
        {
            List<Fornecedor> produtos = new List<Fornecedor>();
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();
                string sql = "Select * from tbFornecedor ";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    produtos.Add(
                        new Fornecedor
                        {
                            CNPJ = row["CNPJ"].ToString(),
                            Nome = row["nome"].ToString(),
                            Endereco = row["endereco"].ToString(),
                            Telefone = row["telefone"].ToString(),
                            Email = row["email"].ToString(),

                        });

                }
                return View(produtos);
            }

        }
        public IActionResult Editar(int cnpj)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "select * from tbFornecedor where CNPJ=@CNPJ";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CNPJ", cnpj);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataReader reader;
            Fornecedor fornecedor = new Fornecedor();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                fornecedor.CNPJ = Convert.ToString(reader["CNPJ"]);
                fornecedor.Nome = Convert.ToString(reader["nome"]);
                fornecedor.Endereco = Convert.ToString(reader["endereco"]);
                fornecedor.Telefone = Convert.ToString(reader["telefone"]);
                fornecedor.Email = Convert.ToString(reader["email"]);




            }

            return View(fornecedor);

        }
        [HttpPost]
        public IActionResult Editar(Fornecedor fornecedor)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "update tbFornecedor set nome=@nome, endereco =@endereco, telefone = @telefone, email= @email where CNPJ=@CNPJ";
            MySqlCommand command = new MySqlCommand(sql, connection);
             
            command.Parameters.AddWithValue("@CNPJ", fornecedor.CNPJ);
            command.Parameters.AddWithValue("@nome", fornecedor.Nome);
            command.Parameters.AddWithValue("@endereco", fornecedor.Endereco);
            command.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
            command.Parameters.AddWithValue("@email", fornecedor.Email);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Fornecedor");
        }
        public IActionResult Deletar(string CNPJ)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "Delete from tbFornecedor where CNPJ = @CNPJ";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CNPJ", CNPJ);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Fornecedor");
        }

    }
}
