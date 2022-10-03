using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ProductoExterno
    {
        public int Id { get; set; }
        public int Id_Externo { get; set; }
        public int Est_Id { get; set; }
        public string Establecimiento { get; set; }
        public string Descripcion { get; set; }
        public int Consumo { get; set; }
        public decimal FactorSeguridad { get; set; }
        public int Unid_Id { get; set; }
        public string Unidad { get; set; }
        public bool Eliminado { get; set; }
        public bool SinRelacionar { get; set; }
    }
}
