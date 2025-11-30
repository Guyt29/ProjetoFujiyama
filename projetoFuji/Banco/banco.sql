CREATE DATABASE dbFujiyama;
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
	Id int primary key,
    Salario decimal(8,2) not null,
    ValorAjuste decimal(8,2) not null,
    DataPagamento date not null,
    Funcionario decimal(11,0), 
    foreign key(Funcionario) references tbFuncionarios(cpf)
);

CREATE TABLE tbCliente(
	Cpf decimal(11,0) primary key,
    DataCadastro date not null, 
    DataPrimeiraCompra date not null, 
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
