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
    public class LibroSolicitudesController : Controller
    {
        // GET: LibroSolicitudes
        public ActionResult Index()
        {
            Drogueria.Models.LibroSolicitudesModel modelo = new Models.LibroSolicitudesModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
            Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
            filtro.EmpId = SessionH.Usuario.EmpId;
            modelo.lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);

            var text = DateTime.Now.ToString();
            Session["FechaActual"] = text;
            Session["ListaProductos"] = new List<Entidades.DetalleSolicitud>();

            return View(modelo);
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
        public JsonResult ObtenerSolicitud(int id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = id;
            var lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);

            if (lista == null || lista.Count == 0)
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerDetalleSolicitud(Entidades.Filtro entity)
        {
            List<Entidades.DetalleSolicitud> detalleSolicitud = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(entity);
            Session["ListaProductos"] = detalleSolicitud;

            if (detalleSolicitud == null || detalleSolicitud.Count == 0)
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = detalleSolicitud, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult InsertarSolicitud(Entidades.Solicitud entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    Entidades.Filtro filtro = new Entidades.Filtro();
                    var numeroVenta = CorrelativoSolicitud(filtro);
                    entity.Folio = int.Parse(Session["numeroSolicitud"].ToString());
                }

                entity.Usr_Id = SessionH.Usuario.Id;
                entity.Emp_Id = SessionH.Usuario.EmpId;
                entity = DAL.SolicitudDAL.InsertarSolicitud(entity);

                List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;
                
                foreach (var detalle in listadoProductos)
                {
                    if (detalle.ProductoNuevo == true)
                    {
                        detalle.Solicitud_Id = entity.Id;
                        detalle.ProductoNuevo = false;
                        DAL.DetalleSolicitudDAL.InsertarDetalleSolicitud(detalle);
                    }
                }

                Reportes.rptSolicitud rptSolicitud = new Reportes.rptSolicitud();
                rptSolicitud.Cargar(entity, listadoProductos);
                rptSolicitud.CreateDocument(true);

                var ruta = ConfigurationSettings.AppSettings.Get("RutaPDF_Fisica") + "Solicitud_N°" + entity.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
                rptSolicitud.ExportToPdf(ruta, null);

                var url = "";

                if (ConfigurationSettings.AppSettings.Get("Ambiente") == "CER")
                {
                    url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_CER") + "Solicitud_N°" + entity.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
                }
                else
                {
                    url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_PROD") + "Solicitud_N°" + entity.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
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
            List<Entidades.Solicitud> lista = DAL.SolicitudDAL.ObtenerCorrelativo(entity);
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
        public JsonResult QuitarProducto(Entidades.DetalleSolicitud entity)
        {
            try
            {
                List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;
                foreach (var detalle in listadoProductos)
                {
                    if (entity.Producto_Id == detalle.Producto_Id)
                    {
                        if (detalle.ProductoNuevo == false)
                        {
                            var id = detalle.Id;
                            DAL.DetalleSolicitudDAL.EliminarProducto(id);
                        }
                        var indice = entity.Indice;
                        listadoProductos.RemoveAt(indice);
                        Session["ListaProductos"] = listadoProductos;
                        //ControlStock.DAL.FacturaDAL.InsertarFactura(entity);
                        return new JsonResult() { ContentEncoding = Encoding.Default, Data = listadoProductos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
               

                //ControlStock.DAL.FacturaDAL.InsertarFactura(entity);
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = listadoProductos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}