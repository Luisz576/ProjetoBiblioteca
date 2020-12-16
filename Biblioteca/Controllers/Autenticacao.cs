using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if(!string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")) && !string.IsNullOrEmpty(controller.HttpContext.Session.GetString("senha")))
            {
                if(!isLoginValid(controller.HttpContext.Session.GetString("user"), controller.HttpContext.Session.GetString("senha")))
                    controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }else
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
        }
        public static void CheckLoginLikeAdm(Controller controller)
        {   
            if(!string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")) && !string.IsNullOrEmpty(controller.HttpContext.Session.GetString("senha")))
            {
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
            return true;
        }

        private static bool isLoginValidLikeAdm(string login, string senha){
            return true;
        }

    }
}