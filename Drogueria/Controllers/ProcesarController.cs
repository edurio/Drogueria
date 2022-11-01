using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}