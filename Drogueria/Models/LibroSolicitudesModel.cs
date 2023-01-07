using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drogueria.Models
{
    public class LibroSolicitudesModel
    {
        public List<Entidades.Solicitud> lista;

        public bool SolicitudCargada;

        public int CantidadNoRelacionada;

        public List<Entidades.ProductoExterno> listaProductosExternos;

        public bool SoloLectura;

        public bool EsRayen;
    }
}