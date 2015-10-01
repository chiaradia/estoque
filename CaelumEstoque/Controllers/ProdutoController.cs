using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaelumEstoque.DAO;
using CaelumEstoque.Models;

namespace CaelumEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        // GET: Produto
        /**A customização de uma url no Asp.Net MVC 5 é feita utilizando-se uma anotação definida no framework chamada RouteAttribute, 
         * essa anotação é utilizada sobre actions dos controllers e recebe como argumento a nova url do método.**/
        [Route("produtos", Name="ListarProdutos")]
        public ActionResult Index()
        {
            ProdutosDAO produtosDAO = new ProdutosDAO();
            IList<Produto> produtos = produtosDAO.Lista();
            ViewBag.Produtos = produtos;
            return View();
        }

        [Route("cadastrar-produto")]
        public ActionResult FormCadastraProduto()
        {
            CategoriasDAO categoriasDAO = new CategoriasDAO();
            IList<CategoriaDoProduto> categorias = categoriasDAO.Lista();
            ViewBag.Categorias = categorias;
            ViewBag.Produto = new Produto();
            return View();
        }

        /**public ActionResult Adiciona(String nome, float preco, String descricao, int quantidade, int categoriaId)
        {
            Produto produto = new Produto();
            produto.Nome = nome;
            produto.Preco = preco;
            produto.Descricao = descricao;
            produto.Quantidade = quantidade;
            produto.CategoriaId = categoriaId;

            ProdutosDAO produtosDAO = new ProdutosDAO();
            produtosDAO.Adiciona(produto);
            return View();
        }**/

        //Utilização do componente Model Binder e o método somente aceitará requisições via POST devido a anotação
        //A variável produto recebida pelo método da classe controladora é instanciada e preenchida por um 
        //componente do ASP.NET MVC conhecido como Model Binder.
        [HttpPost]
        public ActionResult Adiciona(Produto produto)
        {
            int idDaInformatica = 1;
            if (produto.CategoriaId.Equals(idDaInformatica) && produto.Preco < 100)
            {
                ModelState.AddModelError("produto.Invalido", "Produto da informática com preço abaixo do permitido.");
            }
            if (ModelState.IsValid)
            {
                ProdutosDAO dao = new ProdutosDAO();
                dao.Adiciona(produto);
                //Redireciona para outra action RedirectToAction("Index", "Controller")
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Produto = produto;
                CategoriasDAO categoriasDAO = new CategoriasDAO();
                ViewBag.Categorias = categoriasDAO.Lista();
                return View("FormCadastraProduto");
            }
        }

        [Route("produtos/{id}", Name="DetalhesDoProduto")]
        public ActionResult Visualiza(int id)
        {
            ProdutosDAO produtosDAO = new ProdutosDAO();
            Produto produto = produtosDAO.BuscaPorId(id);
            ViewBag.Produto = produto;
            return View();
        }
    }
}