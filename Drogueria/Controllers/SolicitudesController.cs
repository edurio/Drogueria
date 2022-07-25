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
    public class SolicitudesController : Controller
    {
        // GET: Solicitudes
        public ActionResult Index()
        {
            var text = DateTime.Now.ToString();
            Session["FechaActual"] = text;
            Session["ListaProductos"] = new List<Entidades.DetalleSolicitud>();
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
        public JsonResult ObtenerEstado()
        {
            var lista = DAL.EstadoDAL.ObtenerEstado();
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult InsertarSolicitud(Entidades.Solicitud entity)
        {
            try
            {
                Entidades.Filtro filtro = new Entidades.Filtro();
                var numeroVenta = CorrelativoSolicitud(filtro);

                entity.Usr_Id = SessionH.Usuario.Id;
                entity.Emp_Id = SessionH.Usuario.EmpId;
                entity.Folio = int.Parse(Session["numeroSolicitud"].ToString());
                entity = DAL.SolicitudDAL.InsertarSolicitud(entity);

                List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;
                foreach (var detalle in listadoProductos)
                {
                    detalle.Solicitud_Id = entity.Id;
                    DAL.DetalleSolicitudDAL.InsertarDetalleSolicitud(detalle);

                }

                Reportes.rptSolicitud rptSolicitud = new Reportes.rptSolicitud();
                rptSolicitud.Cargar(entity, listadoProductos);
                rptSolicitud.CreateDocument(true);

                var ruta = ConfigurationSettings.AppSettings.Get("RutaPDF_Fisica") + "Solicitud_N°" + entity.Folio.ToString() + "Empresa_ID" + SessionH.Usuario.EmpId + ".pdf";
                rptSolicitud.ExportToPdf(ruta, null);

                var url = "";

                if (ConfigurationSettings.AppSettings.Get("Ambiente") == "CER")
                {
                    url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_CER") + "Solicitud_N°" + entity.Folio.ToString() + "Empresa_ID" + SessionH.Usuario.EmpId + ".pdf";
                }
                else
                {
                    url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_PROD") + "Solicitud_N°" + entity.Folio.ToString() + "Empresa_ID" + SessionH.Usuario.EmpId + ".pdf";
                }

                Session["ListaProductos"] = new List<Entidades.DetalleSolicitud>();

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = url, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult CorrelativoSolicitud(Entidades.Filtro entity)
        {
            var Numero = 0;
            entity.EmpId = SessionH.Usuario.EmpId;
            List<Entidades.Solicitud> lista =DAL.SolicitudDAL.ObtenerCorrelativo(entity);
            if (lista.Count == 0)
            {
                Numero = 1;
            }
            else
            {
                Numero = lista[0].Folio + 1;
            }

            Session["numeroSolicitud"] = Numero;

            if (Numero == 0)
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = Numero, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult AgregarProducto(Entidades.DetalleSolicitud entity)
        {
            try
            {
                if (Session["ListaProductos"] == null)
                {
                    List<Entidades.DetalleSolicitud> listadoProductos = new List<Entidades.DetalleSolicitud>();
                    listadoProductos.Add(entity);
                    Session["ListaProductos"] = listadoProductos;
                }
                else
                {
                    List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;
                    listadoProductos.Add(entity);
                    Session["ListaProductos"] = listadoProductos;
                }

                //ControlStock.DAL.FacturaDAL.InsertarFactura(entity);
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "exito", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


        }
    }
}