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

        public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Livro> query;
                
                if(filtro != null)
                    switch(filtro.TipoFiltro)
                    {
                        case "Autor":
                            query = bc.Livros.Where(l => l.Autor.Contains(filtro.Filtro));
                        break;

                        case "Titulo":
                            query = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Livros;
                        break;
                    }
                else
                    query = bc.Livros;
                
                return query.OrderBy(l => l.Titulo).ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    bc.Livros
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)) )
                    .ToList();
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