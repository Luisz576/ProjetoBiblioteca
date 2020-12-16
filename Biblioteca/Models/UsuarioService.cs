using System.Linq;
using System.Collections.Generic;

namespace Biblioteca.Models
{
    public class UsuarioService
    {

        public bool Inserir(Usuario user){
            if(validate_usuario(user)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    user.Senha = MD5Criptografia.EncodeMD5(user.Senha);
                    user.isAdm = false; //GARANTIR
                    bc.Usuarios.Add(user);
                    bc.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool Atualizar(Usuario user){
            if(validate_usuario(user)){
                using(BibliotecaContext bc = new BibliotecaContext())
                {
                    Usuario usuario = bc.Usuarios.Find(user.Id);
                    usuario.User = user.User;
                    usuario.Senha = MD5Criptografia.EncodeMD5(user.Senha);
                    user.isAdm = ObterPorId(user.Id).isAdm;
                    bc.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void Deletar(int id){
            using(BibliotecaContext bc = new BibliotecaContext())
                {
                    Usuario user = ObterPorId(id);
                    if(!user.isAdm){
                        bc.Usuarios.Remove(user);
                        bc.SaveChanges();
                    }
                }
        }

        public List<Usuario> ListarTodos(string filtro){
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> query;
                
                if(filtro != null)
                    query = bc.Usuarios.Where(u => u.User.Contains(filtro));
                else
                    query = bc.Usuarios;
                
                return query.OrderBy(u => u.User).ToList();
            }
        }

        public Usuario ObterPorId(int id){
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        public bool validate_usuario(Usuario user){
            return !string.IsNullOrEmpty(user.User) && !string.IsNullOrEmpty(user.Senha);
        }

    }
}