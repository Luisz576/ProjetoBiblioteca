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
            else{
                if(string.IsNullOrEmpty(u.Senha))
                    u.Senha = usuarioService.ObterPorId(u.Id).Senha;
                usuarioService.Atualizar(u);
            }
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string filtro)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            UsuarioService usuarioService = new UsuarioService();
            return View(usuarioService.ListarTodos(filtro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            UsuarioService us = new UsuarioService();
            Usuario user = us.ObterPorId(id);
            user.Senha = null;
            return View(user);
        }

        public IActionResult Deletar(int id)
        {
            Autenticacao.CheckLoginLikeAdm(this);
            UsuarioService us = new UsuarioService();
            us.Deletar(id);
            return RedirectToAction("Listagem");
        }

    }
}