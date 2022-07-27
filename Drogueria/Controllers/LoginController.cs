using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Drogueria.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ValidaUsuario(Entidades.Filtro entity)
        {
            var lista = DAL.UsuarioDAL.ValidarUsuario(entity);
            if (lista.Count == 1)
            {
                SessionH.Usuario = lista[0];
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
           

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult AccionSalir()
        {
            SessionH.Usuario = null;
            SessionH.Logueado = false;
            return RedirectToAction("Index", "Login");
        }
    }
}