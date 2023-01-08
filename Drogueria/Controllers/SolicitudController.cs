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
    public class SolicitudController : Controller
    {
        // GET: LibroSolicitudes
        public ActionResult Index(string limpiar, int? id, int? sl)
        {
            //sp:solo lectura

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
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.Desde = Utiles.FechaObtenerMinimo(DateTime.Now);
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);
                modelo.lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);
                modelo.SolicitudCargada = false;
                Session["registrosEncontrados"] = modelo.lista;
            }
            if (Session["registrosEncontrados"] == null && limpiar != null)
            {
                filtro.EmpId = SessionH.Usuario.EmpId;
                Session["FiltroInformeDesde"] = Utiles.ReversaFecha(DateTime.Now);
                Session["FiltroInformeHasta"] = Utiles.ReversaFecha(DateTime.Now);
                filtro.Desde = Utiles.FechaObtenerMinimo(DateTime.Now);
                filtro.Hasta = Utiles.FechaObtenerMaximo(DateTime.Now);
                modelo.lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);
                modelo.SolicitudCargada = false;
                Session["registrosEncontrados"] = modelo.lista;
            }

            if (Session["listaProductosCargados"] != null)
            {
                modelo.listaProductosExternos = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;

                modelo.SolicitudCargada = true;
                Entidades.Filtro filtro2 = new Entidades.Filtro();
                filtro2.Est_Id = SessionH.Usuario.Est_id;
                var rlProd = DAL.RLProductosDAL.ObtenerProductosRelacionados(filtro2);

                int cantidad = 0;
                foreach (var a in modelo.listaProductosExternos)
                {
                    bool encontrado = false;
                    foreach (var b in rlProd)
                    {
                        if (a.Id_Externo == b.Prod_Id_Externo)
                        {
                            encontrado = true;
                            break;
                        }
                    }
                    if (encontrado == false)
                    {
                        cantidad++;
                        a.SinRelacionar = true;
                    }
                    if (encontrado == true)
                    {
                        a.SinRelacionar = false;
                    }
                }
                modelo.EsRayen = true;
                
                modelo.CantidadNoRelacionada = cantidad;

            }

            if (id != null)
            {
                Session["idSolicitud"] = id;
            }

            if (sl == 1)
            {
                modelo.SoloLectura = true;
            }

            Session["EsRayen"] = modelo.EsRayen;
            return View(modelo);
        }

        public JsonResult ObtenerProductos(int id)
        {
            var lista = DAL.ProductoDAL.Obtener(new Entidades.Filtro() { EmpId = SessionH.Usuario.EmpId, ClasId = id });
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RecuperarSolicitud()
        {
            if (Session["idSolicitud"] != null)
            {
                int id = int.Parse(Session["idSolicitud"].ToString());
                Entidades.Filtro filtro = new Entidades.Filtro();
                filtro.Id = filtro.Solicitud_Id = id;
                var solicitud = DAL.SolicitudDAL.ObtenerSolicitud(filtro);
                var detalleSolicitud = DAL.DetalleSolicitudDAL.ObtenerDetalleSolicitud(filtro);

                solicitud[0].DetalleSolicitud = detalleSolicitud;
                Session["ListaProductos"] = detalleSolicitud;

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = solicitud[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {                
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "no", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
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
                //SI ESTO OCURRE ES UNA MODIFICACION
                var textoLog = "Crea solicitud N°";
                if (Session["idSolicitud"] != null)
                {
                    entity.Id = int.Parse(Session["idSolicitud"].ToString());
                    textoLog = "Modifica solicitud N°";
                }

                if (entity.Id == 0)
                {
                    Entidades.Filtro filtro = new Entidades.Filtro();
                    var numeroVenta = CorrelativoSolicitud(filtro);
                    entity.Folio = int.Parse(Session["numeroSolicitud"].ToString());
                }

                entity.Usr_Id = SessionH.Usuario.Id;
                entity.Emp_Id = SessionH.Usuario.EmpId;
                entity.Estado_Id = 2;
                entity.Es_Rayen = bool.Parse(Session["EsRayen"].ToString());
                entity.EstId = SessionH.Usuario.Est_id;
                entity.Establecimiento = SessionH.Establecimiento.Descripcion;

                entity = DAL.SolicitudDAL.InsertarSolicitud(entity);



                List<Entidades.DetalleSolicitud> listadoProductos = Session["ListaProductos"] as List<Entidades.DetalleSolicitud>;

                if (Session["listaProductosCargados"] != null)
                {
                    List<Entidades.ProductoExterno> listadoProductosCargados = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;
                    foreach (var a in listadoProductosCargados)
                    {
                        Entidades.DetalleSolicitud listado = new Entidades.DetalleSolicitud();
                        listado.Producto_Id = a.ProdId;
                        listado.ProductoStr = a.Descripcion;
                        listado.ProductoNuevo = true;
                        listado.Unidad = a.Unidad;
                        listado.Factor = a.FactorSeguridad;
                        listado.Consumo = a.Consumo;
                        listado.Cantidad = a.Solicitado;
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

                //Reportes.rptSolicitud rptSolicitud = new Reportes.rptSolicitud();
                //rptSolicitud.Cargar(entity, listadoProductos);
                //rptSolicitud.CreateDocument(true);

                //var ruta = ConfigurationSettings.AppSettings.Get("RutaPDF_Fisica") + "Solicitud_N°" + entity.Folio.ToString() + "_Empresa_ID" + SessionH.Usuario.EmpId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
                //rptSolicitud.ExportToPdf(ruta, null);

                var ruta = Utiles.ObtenerPDF(entity, listadoProductos);


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

                //LOG
                var Log = new Entidades.Log() { Modulo="Solicitud", Descripcion= textoLog + entity.Folio.ToString(), Usr_Id = SessionH.Usuario.Id };
                DAL.LogDAL.InsertarLog(Log);

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
            List<Entidades.Solicitud> lista = Session["registrosEncontrados"] as List<Entidades.Solicitud>;

            string[] columns = { "FechaMostrar", "Folio", "Prioridad", "Estado", "Observacion_Solicitud", "UsuarioCreador" };
            byte[] filecontent = Code.ExcelExportHelper.ExportExcel(lista, "Listado de solicitudes", true, columns);
            return File(filecontent, Code.ExcelExportHelper.ExcelContentType, "listaSolicitudes_" + timestamp + ".xlsx");

        }
        public JsonResult EnviarSolicitud(int idSolicitud)
        {
            DAL.SolicitudDAL.EnviarSolicitud(idSolicitud);

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult LimpiarSolicitud()
        {
            Session["listaProductosCargados"] = null;
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertarRelacionProducto(Entidades.RLProductos entity)
        {
            try
            {
                entity.Est_Id = SessionH.Usuario.Est_id;
                entity = DAL.RLProductosDAL.InsertarRelacionProducto(entity);

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "exito", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult CambiarFactor(int id, string factor)
        {
            factor = factor.Replace(".", ",");
            decimal factorDec = decimal.Parse(factor.ToString());
            List<Entidades.ProductoExterno> lista = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;
            foreach (var a in lista)
            {
                if (a.Id == id)
                {
                    a.FactorSeguridad = factorDec;
                    var soli = decimal.Parse(a.Consumo.ToString()) * factorDec;

                    string[] textSplit = soli.ToString().Split(new Char[] { ',' });
                    string numeroEntero = textSplit[0];
                    int nuevaCantidad = int.Parse(numeroEntero);

                    a.Solicitado = nuevaCantidad;
                    break;
                }
            }

            Session["listaProductosCargados"] = lista;


            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EliminarNoRelacionados()
        {
            List<Entidades.ProductoExterno> lista = Session["listaProductosCargados"] as List<Entidades.ProductoExterno>;
            List<Entidades.ProductoExterno> listaRetorno = new List<Entidades.ProductoExterno>();

            foreach(var a in lista)
            {
                if (a.SinRelacionar == false)
                {
                    listaRetorno.Add(a);
                }
            }

            Session["listaProductosCargados"] = listaRetorno;

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "ok", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}