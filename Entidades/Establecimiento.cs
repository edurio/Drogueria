using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Establecimiento
    {
        public int Id { get; set; }
        public int Emp_Id { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }
    }
}
