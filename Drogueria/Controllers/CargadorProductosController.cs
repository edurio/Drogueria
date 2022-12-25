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
    public class CargadorProductosController : Controller
    {
        // GET: CargadorProductos
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                file.SaveAs(path);
                ProcesarExcel(path);
            }
            
            return RedirectToAction("Index", "Solicitud");
        }


        void ProcesarExcel(string ruta)
        {
            //Cargar las relaciones de los productos
            Entidades.Filtro filtroR = new Entidades.Filtro();
            filtroR.Est_Id = SessionH.Usuario.Est_id;
            var listaRelaciones= DAL.RLProductosDAL.ObtenerProductosRelacionados(filtroR);

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ruta, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count + 1;
            int colCount = xlRange.Columns.Count;

            Entidades.Filtro filtro = new Entidades.Filtro();
            List<Entidades.ProductoExterno> productoExternos = new List<Entidades.ProductoExterno>();
            filtro.Est_Id = SessionH.Usuario.Est_id;
            productoExternos = DAL.ProductoExternoDAL.ObtenerProductoExterno(filtro);

            List<Entidades.Unidad> unidades = new List<Entidades.Unidad>();
            unidades = DAL.UnidadDAL.ObtenerUnidades();

            List<Entidades.ProductoExterno> listaProductosLeidos = new List<Entidades.ProductoExterno>();

            for (int i = 12; i <= rowCount; i++)
            {
                if (xlWorksheet.Cells[i, 1].Value != null)
                {
                    var idExterno = xlWorksheet.Cells[i, 1].Value;
                    var descripcion = xlWorksheet.Cells[i, 2].Value;
                    var consumo = xlWorksheet.Cells[i, 4].Value;
                    var unidad = xlWorksheet.Cells[i, 7].Value;

                    bool productoEncontrado = false;

                    //Reviso la relación con los productos externos/drogueria
                    int idProductoDrogueria = 0;
                    foreach (var lista in listaRelaciones)
                    {
                        if (lista.Prod_Id_Externo == idExterno)
                        {
                            idProductoDrogueria = lista.Prod_Id_Drogueria;
                            break;
                        }

                    }

                    foreach (var lista in productoExternos)
                    {
                        if (lista.Descripcion.Trim().ToUpper() == descripcion.Trim().ToUpper())
                        {
                            productoEncontrado = true;
                            break;
                        }

                    }

                    bool unidadEncontrada = false;
                    var idUnidad = 0;

                    foreach (var listaUnidades in unidades)
                    {
                        if (listaUnidades.Descripcion.Trim().ToUpper() == unidad.Trim().ToUpper())
                        {
                            unidadEncontrada = true;
                            idUnidad = listaUnidades.Id;
                            break;
                        }

                    }

                    if (unidadEncontrada == false)
                    {
                        Entidades.Unidad unidadNueva = new Entidades.Unidad();
                        unidadNueva.Descripcion = unidad;
                        Entidades.Unidad nuevaUnidad = DAL.UnidadDAL.InsertarUnidad(unidadNueva);
                        idUnidad = nuevaUnidad.Id;
                    }



                    decimal proyectado = decimal.Parse(consumo.ToString()) * SessionH.Establecimiento.FactorRiesgo;
                    string numtext = proyectado.ToString();
                    string[] textSplit = numtext.Split(new Char[] { ','});
                    string numeroEntero = textSplit[0];
                    int nuevaCantidad = int.Parse(numeroEntero);

                    if (productoEncontrado == false)
                    {
                        Entidades.ProductoExterno productoExterno = new Entidades.ProductoExterno();
                        
                        productoExterno.ProdId = idProductoDrogueria;
                        productoExterno.Id_Externo = int.Parse(idExterno.ToString());
                        productoExterno.Descripcion = descripcion;
                        productoExterno.Est_Id = SessionH.Usuario.Est_id;
                        productoExterno.Unid_Id = idUnidad;
                        productoExterno.Unidad = unidad;
                        productoExterno.Consumo = int.Parse(consumo.ToString());
                        productoExterno.FactorSeguridad = SessionH.Establecimiento.FactorRiesgo;
                        productoExterno.Solicitado = int.Parse(nuevaCantidad.ToString());
                        productoExterno.SinRelacionar = false;
                        Entidades.ProductoExterno productoNuevo = DAL.ProductoExternoDAL.InsertarProductoExterno(productoExterno);
                        productoExterno.Id = i;

                        productoExternos.Add(productoNuevo);
                        listaProductosLeidos.Add(productoNuevo);
                    }
                    else
                    {
                        Entidades.ProductoExterno productoExterno = new Entidades.ProductoExterno();
                        productoExterno.Id = i;
                        productoExterno.ProdId = idProductoDrogueria;
                        productoExterno.Id_Externo = int.Parse(idExterno.ToString());
                        productoExterno.Descripcion = descripcion;
                        productoExterno.Est_Id = SessionH.Usuario.Est_id;
                        productoExterno.Unid_Id = idUnidad;
                        productoExterno.Unidad = unidad;
                        productoExterno.SinRelacionar = false;
                        productoExterno.Consumo = int.Parse(consumo.ToString());
                        productoExterno.FactorSeguridad = SessionH.Establecimiento.FactorRiesgo;
                        productoExterno.Solicitado = int.Parse(nuevaCantidad.ToString());
                        listaProductosLeidos.Add(productoExterno);
                    }
                }
            }
            xlWorkbook.Close();
            xlApp.Quit();


            Session["listaProductosCargados"] = listaProductosLeidos;

        }
    }
}