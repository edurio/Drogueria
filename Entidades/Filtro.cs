using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Filtro
    {
        public int EmpId { get; set; }
        public int Id { get; set; }

        public int ClasId { get; set; }
        public int Solicitud_Id { get; set; }
        public int Estado_Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public int Est_Id { get; set; }
        public string Descripcion { get; set; }
    }
}
