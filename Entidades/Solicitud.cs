﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Solicitud
    {
        public int Id { get; set; }
        public int Emp_Id { get; set; }
        public string Empresa { get; set; }
        public int Usr_Id { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
        public string FechaMostrar
        {
            get
            {
                return Fecha_Ingreso.ToShortDateString();
            }
        }
        public int Folio { get; set; }
        public int Prioridad_Id { get; set; }
        public string Prioridad { get; set; }
        public int Estado_Id { get; set; }

        public int Tipo_Id { get; set; }
        public string Estado { get; set; }
        public string Observacion_Solicitud { get; set; }
        public bool Nula { get; set; }
        public bool Eliminado { get; set; }
        public bool Es_Modificacion { get; set; }
        public string UsuarioCreador { get; set; }

        public string Tipo 
        {
            get
            {
                if (Tipo_Id == 1)
                    return "Mensual";

                if (Tipo_Id == 2)
                    return "Crítico";

                return string.Empty;
            }
        }
    }
}
