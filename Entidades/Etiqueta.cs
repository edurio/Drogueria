using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Etiqueta
    {
        public int Id { get; set; }

        public int Año { get; set; }
        public int ProdId { get; set; }
        public string Numero { get; set; }
        public string Lote { get; set; }

        public string Articulo { get; set; }

        public DateTime FechaVencimiento { get; set; }
        public decimal Cantidad { get; set; }


        public string FechaVencimientoStr 
        { 
            get
            {
                return FechaVencimiento.ToShortDateString();
            }
        }

    }
}
