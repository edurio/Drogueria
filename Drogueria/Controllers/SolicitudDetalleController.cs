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
    public class SolicitudDetalleController : Controller
    {
        // GET: SolicitudDetalle
        public ActionResult Index(string limpiar)
        {
            Drogueria.Models.SolicitudDetalleModel modelo = new Models.SolicitudDetalleModel();
            Entidades.Filtro filtro = new Entidades.Filtro();

            if (Session["registrosEncontrados"] != null && limpiar == null)
            {
                Session["FiltroInformeDesde"] = Session["FiltroInformeDesde"];
                Session["FiltroInformeHasta"] = Session["FiltroInformeHasta"];
                modelo.lista = Session["registrosEncontrados"] as List<Entidades.DetalleSolicitud>;
            }
            if (Session["registrosEncontrados"] != null && limpiar != null)
            {
                
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.EmpId = SessionH.Usuario.EmpId;
                filtro.Estado_Id = 1;
                filtro.Desde = Utiles.FechaObtenerMinimo(DateTime.Now);
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);
                modelo.lista = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);
                Session["registrosEncontrados"] = modelo.lista;
            }
            if (Session["registrosEncontrados"] == null && limpiar != null)
            {
                
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.EmpId = SessionH.Usuario.EmpId;
                filtro.Estado_Id = 1;
                filtro.Desde = Utiles.FechaObtenerMinimo(DateTime.Now);
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);
                modelo.lista = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);
                Session["registrosEncontrados"] = modelo.lista;
            }

            return View(modelo);
        }
        public ActionResult BusquedaFiltro(Entidades.Filtro entity)
        {
            entity.Desde = Utiles.FechaObtenerMinimo(entity.Desde);
            entity.Hasta = Utiles.FechaObtenerMaximo(entity.Hasta);
            entity.EmpId = SessionH.Usuario.EmpId;
            entity.Estado_Id = 1;
            List<Entidades.DetalleSolicitud> historicosEncontrados = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(entity);
            Session["FiltroInformeDesde"] = Utiles.ReversaFecha(entity.Desde);
            Session["FiltroInformeHasta"] = Utiles.ReversaFecha(entity.Hasta);
            Session["registrosEncontrados"] = historicosEncontrados;

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "OK", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public FileContentResult ExportToExcel()
        {
            var timestamp = Utiles.ObtenerTimeStamp();
            List<Entidades.DetalleSolicitud> lista = Session["registrosEncontrados"] as List<Entidades.DetalleSolicitud>;

            string[] columns = { "FolioSolicitud", "FechaMostrar", "Prioridad", "ProductoStr", "Cantidad", "Observacion" };
            byte[] filecontent = Code.ExcelExportHelper.ExportExcel(lista, "Listado de solicitudes por detalle", true, columns);
            return File(filecontent, Code.ExcelExportHelper.ExcelContentType, "listaDetalleSolicitudes_" + timestamp + ".xlsx");

        }
    }
}