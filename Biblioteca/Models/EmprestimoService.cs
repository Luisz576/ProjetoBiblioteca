using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public bool Inserir(Emprestimo e)
        {
            if(validate_emprestimo(e)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    bc.Emprestimos.Add(e);
                    bc.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool Atualizar(Emprestimo e)
        {
            if(validate_emprestimo(e)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    try{
                        Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                        emprestimo.NomeUsuario = e.NomeUsuario;
                        emprestimo.Telefone = e.Telefone;
                        emprestimo.LivroId = e.LivroId;
                        emprestimo.DataEmprestimo = e.DataEmprestimo;
                        emprestimo.DataDevolucao = e.DataDevolucao;
                        emprestimo.Devolvido = e.Devolvido;

                        bc.SaveChanges();
                        return true;
                    }catch{
                        return false;
                    }
                }
            }
            return false;
        }

        public List<Emprestimo> ListarTodos(FiltrosEmprestimos filtro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                List<Emprestimo> query;
                
                if(filtro != null)
                {
                    switch(filtro.TipoFiltro)
                    {
                        case "Usuario":
                            query = bc.Emprestimos.Where(e => e.NomeUsuario.ToLower().Contains(filtro.Filtro.ToLower())).OrderByDescending(e => e.DataDevolucao).Include(e => e.Livro).ToList();
                        break;

                        case "Livro":
                            query = bc.Emprestimos.Where(e => e.Livro.Titulo.ToLower().Contains(filtro.Filtro.ToLower())).OrderByDescending(e => e.DataDevolucao).Include(e => e.Livro).ToList();
                        break;

                        default:
                            query = bc.Emprestimos.Include(e => e.Livro).ToList();
                        break;
                    }
                }
                else
                {
                    query = bc.Emprestimos.Include(e => e.Livro).ToList();
                }
                
                return query;
            }
        }

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }

        private bool validate_emprestimo(Emprestimo e){
            return (!string.IsNullOrEmpty(e.NomeUsuario) && !string.IsNullOrEmpty(e.Telefone));
        }

    }
}