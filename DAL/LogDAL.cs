using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.Common;


namespace DAL
{
    public class LogDAL
    {
        public static Entidades.Log InsertarLog(Entidades.Log log)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_LOG_INS");

          
            db.AddInParameter(dbCommand, "MODULO", DbType.String, log.Modulo);
            db.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, log.Descripcion);
            db.AddInParameter(dbCommand, "USR_ID", DbType.Int32, log.Usr_Id);


            db.ExecuteNonQuery(dbCommand);

            return log;
        }


        public static List<Entidades.Log> ObtenerLog(Entidades.Filtro filtro)
        {
            List<Entidades.Log> lista = new List<Entidades.Log>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_LOG_READ");
           
            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int FECHA_HORA = reader.GetOrdinal("FECHA_HORA");
                int MODULO = reader.GetOrdinal("MODULO");
                int DESCRIPCION = reader.GetOrdinal("DESCRIPCION");
                int USUARIO = reader.GetOrdinal("USUARIO");


                while (reader.Read())
                {
                    Entidades.Log OBJ = new Entidades.Log();
                    //BeginFields
                    OBJ.Fecha = (DateTime)(!reader.IsDBNull(FECHA_HORA) ? reader.GetValue(FECHA_HORA) : DateTime.MinValue);
                    OBJ.Modulo = (String)(!reader.IsDBNull(MODULO) ? reader.GetValue(MODULO) : string.Empty);
                    OBJ.Descripcion = (String)(!reader.IsDBNull(DESCRIPCION) ? reader.GetValue(DESCRIPCION) : string.Empty);
                    OBJ.Usuario = (String)(!reader.IsDBNull(USUARIO) ? reader.GetValue(USUARIO) : string.Empty);



                    //EndFields

                    lista.Add(OBJ);
                }
            }
            catch (Exception ex)
            {
                //GlobalesDAO.InsertErrores(ex);
                throw ex;
            }
            finally
            {
                reader.Close();
            }

            return lista;

        }
    }
}
