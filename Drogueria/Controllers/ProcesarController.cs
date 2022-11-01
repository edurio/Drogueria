using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace Drogueria.Controllers
{
    public class ProcesarController : Controller
    {
        // GET: Procesar
        public ActionResult Index(int id)
        {
            //Cargar las relaciones de los productos
            Entidades.Filtro filtroR = new Entidades.Filtro();
            filtroR.Est_Id = SessionH.Usuario.Est_id;
            var listaRelaciones = DAL.RLProductosDAL.ObtenerProductosRelacionados(filtroR);

            Drogueria.Models.SolicitudDetalleModel model = new Models.SolicitudDetalleModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Solicitud_Id = id;
            List<Entidades.DetalleSolicitud> detalleSolicitud = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);


            foreach(var a in detalleSolicitud)
            {
                foreach(var b in listaRelaciones)
                {
                    if (a.Producto_Id == b.Prod_Id_Drogueria)
                    {
                        a.ProductoStr = b.Descripcion_Drogueria;
                        break;
                    }
                }
            }

            model.lista = detalleSolicitud;

            return View(model);
        }

        public JsonResult ObtenerEtiqueta(string numero)
        {
            string[] split = numero.Split(new Char[] { '/','-' });

            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Numero = int.Parse(split[0]);
            filtro.Año = int.Parse(split[1]);
            filtro.EmpId = SessionH.Usuario.EmpId;

            var lista = DAL.EtiquetaDAL.Obtener(filtro);
            if (lista.Count > 0)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            
        }

        
    }
}