using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Biblioteca.Controllers
{
    
    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarDisponiveis();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            Autenticacao.CheckLogin(this);
            EmprestimoService emprestimoService = new EmprestimoService();
            ViewBag.erro = null;
            if(viewModel.Emprestimo.Id == 0){
                if(!emprestimoService.Inserir(viewModel.Emprestimo)){
                    ViewBag.erro = "Algum campo está vazio";
                }
            }else{
                if(!emprestimoService.Atualizar(viewModel.Emprestimo)){
                    ViewBag.erro = "Erro ao atualizar";
                }
            }
            if(string.IsNullOrEmpty(ViewBag.erro)){
                return RedirectToAction("Listagem");
            }else{
                LivroService livroService = new LivroService();
                CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
                cadModel.Emprestimo = viewModel.Emprestimo;
                cadModel.Livros = livroService.ListarDisponiveis(viewModel.Emprestimo);
                return View(cadModel);
            }
        }

        public IActionResult Listagem(int page, string tipoFiltro, string filtro)
        {
            Autenticacao.CheckLogin(this);
            ViewBag.pageAtual = page > 1 ? page : 1;
            FiltrosEmprestimos objFiltro = null;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            EmprestimoService emprestimoService = new EmprestimoService();
            ViewBag.dataNow = DateTime.Now;
            ViewBag.Filtro = string.IsNullOrEmpty(filtro) ? null : filtro;
            ViewBag.TipoFiltro = string.IsNullOrEmpty(tipoFiltro) ? null : tipoFiltro;
            return View(emprestimoService.ListarTodos(objFiltro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Emprestimo = e;
            cadModel.Livros = livroService.ListarDisponiveis(e);
            
            return View(cadModel);
        }
    }
}