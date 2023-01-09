using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drogueria.Controllers
{
    public class VideosController : Controller
    {
        // GET: Videos
        public ActionResult Index()
        {
            Drogueria.Models.LoginModel modelo = new Models.LoginModel();
            modelo.ListaVideos = DAL.VideoDAL.ObtenerVideos();
            return View(modelo);
        }
    }
}