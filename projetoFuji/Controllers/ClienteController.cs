using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using projetoFuji.Models;

namespace projetoFuji.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IConfiguration _configuration;
        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Cadastrar(ClientePessoa clientePessoa)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = @"CALL sp_insert_Cliente(@Cpf, @Nome, @Email, @Genero, @Idade, @Telefone, @Senha)";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Cpf", clientePessoa.Pessoa.Cpf);
            command.Parameters.AddWithValue("@Nome", clientePessoa.Pessoa.Nome);
            command.Parameters.AddWithValue("@Email", clientePessoa.Pessoa.Email);
            command.Parameters.AddWithValue("@Genero", clientePessoa.Pessoa.Genero);
            command.Parameters.AddWithValue("@Idade", clientePessoa.Pessoa.Idade);
            command.Parameters.AddWithValue("@Telefone", clientePessoa.Pessoa.Telefone);
            command.Parameters.AddWithValue("@Senha", clientePessoa.Cliente.Senha);

            command.ExecuteNonQuery(); //executa

            return RedirectToAction("Index", "Home"); //volta para a index

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Login(Cliente cliente)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = @"SELECT cpf from tbCliente where cpf = @Cpf && senha = @senha";
            MySqlCommand command = new MySqlCommand(sql, connection); //adiciona os parametros
            command.Parameters.AddWithValue("@Cpf", cliente.Cpf);
            command.Parameters.AddWithValue("@Senha", cliente.Senha);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataReader reader;
            Cliente login = new Cliente();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["cpf"] != DBNull.Value)
                {
                    cliente.Cpf = reader["cpf"]!.ToString();
                    TempData["Cpf"] = cliente.Cpf; //salva o cpf do cliente atráves das controllers
                    TempData.Keep("Cpf"); // mantem ele após varias requisições
                    TempData["LoginStatus"] = "Logado com sucesso"; //mensagem de logado (Alert)
                    
                }


            }
            if(TempData.Peek("Cpf") == null)
            {
                TempData["LoginStatus"] = "Erro no login"; // verifica se o tempdata for preenchido e caso n muda o mensagem
            }
            return RedirectToAction("Index", "Cliente"); //volta para a index

        }

        public IActionResult Logout()
        {
            if (TempData.Peek("Cpf") != null) //Peek pega o valor da tempdata mas n deleta ele 
            {
                TempData.Remove("cpf");
            }
            return RedirectToAction("Index", "Cliente"); //volta para a index

        }
        [HttpGet]
        public IActionResult Editar()
        {
            string? cpf = TempData.Peek("cpf") as string;

            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = @"SELECT p.Cpf, p.Nome, p.Email, p.Genero, p.Idade, p.Telefone,c.Senha
												 FROM tbCliente c
                                                  INNER JOIN tbPessoa p On p.cpf = c.cpf
                                                  WHERE c.Cpf = @cpf";

            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@cpf", cpf);

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataReader reader;
            ClientePessoa viewModel = new ClientePessoa();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                viewModel.Pessoa.Cpf = Convert.ToString(reader["Cpf"]);
                viewModel.Pessoa.Nome = Convert.ToString(reader["Nome"]);
                viewModel.Pessoa.Email = Convert.ToString(reader["Email"]);
                viewModel.Pessoa.Genero = Convert.ToString(reader["Genero"]);
                viewModel.Pessoa.Idade = Convert.ToInt32(reader["Idade"]);
                viewModel.Pessoa.Telefone = Convert.ToString(reader["Telefone"]);

                viewModel.Cliente.Senha = reader["Senha"].ToString();


            }

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Editar(ClientePessoa model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "CALL sp_update_Cliente(@Cpf, @Nome, @Email, @Genero, @Idade, @Telefone, @Senha)";
            MySqlCommand command = new MySqlCommand(sql, connection); 

            command.Parameters.AddWithValue("@Cpf", model.Pessoa.Cpf);             //adiciona os parametros
            command.Parameters.AddWithValue("@Nome", model.Pessoa.Nome);
            command.Parameters.AddWithValue("@Genero", model.Pessoa.Genero);
            command.Parameters.AddWithValue("@Telefone", model.Pessoa.Telefone);
            command.Parameters.AddWithValue("@Email", model.Pessoa.Email);
            command.Parameters.AddWithValue("@Idade", model.Pessoa.Idade);
            command.Parameters.AddWithValue("@Senha", model.Cliente.Senha);



            command.ExecuteNonQuery();

            return RedirectToAction("Index", "Cliente");
        }
    }
}
