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
            Drogueria.Models.MantenedorProductosModel modelo = new Models.MantenedorProductosModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            var text = DateTime.Now.ToString();
            Session["FechaActual"] = text;

            filtro.Est_Id = SessionH.Usuario.Est_id;
            modelo.lista = DAL.ProductoExternoDAL.ObtenerProductoExterno(filtro);

            return View(modelo);
        }
        public JsonResult ObtenerUnidades()
        {
            var lista = DAL.UnidadDAL.ObtenerUnidades();
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult InsertarProductoExterno(Entidades.ProductoExterno entity)
        {
            try
            {
                entity.Est_Id = 1;
                entity = DAL.ProductoExternoDAL.InsertarProductoExterno(entity);

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult RecuperaProducto(int id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = id;
            var lista = DAL.ProductoExternoDAL.ObtenerProductoExterno(filtro);

            if (lista.Count == 1)
            {

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "noexiste", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}