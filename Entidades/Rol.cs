using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Rol
    {
        public int Id { get; set; }

        public int Contador { get; set; }
        public int RL_Usr_Rol_Id { get; set; }
        public int Usr_Id { get; set; }
        public string RolStr { get; set; }
        public bool Eliminado { get; set; }
    }
}
