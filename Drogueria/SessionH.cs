using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drogueria
{
    public class SessionH
    {
        private const string VAR_LOGUEADO = "logueado";
        private const string VAR_USUARIO = "usuario";
        private const string VAR_ESTABLECIMIENTO = "";

        private static T Lee<T>(string variable)
        {
            object valor = HttpContext.Current.Session[variable];
            if (valor == null)
                return default(T);
            else return ((T)valor);
        }

        private static void Escribe(string variable, object valor)
        {
            HttpContext.Current.Session[variable] = valor;
        }

        public static bool Logueado
        {
            get { return Lee<bool>(VAR_LOGUEADO); }
            set { Escribe(VAR_LOGUEADO, value); }
        }

        public static Entidades.Usuario Usuario
        {
            get { return Lee<Entidades.Usuario>(VAR_USUARIO); }
            set { Escribe(VAR_USUARIO, value); }
        }


        public static Entidades.Establecimiento Establecimiento
        {
            get { return Lee<Entidades.Establecimiento>(VAR_ESTABLECIMIENTO); }
            set { Escribe(VAR_ESTABLECIMIENTO, value); }
        }
    }
}