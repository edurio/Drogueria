﻿using System;
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
    public class LibroSolicitudesController : Controller
    {
        // GET: LibroSolicitudes
        public ActionResult Index(string limpiar)
        {
            Drogueria.Models.LibroSolicitudesModel modelo = new Models.LibroSolicitudesModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            var text = DateTime.Now.ToString();
            Session["FechaActual"] = text;
            Session["ListaProductos"] = new List<Entidades.DetalleSolicitud>();

            if (Session["registrosEncontrados"] != null && limpiar == null)
            {
                Session["FiltroInformeDesde"] = Session["FiltroInformeDesde"];
                Session["FiltroInformeHasta"] = Session["FiltroInformeHasta"];
                modelo.lista = Session["registrosEncontrados"] as List<Entidades.Solicitud>;
                modelo.SolicitudCargada = false;
            }
            if (Session["registrosEncontrados"] != null && limpiar != null)
            {
                filtro.EmpId = SessionH.Usuario.EmpId;
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.Desde = Utiles.FechaObtenerMinimo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);

                //Si soy Administrador veo todo, sino, solo veo las de mi establecimiento
                if (!SessionH.Usuario.TieneRol(Entidades.Enumerados.Rol.Administrador))
                {
                    filtro.Est_Id = SessionH.Usuario.Est_id;
                }

                modelo.lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);
                modelo.SolicitudCargada = false;
                Session["registrosEncontrados"] = modelo.lista;
            }
            if (Session["registrosEncontrados"] == null && limpiar != null)
            {
                filtro.EmpId = SessionH.Usuario.EmpId;
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.Desde = Utiles.FechaObtenerMinimo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);
                //Si soy Administrador veo todo, sino, solo veo las de mi establecimiento
                if (!SessionH.Usuario.TieneRol(Entidades.Enumerados.Rol.Administrador))
                {
                    filtro.Est_Id = SessionH.Usuario.Est_id;
                }

                modelo.lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);
                modelo.SolicitudCargada = false;
                Session["registrosEncontrados"] = modelo.lista;
            }

            if (Session["listaProductosCargados"] != null)
            {
                modelo.SolicitudCargada = true;
                modelo.listaProductosExternos = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;
            }

            Session["idSolicitud"] = null;
            return View(modelo);
        }
        public JsonResult ObtenerProductos(int id)
        {
            var lista = DAL.ProductoDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId, ClasId = id });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerPrioridad()
        {
            var lista = DAL.PrioridadDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerClases()
        {
            var lista = DAL.ClaseDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId });
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
                entity.Estado_Id = 2;
                entity = DAL.SolicitudDAL.InsertarSolicitud(entity);

                

                List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;

                if (Session["listaProductosCargados"] != null)
                {
                    List<Entidades.ProductoExterno> listadoProductosCargados = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;
                    foreach (var a in listadoProductosCargados)
                    {
                        Entidades.DetalleSolicitud listado = new Entidades.DetalleSolicitud();
                        listado.Producto_Id = a.Id_Externo;
                        listado.ProductoStr = a.Descripcion;
                        listado.ProductoNuevo = true;
                        listado.Unidad = a.Unidad;
                        listado.Cantidad = a.Consumo;
                        listadoProductos.Add(listado);
                    }
                }

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
                Session["listaProductosCargados"] = null;

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
                            //DAL.DetalleSolicitudDAL.EliminarProducto(id);
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
        public ActionResult BusquedaFiltro(Entidades.Filtro entity)
        {
            entity.Desde = Utiles.FechaObtenerMinimo(entity.Desde);
            entity.Hasta = Utiles.FechaObtenerMaximo(entity.Hasta);
            entity.EmpId = SessionH.Usuario.EmpId;

            //Si soy Administrador veo todo, sino, solo veo las de mi establecimiento
            if (!SessionH.Usuario.TieneRol(Entidades.Enumerados.Rol.Administrador))
            {
                entity.Est_Id = SessionH.Usuario.Est_id;
            }

            List<Entidades.Solicitud> historicosEncontrados = DAL.SolicitudDAL.ObtenerSolicitud(entity);
            Session["FiltroInformeDesde"] = Utiles.ReversaFecha(entity.Desde);
            Session["FiltroInformeHasta"] = Utiles.ReversaFecha(entity.Hasta);
            Session["registrosEncontrados"] = historicosEncontrados;

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "OK", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public FileContentResult ExportToExcel()
        {
            var timestamp = Utiles.ObtenerTimeStamp();
            List<Entidades.Solicitud> lista = Session["registrosEncontrados"] as List<Entidades.Solicitud>;

            string[] columns = { "FechaMostrar", "Folio", "Prioridad", "Estado", "Observacion_Solicitud", "UsuarioCreador" };
            byte[] filecontent = Code.ExcelExportHelper.ExportExcel(lista, "Listado de solicitudes", true, columns);
            return File(filecontent, Code.ExcelExportHelper.ExcelContentType, "listaSolicitudes_" + timestamp + ".xlsx");

        }
        public JsonResult EnviarSolicitud(int idSolicitud)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = idSolicitud;
            filtro.Solicitud_Id = idSolicitud;
            var lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro)[0];
            List<Entidades.DetalleSolicitud> detalleSolicitud = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);
            lista.Estado_Id = 1;
            lista.Estado = "Enviada";

            var rutaPDF = Utiles.ObtenerPDF(lista, detalleSolicitud);


            DAL.SolicitudDAL.EnviarSolicitud(idSolicitud);

            Mensajeria.EnviarConfirmarEmail("eduardo.rios@erex.cl", "Eduardo Ríos", "Solicitud " + lista.Tipo + " N°" + lista.Folio.ToString(), lista.Observacion_Solicitud, rutaPDF);

            //LOG
            var Log = new Entidades.Log() { Modulo = "Solicitud", Descripcion = "Se envía la solicitud N°" + lista.Folio.ToString(), Usr_Id = SessionH.Usuario.Id };
            DAL.LogDAL.InsertarLog(Log);

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Imprimir(int id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = id;
            filtro.Solicitud_Id = id;
            var solicitud = DAL.SolicitudDAL.ObtenerSolicitud(filtro)[0];
            List<Entidades.DetalleSolicitud> detalleSolicitud = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);

            if (!solicitud.Es_Rayen)
            {
                var rutaPDF = Utiles.ObtenerPDF(solicitud, detalleSolicitud);
            }
            else
            {
                var rutaPDF = Utiles.ObtenerPDF_Rayen(solicitud, detalleSolicitud);
            }


            var url = "";
            if (ConfigurationSettings.AppSettings.Get("Ambiente") == "CER")
            {
                url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_CER") + "Solicitud_N°" + solicitud.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
            }
            else
            {
                url = ConfigurationSettings.AppSettings.Get("RutaPDF_Url_PROD") + "Solicitud_N°" + solicitud.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
            }

            //LOG
            var Log = new Entidades.Log() { Modulo = "Solicitud", Descripcion = "Imprime PDF la solicitud N°" + solicitud.Folio.ToString(), Usr_Id = SessionH.Usuario.Id };
            DAL.LogDAL.InsertarLog(Log);

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = url, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult Eliminar(int Id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = Id;
            filtro.Solicitud_Id = Id;
            var solicitud = DAL.SolicitudDAL.ObtenerSolicitud(filtro)[0];

            DAL.SolicitudDAL.Eliminar(Id);

            //LOG
            var Log = new Entidades.Log() { Modulo = "Solicitud", Descripcion = "Se elimina la solicitud N°" + solicitud.Folio.ToString(), Usr_Id = SessionH.Usuario.Id };
            DAL.LogDAL.InsertarLog(Log);
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

    }
}