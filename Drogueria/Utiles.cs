﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drogueria
{
    public class Utiles
    {
        public static int FechaToInteger(DateTime date)
        {
            string a = date.Year.ToString();
            string b = date.Month.ToString("00");
            string c = date.Day.ToString("00");
            string d = a + b + c;

            if (d == "10101")
                d = "0";

            return int.Parse(d);
        }
        public static string ReversaFecha(DateTime fecha)
        {
            string yy = fecha.Year.ToString("0000");
            string mm = fecha.Month.ToString("00");
            string dd = fecha.Day.ToString("00");

            return yy + "-" + mm + "-" + dd;

        }
        public static DateTime FechaObtenerMinimo(DateTime fecha)
        {
            int year = fecha.Year;
            int mes = fecha.Month;
            int day = fecha.Day;

            return new DateTime(year, mes, day, 0, 0, 0);
        }
        public static DateTime FechaObtenerMaximo(DateTime fecha)
        {
            int year = fecha.Year;
            int mes = fecha.Month;
            int day = fecha.Day;

            return new DateTime(year, mes, day, 23, 59, 59);
        }
        public static string ObtenerTimeStamp()
        {
            string yy = DateTime.Now.Year.ToString("0000");
            string mm = DateTime.Now.Month.ToString("00");
            string dd = DateTime.Now.Day.ToString("00");
            string hh = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            return yy + mm + dd + hh + mn + ss;

        }
        public static int ObtieneRut_INT(string rut)
        {
            if (rut.Trim().Length == 0)
                return 0;
            int rutDevuelto = 0;

            if (rut.Trim().Length == 8)
            {
                rutDevuelto = int.Parse(rut.Substring(0, 7));
            }
            if (rut.Trim().Length == 9)
            {
                rutDevuelto = int.Parse(rut.Substring(0, 8));
            }
            if (rut.Trim().Length == 10)
            {
                rutDevuelto = int.Parse(rut.Substring(0, 8));
            }

            return rutDevuelto;
        }
        public static string FormateaRut(string rut)
        {
            rut = rut.Trim();
            string rutDevuelto = string.Empty;
            if (rut.Length == 8)
            {
                rut = "0" + rut;
                rutDevuelto = rut.Substring(0, 8) + "-" + rut.Substring(8, 1);
            }
            if (rut.Length == 9)
            {
                if (rut.IndexOf("-") > 0)
                {
                    rutDevuelto = "0" + rut;

                }
                else
                {
                    rutDevuelto = rut.Substring(0, 8) + "-" + rut.Substring(8, 1);
                }
            }
            if (rut.Length == 10)
            {
                rutDevuelto = rut;
            }
            return rutDevuelto;
        }
    }
}