using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using projetoFuji.Models;
using System.Data;

namespace projetoFuji.Controllers
{
    public class FuncionarioController : Controller
    {

        private readonly IConfiguration _configuration;
        public FuncionarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Cadastrar(CadastroFuncionarioViewModel  model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString); //
            connection.Open();
            string sql = "CALL sp_insert_Funcionario(@Cpf, @Nome, @Email, @Genero, @Idade, @Telefone, @Supervisor, @Funcao, @Salario, @DataDeAdmissao, @DataDemissao)";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Cpf", model.Pessoa.Cpf);
            command.Parameters.AddWithValue("@Nome", model.Pessoa.Nome);
            command.Parameters.AddWithValue("@Genero", model.Pessoa.Genero);
            command.Parameters.AddWithValue("@Telefone", model.Pessoa.Telefone);
            command.Parameters.AddWithValue("@Email", model.Pessoa.Email);
            command.Parameters.AddWithValue("@Idade", model.Pessoa.Idade);
            command.Parameters.AddWithValue("@Supervisor", model.Funcionario.Supervisor);
            command.Parameters.AddWithValue("@Funcao", model.Funcionario.Funcao);
            command.Parameters.AddWithValue("@Salario", model.Funcionario.Salario);
            command.Parameters.AddWithValue("@DataDeAdmissao", model.Funcionario.DataDeAdmissao);
            command.Parameters.AddWithValue("@DataDemissao", model.Funcionario.DataDemissao);




            command.ExecuteNonQuery(); //executa

            return RedirectToAction("Listar", "Funcionario"); //volta pra lista

        }
        public IActionResult Listar()
        {
            List<CadastroFuncionarioViewModel> funcionarios = new List<CadastroFuncionarioViewModel>();
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();
                string sql = @"SELECT 
                            p.Cpf, p.Nome, p.Email, p.Genero, p.Idade, p.Telefone,
                            f.Supervisor, f.Funcao, f.Salario, f.DataDeAdmissao, f.DataDemissao
                            FROM tbPessoa p
                            INNER JOIN tbFuncionarios f ON p.Cpf = f.Cpf; ";

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    var viewModel = new CadastroFuncionarioViewModel();

                    viewModel.Pessoa.Cpf = row["Cpf"].ToString();
                    viewModel.Pessoa.Nome = row["nome"].ToString();
                    viewModel.Pessoa.Email = row["email"].ToString();
                    viewModel.Pessoa.Genero = row["genero"].ToString();
                    viewModel.Pessoa.Idade = Convert.ToInt32(row["idade"]);
                    viewModel.Pessoa.Telefone = row["Nome"].ToString();

                    viewModel.Funcionario.Cpf = row["Cpf"].ToString();
                    viewModel.Funcionario.Funcao = row["funcao"].ToString();
                    viewModel.Funcionario.Salario = Convert.ToDecimal(row["salario"]);
                    viewModel.Funcionario.DataDeAdmissao = Convert.ToDateTime(row["dataDeAdmissao"]);
                    //Verifica se essa coluna está null senao da erro pq o null do c# é diferente do sql???
                    if (row["DataDemissao"] != DBNull.Value)
                    {
                        viewModel.Funcionario.DataDemissao = Convert.ToDateTime(row["DataDemissao"]);
                    }
                    else
                    {
                        viewModel.Funcionario.DataDemissao = null; 
                    }
                    if (row["Supervisor"] != DBNull.Value)
                    {
                        viewModel.Funcionario.Supervisor = row["Supervisor"].ToString();
                    }
                    else
                    {
                        viewModel.Funcionario.Supervisor = null;
                    }
                    funcionarios.Add(viewModel);
    
                }
                return View(funcionarios);
            }

        }
        public IActionResult Editar(string cpf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"SELECT 
                            p.Cpf, p.Nome, p.Email, p.Genero, p.Idade, p.Telefone,
                            f.Supervisor, f.Funcao, f.Salario, f.DataDeAdmissao, f.DataDemissao
                            FROM tbPessoa p
                            INNER JOIN tbFuncionarios f ON p.Cpf = f.Cpf
                            WHERE p.Cpf = @Cpf"; 
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Cpf", cpf);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataReader reader;
            CadastroFuncionarioViewModel viewModel = new CadastroFuncionarioViewModel();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                viewModel.Pessoa.Cpf = Convert.ToString(reader["Cpf"]);
                viewModel.Pessoa.Nome = Convert.ToString(reader["Nome"]);
                viewModel.Pessoa.Email = Convert.ToString(reader["Email"]);
                viewModel.Pessoa.Genero = Convert.ToString(reader["Genero"]);
                viewModel.Pessoa.Idade = Convert.ToInt32(reader["Idade"]);
                viewModel.Pessoa.Telefone = Convert.ToString(reader["Telefone"]);

                viewModel.Funcionario.Cpf = Convert.ToString(reader["Cpf"]);
                viewModel.Funcionario.Funcao = Convert.ToString(reader["Funcao"]);
                viewModel.Funcionario.Salario = Convert.ToDecimal(reader["Salario"]);
                viewModel.Funcionario.DataDeAdmissao = Convert.ToDateTime(reader["DataDeAdmissao"]);

                if (reader["DataDemissao"] != DBNull.Value)
                {
                    viewModel.Funcionario.DataDemissao = Convert.ToDateTime(reader["DataDeAdmissao"]);
                }
                else
                {
                    viewModel.Funcionario.DataDemissao = null;
                }
                if (reader["Supervisor"] != DBNull.Value)
                {
                    viewModel.Funcionario.Supervisor = Convert.ToString(reader["Supervisor"]);

                }
                else
                {
                    viewModel.Funcionario.Supervisor = null;
                }


            }

            return View(viewModel);

        }
        [HttpPost]
        public IActionResult Editar(CadastroFuncionarioViewModel model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "CALL sp_update_Funcionario(@Cpf, @Nome, @Email, @Genero, @Idade, @Telefone, @Supervisor, @Funcao, @Salario, @DataDeAdmissao, @DataDemissao)";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Cpf", model.Pessoa.Cpf);
            command.Parameters.AddWithValue("@Nome", model.Pessoa.Nome);
            command.Parameters.AddWithValue("@Genero", model.Pessoa.Genero);
            command.Parameters.AddWithValue("@Telefone", model.Pessoa.Telefone);
            command.Parameters.AddWithValue("@Email", model.Pessoa.Email);
            command.Parameters.AddWithValue("@Idade", model.Pessoa.Idade);
            command.Parameters.AddWithValue("@Supervisor", model.Funcionario.Supervisor);
            command.Parameters.AddWithValue("@Funcao", model.Funcionario.Funcao);
            command.Parameters.AddWithValue("@Salario", model.Funcionario.Salario);
            command.Parameters.AddWithValue("@DataDeAdmissao", model.Funcionario.DataDeAdmissao);
            command.Parameters.AddWithValue("@DataDemissao", model.Funcionario.DataDemissao);

            Console.WriteLine(model.Pessoa.Cpf);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Funcionario");
        }
        public IActionResult Demitir(string cpf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "CALL sp_demitir_Funcionario(@Cpf)";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Cpf", cpf);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Funcionario");
        }

    }
}
