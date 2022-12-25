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

                //Me traigo el establecimiento
                var establecimiento = DAL.EstablecimientoDAL.ObtenerEstablecimiento(new Entidades.Filtro() { Id = lista[0].Est_id });
                SessionH.Establecimiento = establecimiento[0];

                //LOG
                var Log = new Entidades.Log() { Modulo = "Login", Descripcion = "Se logea en el sistema", Usr_Id = SessionH.Usuario.Id };
                DAL.LogDAL.InsertarLog(Log);

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