using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using projetoFuji.Models;
using System.Data;

namespace projetoFuji.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IConfiguration _configuration;

        public CategoriaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Categoria categoria)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"INSERT INTO tbCategoria
                           VALUES (@Id, @Descricao, @Nome)";

            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Id", categoria.ID);
            command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
            command.Parameters.AddWithValue("@Nome", categoria.Nome);

            command.ExecuteNonQuery(); //executa

            return RedirectToAction("Listar", "Categoria"); //volta pra lista
        }

        public IActionResult Listar()
        {
            List<Categoria> categoria = new List<Categoria>();
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                string sql = "Select * from tbCategoria";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    categoria.Add(
                        new Categoria
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            Descricao = row["descricao"].ToString(),
                            Nome = row["nome"].ToString()
                        });
                }

                return View(categoria);
            }
        }

        public IActionResult Editar(int Id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "select * from tbCategoria where id = @Id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("Id", Id);
            MySqlDataReader reader;
            Categoria categoria = new Categoria();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                categoria.ID = Convert.ToInt32(reader["ID"]);
                categoria.Descricao = reader["descricao"].ToString();
                categoria.Nome = reader["Nome"].ToString();
            }

            return View(categoria);
        }

        [HttpPost]
        public IActionResult Editar(Categoria categoria)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"update tbCategoria 
                           set id = @Id,
                               Descricao = @Descricao,
                               nome = @Nome";

            MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", categoria.ID);
            command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
            command.Parameters.AddWithValue("@Nome", categoria.Nome);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Categoria");
        }

        public IActionResult Deletar(int Id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "Delete from tbCategoria where id = @Id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", Id);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Categoria");
        }


    }
}