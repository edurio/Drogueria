using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RLProductos
    {
        public int Id { get; set; }
        public int Est_Id { get; set; }
        public int Prod_Id_Drogueria { get; set; }
        public string Descripcion_Drogueria { get; set; }
        public int Prod_Id_Externo { get; set; }
        public string Descripcion_Externa { get; set; }

    }
}
