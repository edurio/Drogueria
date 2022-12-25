using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;

namespace Drogueria.Controllers
{
    public class MantenedorRelacionProductosController : Controller
    {
        // GET: MantenedorRelacionProductos
        public ActionResult Index()
        {
            Drogueria.Models.MantenedorRelacionProductosModel modelo = new Models.MantenedorRelacionProductosModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Est_Id = SessionH.Usuario.Est_id;
            modelo.lista = DAL.RLProductosDAL.ObtenerProductosRelacionados(filtro);
            return View(modelo);
        }
        public JsonResult ObtenerProductos()
        {
            var lista = DAL.ProductoDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId});
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerProductoExterno()
        {
            var lista = DAL.ProductoExternoDAL.ObtenerProductoExterno(new Entidades.Filtro() { Est_Id = SessionH.Usuario.Est_id });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult InsertarRelacionProducto(Entidades.RLProductos entity)
        {
            try
            {
                entity.Est_Id = SessionH.Usuario.Est_id;
                entity =  DAL.RLProductosDAL.InsertarRelacionProducto(entity);

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "exito", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult ObtenerProductosRelacionados(int id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = id;
            var lista = DAL.RLProductosDAL.ObtenerProductosRelacionados(filtro);

            if (lista.Count == 1)
            {

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "noexiste", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}