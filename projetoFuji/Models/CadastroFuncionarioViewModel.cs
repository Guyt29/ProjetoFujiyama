namespace projetoFuji.Models
{
    public class CadastroFuncionarioViewModel
        //Para conseguir criar o funcionario e a pessoa na mesma view
    {
        public Pessoa Pessoa { get; set; } = new Pessoa(); // recebe um objeto de pessoa para não retornar nulo
        public Funcionario Funcionario { get; set; } = new Funcionario();
    }
}
