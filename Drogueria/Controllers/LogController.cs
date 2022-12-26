using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drogueria.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            Models.LogModel modelo = new Models.LogModel();
            modelo.ListaLog = DAL.LogDAL.ObtenerLog(new Entidades.Filtro());

            Session["registrosEncontradosLog"] = modelo.ListaLog;
            return View(modelo);
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