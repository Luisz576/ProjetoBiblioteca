using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class LivroService
    {
        public bool Inserir(Livro l)
        {
            if(validate_livro(l)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    bc.Livros.Add(l);
                    bc.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool Atualizar(Livro l)
        {
            if(validate_livro(l)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    Livro livro = bc.Livros.Find(l.Id);
                    livro.Autor = l.Autor;
                    livro.Titulo = l.Titulo;
                    livro.Ano = l.Ano;
                    bc.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                List<Livro> query;
                
                if(filtro != null)
                    switch(filtro.TipoFiltro)
                    {
                        case "Autor":
                            query = bc.Livros.Where(l => l.Autor.Contains(filtro.Filtro)).ToList();
                        break;

                        case "Titulo":
                            query = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro)).ToList();
                        break;

                        default:
                            query = bc.Livros.ToList();
                        break;
                    }
                else
                    query = bc.Livros.ToList();
                
                return query.OrderBy(l => l.Id).Reverse().ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return
                    bc.Livros
                    .Where(l => (!(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)))).OrderBy(e => e.Titulo)
                    .ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis(Emprestimo emprestimo)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                ICollection<Livro> livrosDisponiveis = bc.Livros
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)) )
                    .ToList();
                if(!livrosDisponiveis.Contains(ObterPorId(emprestimo.LivroId)))
                    livrosDisponiveis.Add(ObterPorId(emprestimo.LivroId));
                return livrosDisponiveis.OrderBy(l => l.Titulo).ToList();
            }
        }

        public Livro ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Livros.Find(id);
            }
        }

        private bool validate_livro(Livro l){
            return (
                l.Titulo != null &&
                l.Titulo.Trim() != "" &&
                l.Autor != null &&
                l.Autor.Trim() != ""
            );
        }

    }
}