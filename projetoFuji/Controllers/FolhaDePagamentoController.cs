using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using projetoFuji.Models;
using System.Data;

namespace projetoFuji.Controllers
{
    public class FolhaDePagamentoController : Controller
    {
        private readonly IConfiguration _configuration;
        public FolhaDePagamentoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Cadastrar(string? cpf, decimal salario)
        {
            var modelo = new FolhaDePagamento();
            modelo.Cpf = cpf;
            modelo.Salario = salario;
            return View(modelo);

        }
        [HttpPost]
        public IActionResult Cadastrar(FolhaDePagamento pagamento)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString); //
            connection.Open();
            string sql = @"INSERT INTO tbFolhaDePagamento (Salario, ValorAjuste, DataPagamento, Funcionario) 
                                VALUES (@Salario, @ValorAjuste, @DataPagamento, @Funcionario)";

            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Salario", pagamento.Salario);
            command.Parameters.AddWithValue("@DataPagamento", pagamento.DataPagamento);
            command.Parameters.AddWithValue("@ValorAjuste", pagamento.ValorAjuste);
            command.Parameters.AddWithValue("@Funcionario", pagamento.Cpf);

            command.ExecuteNonQuery(); //executa
            return RedirectToAction("Listar", "FolhaDePagamento", new {cpf = pagamento.Cpf}); //volta pra lista

        }
        public IActionResult Listar(string cpf)
        {
            ViewBag.cpf = cpf;
            List<FolhaDePagamento> pagamentos = new List<FolhaDePagamento>();
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();
                string sql = @"SELECT * FROM tbFolhaDePagamento WHERE Funcionario = @Funcionario";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@Funcionario", cpf);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    var pagamento = new FolhaDePagamento();

                    pagamento.Id = Convert.ToInt32(row["id"]);
                    pagamento.Cpf = row["funcionario"].ToString();
                    pagamento.Salario = Convert.ToDecimal(row["Salario"]);
                    pagamento.ValorAjuste = Convert.ToDecimal(row["ValorAjuste"]);
                    pagamento.DataPagamento = Convert.ToDateTime(row["DataPagamento"]);
                    pagamento.ValorPago = pagamento.Salario + pagamento.ValorAjuste;

                    pagamentos.Add(pagamento);

                }
                return View(pagamentos);
            }

        }
        public IActionResult Editar(string cpf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"SELECT * FROM tbFolhaDePagamento";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataReader reader;
            FolhaDePagamento pagamento = new FolhaDePagamento();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                pagamento.Id = Convert.ToInt32(reader["id"]);
                pagamento.Salario = Convert.ToDecimal(reader["salario"]);
                pagamento.Cpf = reader["Funcionario"].ToString();
                pagamento.ValorAjuste = Convert.ToDecimal(reader["valorAjuste"]);
                pagamento.DataPagamento = Convert.ToDateTime(reader["DataPagamento"]);




            }

            return View(pagamento);

        }
        [HttpPost]
        public IActionResult Editar(FolhaDePagamento model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"UPDATE tbFolhaDePagamento
                           SET Id = @Id,
                               Salario = @Salario,
                               ValorAjuste = @ValorAjuste,
                                DataPagamento = @DataPagamento,
                                Funcionario = @Funcionario";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Id", model.Id);
            command.Parameters.AddWithValue("@Salario", model.Salario);
            command.Parameters.AddWithValue("@ValorAjuste", model.ValorAjuste);
            command.Parameters.AddWithValue("@DataPagamento", model.DataPagamento);
            command.Parameters.AddWithValue("@Funcionario", model.Cpf);



            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "FolhaDePagamento", new {cpf = model.Cpf});
        }
        public IActionResult Deletar(int id, string cpf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "DELETE FROM tbFolhaDePagamento WHERE id = @id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "FolhaDePagamento", new {cpf = cpf});
        }
    }
}
