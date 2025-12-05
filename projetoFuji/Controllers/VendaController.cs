using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using projetoFuji.Models;
using System.Data;

namespace projetoFuji.Controllers
{
    public class VendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IConfiguration _configuration;

        public VendaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult VerificarLogin()
        {
            string? cpf = TempData.Peek("cpf") as string; // pega o cpf do tempdata (.peek não marca para exclusão)
            if(cpf != null)
            {
                return RedirectToAction("Cadastrar", "Venda"); 
            }
            else
            {
                TempData["MensagemAlerta"] = "Logue para comprar"; //mensagem para o alert
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(List<ItemProduto> itemProduto)
        {

            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); //pega a string de conexão
            using var connection =  new MySqlConnection(connectionString);
            connection.Open();
            string? cpf = TempData.Peek("cpf") as string;

            string sqlVenda = @"CALL sp_insert_Venda(@nf, @Cliente)";
            MySqlCommand command = new MySqlCommand(sqlVenda, connection);
            command.Parameters.AddWithValue("@Nf", itemProduto[0].Venda.Nf);
            command.Parameters.AddWithValue("@Cliente", cpf);
            command.ExecuteNonQuery();

            for (int i = 0; i<itemProduto.Count; i++) // Para cada itemProduto
            { 


                string sql = @"CALL sp_insert_itemVenda(@Nf, @CodigoBarras, @Qtd)"; // string sql

                MySqlCommand commandItem = new MySqlCommand(sql, connection); //comando do itemvenda
                commandItem.Parameters.AddWithValue("@Nf", itemProduto[0].Venda.Nf); // parametros
                commandItem.Parameters.AddWithValue("@CodigoBarras", itemProduto[i].Produto.Codigo_de_barras);
                commandItem.Parameters.AddWithValue("@Qtd", itemProduto[i].Qtd);

                commandItem.ExecuteNonQuery(); // executa
            }
            connection.Close();

            return RedirectToAction("Index", "Venda"); //volta pra lista

        }
        public IActionResult Historico(string? cpf)
        {
            cpf = TempData.Peek("cpf") as string;

            List<Venda> Vendas = new List<Venda>(); // cria uma lista de classe venda

            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexao
            using var connection = new MySqlConnection(connectionString); // conexão
            connection.Open(); //abre a conexao

            string sql = @"SELECT v.Nf, v.DataHora, SUM(i.Preco * i.Qtd) AS TotalGeral
                        FROM tbVenda v
                        INNER JOIN tbItemProduto i ON v.nf = i.nf
                        WHERE v.Cliente = @Cpf
                        GROUP BY v.Nf"; // sql

            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Cpf", cpf);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            connection.Close();
            foreach (DataRow row in dataTable.Rows)
            {
                var venda = new Venda();

                venda.Nf = row["nf"].ToString();
                venda.DataHora = Convert.ToDateTime(row["DataHora"]);
                venda.TotalVenda = Convert.ToDecimal(row["TotalGeral"]);

                Vendas.Add(venda);

            }
            return View(Vendas);
        }
        public IActionResult Listar()
        {

            List<Venda> Vendas = new List<Venda>(); // cria uma lista de classe venda

            string? connectionString = _configuration.GetConnectionString("DefaultConnection"); // string de conexao
            using var connection = new MySqlConnection(connectionString); // conexão
            connection.Open(); //abre a conexao

            string sql = @"SELECT v.Nf, v.DataHora, SUM(i.Preco * i.Qtd) AS TotalGeral,
                                  v.Cliente, v.Funcionario
                        FROM tbVenda v
                        INNER JOIN tbItemProduto i ON v.nf = i.nf
                        GROUP BY v.Nf
                        ORDER BY v.DataHora DESC"; // sql

            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            connection.Close();
            foreach (DataRow row in dataTable.Rows)
            {
                var venda = new Venda();

                venda.Nf = row["nf"].ToString(); //mapeia o valor do campo para o objeto
                venda.Cliente = row["Cliente"].ToString();
                venda.Funcionario = row["Funcionario"].ToString();
                venda.DataHora = Convert.ToDateTime(row["DataHora"]);
                venda.TotalVenda = Convert.ToDecimal(row["TotalGeral"]);

                Vendas.Add(venda); //adiciona a venda a lista

            }
            return View(Vendas);
        }
        public IActionResult Editar(string nf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "select * from tbVenda where nf = @nf";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nf", nf);
            MySqlDataReader reader;
            Venda venda = new Venda();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                venda.Nf = reader["nf"].ToString();
                venda.Cliente = reader["Cliente"].ToString();

                venda.DataHora = Convert.ToDateTime(reader["DataHora"]);
                venda.Funcionario = reader["Funcionario"].ToString();
            }

            return View(venda);
        }

        [HttpPost]
        public IActionResult Editar(Venda venda)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"UPDATE tbVenda 
                   SET DataHora = @DataHora,
                       Funcionario = @Funcionario
                   WHERE nf = @nf"; 

            MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@nf", venda.Nf);
            command.Parameters.AddWithValue("@DataHora", venda.DataHora);
            command.Parameters.AddWithValue("@Funcionario", venda.Funcionario);

            command.ExecuteNonQuery();

            return RedirectToAction("Listar", "Venda");
        }

        public IActionResult Deletar(int nf)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "Delete from tbItemProduto WHERE nf = @Nf";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Nf", nf);

            command.ExecuteNonQuery();

            string sqlVenda = "Delete from tbVenda WHERE nf = @Nf";
            MySqlCommand commandVenda = new MySqlCommand(sqlVenda, connection);

            commandVenda.Parameters.AddWithValue("@Nf", nf);

            commandVenda.ExecuteNonQuery();

            return RedirectToAction("Listar", "Vendar");
        }

    }
}
