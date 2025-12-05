// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function alertar(mensagem) {
    alert('@ViewBag.loginMensagem')
}

let i = 1 // contador do itemvenda

function adicionarFormProduto() {

    const form = document.querySelector("#compraProduto") // pega o form
    const grupoBotao = document.querySelector("#grupoBotao") //pega a div dos botoes
    const inputCodigoTemplate = document.querySelector("#CodigoBarrasInput") // pega os templates dos inputs
    const inputQtdTemplate = document.querySelector("#QtdInput")

    const inputCodigoNovo = inputCodigoTemplate.cloneNode(true) // clona o templete, o true sendo para copiar seus filhos
    const inputQtdNovo = inputQtdTemplate.cloneNode(true)
    const separador = document.createElement("h2") // cria o h2
    separador.innerText = `Produto ${i}` // muda o texto
    inputCodigoNovo.querySelector('input').id = `z${i}__Produto_Codigo_de_barras` // muda o id para o asp net indentificar o registro
    inputCodigoNovo.querySelector('input').name = `[${i}].Produto.Codigo_de_barras` // muda o name para o asp net indentificar o registro

    inputQtdNovo.querySelector('input').id = `z${i}__Qtd` // muda o id para o asp net indentificar o registro
    inputQtdNovo.querySelector('input').name = `[${i}].Qtd` // muda o name para o asp net indentificar o registro

    i++
    form.insertBefore(separador, grupoBotao) // inseri o separador antes do grupobotao
    form.insertBefore(inputCodigoNovo, grupoBotao) // inseri o input antes do grupobotao
    form.insertBefore(inputQtdNovo, grupoBotao) // inseri o input antes do grupobotao
}
