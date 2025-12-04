/* DROP DATABASE IF EXISTS dbfujiyama;
    
    CREATE DATABASE IF NOT EXISTS dbFujiyama;
	USE dbFujiyama;


	CREATE TABLE tbFornecedor(
		CNPJ char(14) PRIMARY KEY,
		nome varchar(100) NOT NULL,
		endereco varchar(200),
		telefone char(11),
		email varchar(100)
	);

	CREATE TABLE tbProduto(
		Codigo_de_Barras decimal(13,0) PRIMARY KEY,
		Preco decimal(8,2) NOT NULL,
		Custo decimal(8,2) NOT NULL,
		Nome varchar(150) NOT NULL,
		Pais_de_origem varchar(50),
		Data_de_Validade date,
		Descricao varchar(200),
		Qtd int,
		Fornecedor char(14),
		FOREIGN KEY (Fornecedor) REFERENCES tbFornecedor(CNPJ)
	);	

	CREATE TABLE tbCategoria(
		id int PRIMARY KEY,
		Descricao varchar(200),
		Nome varchar(100) NOT NULL UNIQUE
	);

	CREATE TABLE tbProdutoCategoria(
		Categoria int,
		Codigo_de_Barras decimal(13,0),
		PRIMARY KEY(Categoria, Codigo_de_Barras),
		FOREIGN KEY (Categoria) REFERENCES tbCategoria(ID),
		FOREIGN KEY (Codigo_de_Barras) REFERENCES tbProduto(Codigo_de_barras)
	); 

	CREATE TABLE tbPessoa(
		Cpf decimal(11,0) primary key,
		nome varchar(200) not null,
		email varchar(100) not null, 
		genero char(1)  not null,
		idade tinyint not null, 
		telefone char(11) not null
	);

	CREATE TABLE tbFuncionarios(
		Cpf decimal(11,0) PRIMARY KEY, 
		Supervisor decimal(11,0),
		funcao varchar(75) not null,
		salario decimal(8,2) not null,
		dataDeAdmissao date not null,
		dataDemissao date,
		FOREIGN KEY (Supervisor) REFERENCES tbFuncionarios(CPF),
		FOREIGN KEY (Cpf) REFERENCES tbPessoa(Cpf)
	);

	CREATE TABLE tbFolhaDePagamento(
		Id int primary key AUTO_INCREMENT,
		Salario decimal(8,2) not null,
		ValorAjuste decimal(8,2) not null,
		DataPagamento date not null,
		Funcionario decimal(11,0) NOT NULL, 
		foreign key(Funcionario) references tbFuncionarios(cpf)
	);
	CREATE TABLE tbCliente(
		Cpf decimal(11,0) primary key,
		DataCadastro date not null, 
		DataPrimeiraCompra date,
        Senha varchar(50) NOT NULL,
		foreign key(Cpf) references tbPessoa(Cpf)
	); 

	CREATE TABLE tbVenda(
		Nf decimal(9,0) primary key, 
		DataHora datetime, 
		Funcionario decimal(11,0) not null, 
		Cliente decimal(11,0) not null,
		foreign key(Funcionario) references tbFuncionarios(cpf),
		foreign key(Cliente) references tbCliente(cpf)
	);

	CREATE TABLE tbItemProduto(
		CodigoBarras decimal(13, 0),
		Nf decimal(9, 0),
		Qtd smallint not null, 
		Preco decimal(8,2) not null,
		primary key (CodigoBarras, Nf), 
		foreign key (CodigoBarras) references tbProduto (Codigo_de_Barras),
		foreign key (Nf) references tbVenda(NF)
	);


	-- PROCEDURES
	DELIMITER $$
	CREATE PROCEDURE sp_insert_Pessoa(vCPF decimal(11,0), vNome varchar(200), vEmail varchar(100), vGenero char(1), vIdade tinyint, vTelefone char(11))
	BEGIN
		IF NOT EXISTS(SELECT CPF from TBPESSOA WHERE CPF = vCpf) THEN
			INSERT INTO tbPessoa VALUES (vCpf, vNome, vEmail, vGenero, vIdade, vTelefone);
		END IF;
		
	END $$


	DELIMITER $$
	CREATE PROCEDURE sp_insert_Funcionario(vCpf decimal(11,0), vNome varchar(200), vEmail varchar(100), vGenero char(1), vIdade tinyint, vTelefone char(11), vSupervisor decimal(11,0), vFuncao varchar(75), vSalario decimal(8,2), vDataDeAdmissao date, vDataDemissao date)
	BEGIN
		IF NOT EXISTS(SELECT CPF from TBPESSOA WHERE CPF = vCpf) THEN
			CALL sp_insert_Pessoa(vCpf, vNome, vEmail, vGenero, vIdade, vTelefone);
		END IF;
		INSERT INTO tbFuncionarios VALUES (vCpf, vSupervisor, vFuncao, vSalario, vDataDeAdmissao, vDataDemissao);
		
	END $$

	DELIMITER $$
	CREATE PROCEDURE sp_update_Funcionario(vCpf decimal(11,0), vNome varchar(200), vEmail varchar(100), vGenero char(1), vIdade tinyint, vTelefone char(11), vSupervisor decimal(11,0), vFuncao varchar(75), vSalario decimal(8,2), vDataDeAdmissao date, vDataDemissao date)
	BEGIN
		UPDATE tbPessoa SET
		nome = vNome,
		email = vEmail,
		genero= vGenero,
		idade = vIdade,
		telefone = vTelefone
		WHERE Cpf = vCpf;
		
		UPDATE tbFuncionarios SET
		Supervisor = vSupervisor,
		funcao = vFuncao,
		salario = vSalario,
		dataDeAdmissao = vDataDeAdmissao,
		dataDemissao = vDataDemissao
		WHERE Cpf = vCpf;
	END $$

	DELIMITER $$
	CREATE PROCEDURE sp_demitir_Funcionario(vCpf decimal(11,0))
	BEGIN

		UPDATE tbFuncionarios SET
		dataDemissao = current_date()
		WHERE Cpf = vCpf;
	END $$
    
DESCRIBE tbCliente;
DELIMITER $$
CREATE PROCEDURE sp_insert_Cliente(vCPF decimal(11,0), vNome varchar(200), vEmail varchar(100), vGenero char(1), vIdade tinyint, vTelefone char(11), vSenha varchar(50))
BEGIN
		IF NOT EXISTS(SELECT CPF from TBPESSOA WHERE CPF = vCpf) THEN
			CALL sp_insert_Pessoa(vCpf, vNome, vEmail, vGenero, vIdade, vTelefone);
		END IF;
        INSERT INTO tbCliente(Cpf, DataCadastro, Senha) VALUES (vCpf, current_timestamp(), vSenha);
END $$

	DELIMITER $$
	CREATE PROCEDURE sp_update_Cliente(vCpf decimal(11,0), vNome varchar(200), vEmail varchar(100), vGenero char(1), vIdade tinyint, vTelefone char(11), vSenha varchar(50))
	BEGIN
		UPDATE tbPessoa SET
		nome = vNome,
		email = vEmail,
		genero= vGenero,
		idade = vIdade,
		telefone = vTelefone
		WHERE Cpf = vCpf;
		
		UPDATE tbCliente SET
		senha = vSenha
		WHERE Cpf = vCpf;
	END $$

/* SELECT 
p.Cpf, p.Nome, p.Email, p.Genero, p.Idade, p.Telefone,
f.Supervisor, f.Funcao, f.Salario, f.DataDeAdmissao, f.DataDemissao
FROM tbPessoa p
INNER JOIN tbFuncionarios f ON p.Cpf = f.Cpf;

SELECT cpf  from tbCliente where cpf = '21234567891' && Senha = 'bombom';
select * from tbcliente;
DESCRIBE tbProdutoCategoria
SELECT p.codigo_de_barras, p.Nome AS prodNome, c.Id, c.Nome AS cateNome, c.descricao
FROM tbProdutoCategoria pc
INNER JOIN tbProduto p ON p.codigo_de_barras = pc.codigo_de_barras
INNER JOIN tbCategoria c on c.Id = pc.Categoria
WHERE pc.codigo_de_barras = 1234567891011;
SELECT * FROM tbProdutoCategoria;

SELECT p.Cpf, p.Nome, p.Email, p.Genero, p.Idade, p.Telefone,c.Senha
												 FROM tbCliente c
                                                  INNER JOIN tbPessoa p On p.cpf = c.cpf
                                                  WHERE c.Cpf = '21234567891'
                                                  
                                                  select * from tbcliente*/
    