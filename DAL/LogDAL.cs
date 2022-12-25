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
    }
}
