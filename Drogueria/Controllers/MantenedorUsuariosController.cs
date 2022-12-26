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
    public class MantenedorUsuariosController : Controller
    {
        // GET: MantenedorUsuarios
        public ActionResult Index()
        {
            Drogueria.Models.MantenedorUsuariosModel modelo = new Models.MantenedorUsuariosModel();
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.EmpId = SessionH.Usuario.EmpId;
            filtro.TraeRoles = true;
            modelo.lista = DAL.UsuarioDAL.ObtenerUsuarios(filtro);
            Session["registrosEncontrados"] = modelo.lista;
            return View(modelo);
        }
        public JsonResult ObtenerRoles()
        {
            var lista = DAL.RolDAL.ObtenerRoles();
            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerEstablecimientoDrogueria()
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.EmpId = SessionH.Usuario.EmpId;
            var lista = DAL.EstablecimientoDAL.ObtenerEstablecimientoDrogueria(filtro);

            if (lista == null || lista.Count == 0)
                return new JsonResult() { ContentEncoding = Encoding.Default, Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GuardarUsuario(Entidades.Usuario entity, string idsRoles)
        {
            entity.EmpId = SessionH.Usuario.EmpId;
            DAL.UsuarioDAL.GuardaUsuario(entity);
            var idUsuario = entity.Id;

            List<int> ids = PreparaRoles(idsRoles);
            int contador = 1;
            foreach (var a in ids)
            {
                Entidades.Rol rol = new Entidades.Rol();
                rol.Usr_Id = idUsuario;
                rol.Id = a;
                rol.Contador = contador;
                contador++;
                DAL.RolDAL.InsertarRolUsuario(rol);
            }

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "exito", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        List<int> PreparaRoles(string idsRoles)
        {
            string[] split = idsRoles.Split(new Char[] { ',' });
            List<int> retorno = new List<int>();

            foreach (var a in split)
            {
                int v;
                if (int.TryParse(a, out v))
                {
                    retorno.Add(int.Parse(a));
                }
            }
            return retorno;
        }
        public JsonResult ObtenerUsuario(int id)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = id;
            filtro.TraeRoles = true;
            var lista = DAL.UsuarioDAL.ObtenerUsuarios(filtro);

            if (lista.Count == 1)
            {

                return new JsonResult() { ContentEncoding = Encoding.Default, Data = lista[0], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult() { ContentEncoding = Encoding.Default, Data = "noexiste", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public FileContentResult ExportToExcel()
        {
            var timestamp = Utiles.ObtenerTimeStamp();
            List<Entidades.Usuario> lista = Session["registrosEncontrados"] as List<Entidades.Usuario>;

            string[] columns = { "Id", "Nombre", "Correo", "Username", "Establecimiento", "Roles_Usuario" };
            byte[] filecontent = Code.ExcelExportHelper.ExportExcel(lista, "Listado de usuarios", true, columns);
            return File(filecontent, Code.ExcelExportHelper.ExcelContentType, "listaUsuarios_" + timestamp + ".xlsx");

        }
    }
}