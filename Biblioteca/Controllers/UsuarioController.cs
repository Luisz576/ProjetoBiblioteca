using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLoginLikeAdm(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            UsuarioService usuarioService = new UsuarioService();
            ViewData["Erro"] = null;
            if(u.Id == 0)
                usuarioService.Inserir(u);
            else
                usuarioService.Atualizar(u);
            
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            return View();
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            return View();
        }

    }
}