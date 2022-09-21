using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Configuration;

namespace Drogueria.Controllers
{
    public class MantenedorProductosController : Controller
    {
        // GET: MantenedorProductos
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ObtenerUnidades()
        {
            var lista = DAL.UnidadDAL.ObtenerUnidades();
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerEstablecimiento()
        {
            var lista = DAL.EstablecimientoDAL.ObtenerEstablecimiento(new Entidades.Filtro() { EmpId = 49 });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}