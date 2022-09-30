using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public int Est_id { get; set; }

        public bool EsDrogueria 
        { 
            get
            {
                if (Est_id == 0)
                {
                    return true;
                }

                return false;

            }
        }
    }
}
