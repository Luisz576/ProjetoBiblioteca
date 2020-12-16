using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if(!string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")) && !string.IsNullOrEmpty(controller.HttpContext.Session.GetString("senha")))
            {
                tryUseSistemLikeAdm(controller);
                if(!isLoginValid(controller.HttpContext.Session.GetString("user"), controller.HttpContext.Session.GetString("senha")))
                    controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }else
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
        }
        public static void CheckLoginLikeAdm(Controller controller)
        {   
            if(!string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")) && !string.IsNullOrEmpty(controller.HttpContext.Session.GetString("senha")))
            {
                tryUseSistemLikeAdm(controller);
                if(!isLoginValidLikeAdm(controller.HttpContext.Session.GetString("user"), controller.HttpContext.Session.GetString("senha"))){
                    if(!isLoginValid(controller.HttpContext.Session.GetString("user"), controller.HttpContext.Session.GetString("senha")))
                        controller.Request.HttpContext.Response.Redirect("/Home/Login");
                    else
                        controller.Request.HttpContext.Response.Redirect("/Home/Index");
                }
            }else
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
        }

        public static bool isLoginValid(string login, string senha){
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                foreach(Usuario user in bc.Usuarios){
                    if(user.User == login)
                        if(MD5Criptografia.EncodeMD5(senha) == user.Senha)
                            return true;
                }
            }
            return false;
        }

        public static bool isLoginValidLikeAdm(string login, string senha){
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                foreach(Usuario user in bc.Usuarios){
                    if(user.isAdm)
                        if(user.User == login)
                            if(MD5Criptografia.EncodeMD5(senha) == user.Senha)
                                return true;
                }
            }
            return false;
        }

        private static void tryUseSistemLikeAdm(Controller controller){
            controller.ViewBag.isAdm = false;
            if(!string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")) && !string.IsNullOrEmpty(controller.HttpContext.Session.GetString("senha")))
                controller.ViewBag.isAdm = Autenticacao.isLoginValidLikeAdm(controller.HttpContext.Session.GetString("user"), controller.HttpContext.Session.GetString("senha"));
        }

    }
}