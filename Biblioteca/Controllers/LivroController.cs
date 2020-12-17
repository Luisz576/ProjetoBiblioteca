using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Livro l)
        {
            Autenticacao.CheckLogin(this);
            LivroService livroService = new LivroService();
            ViewData["Erro"] = null;
            if(l.Id == 0)
                livroService.Inserir(l);
            else
                livroService.Atualizar(l);
            
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(int page, string tipoFiltro, string filtro)
        {
            Autenticacao.CheckLogin(this);
            FiltrosLivros objFiltro = null;
            ViewBag.pageAtual = page > 1 ? page : 1;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosLivros();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            LivroService livroService = new LivroService();
            ViewBag.Filtro = string.IsNullOrEmpty(filtro) ? null : filtro;
            ViewBag.TipoFiltro = string.IsNullOrEmpty(tipoFiltro) ? null : tipoFiltro;
            return View(livroService.ListarTodos(objFiltro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            LivroService ls = new LivroService();
            Livro l = ls.ObterPorId(id);
            return View(l);
        }
    }
}