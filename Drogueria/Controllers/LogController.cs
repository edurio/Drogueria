using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Drogueria.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
            Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);


            Models.LogModel modelo = new Models.LogModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Desde = Utiles.FechaObtenerMinimo(DateTime.Now);
            filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);

            modelo.ListaLog = DAL.LogDAL.ObtenerLog(filtro);

            Session["registrosEncontradosLog"] = modelo.ListaLog;
            return View(modelo);
        }

        public ActionResult BusquedaFiltro(Entidades.Filtro entity)
        {
            entity.Desde = Utiles.FechaObtenerMinimo(entity.Desde);
            entity.Hasta = Utiles.FechaObtenerMaximo(entity.Hasta);
            entity.EmpId = SessionH.Usuario.EmpId;
            List<Entidades.Solicitud> historicosEncontrados = DAL.SolicitudDAL.ObtenerSolicitud(entity);
            Session["FiltroInformeDesde"] = Utiles.ReversaFecha(entity.Desde);
            Session["FiltroInformeHasta"] = Utiles.ReversaFecha(entity.Hasta);
            Session["registrosEncontrados"] = historicosEncontrados;

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "OK", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public FileContentResult ExportToExcel()
        {
            var timestamp = Utiles.ObtenerTimeStamp();
            List<Entidades.Log> lista = Session["registrosEncontradosLog"] as List<Entidades.Log>;

            string[] columns = { "Fecha", "Modulo", "Descripcion", "Usuario"};
            byte[] filecontent = Code.ExcelExportHelper.ExportExcel(lista, "Log del sistema", true, columns);
            return File(filecontent, Code.ExcelExportHelper.ExcelContentType, "listaLog_" + timestamp + ".xlsx");

        }
    }
}