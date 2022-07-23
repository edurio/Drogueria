using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;


namespace Drogueria.Controllers
{
    public class SolicitudesController : Controller
    {
        // GET: Solicitudes
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult ObtenerProductos()
        {
            var lista = DAL.ProductoDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId });            
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ObtenerPrioridad()
        {
            var lista = DAL.PrioridadDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}