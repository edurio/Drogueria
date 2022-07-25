using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleSolicitud
    {
        public int Id { get; set; }
        public int Solicitud_Id { get; set; }
        public int Producto_Id { get; set; }
        public string ProductoStr { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
    }
}
