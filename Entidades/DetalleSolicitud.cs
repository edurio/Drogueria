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
        public int FolioSolicitud { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
        public string FechaMostrar
        {
            get
            {
                return Fecha_Ingreso.ToShortDateString();
            }
        }
        public string Prioridad { get; set; }
        public int Producto_Id { get; set; }
        public string ProductoStr { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
        public bool ProductoNuevo { get; set; }
        public int Indice { get; set; }
        public bool Eliminado { get; set; }
        public string Unidad { get; set; }
    }
}
