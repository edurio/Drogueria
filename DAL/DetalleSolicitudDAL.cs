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
    public class DetalleSolicitudDAL
    {
        public static Entidades.DetalleSolicitud InsertarDetalleSolicitud(Entidades.DetalleSolicitud detalleSolicitud)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_DETALLE_INS");

            db.AddInParameter(dbCommand, "SOLICITUD_ID", DbType.Int32, detalleSolicitud.Solicitud_Id != 0 ? detalleSolicitud.Solicitud_Id : (object)null);
            db.AddInParameter(dbCommand, "PROD_ID", DbType.Int32, detalleSolicitud.Producto_Id != 0 ? detalleSolicitud.Producto_Id : (object)null);
            db.AddInParameter(dbCommand, "CANTIDAD", DbType.Int32, detalleSolicitud.Cantidad != 0 ? detalleSolicitud.Cantidad : (object)null);
            db.AddInParameter(dbCommand, "OBSERVACION", DbType.String, detalleSolicitud.Observacion != "" ? detalleSolicitud.Observacion : (object)null);

            db.ExecuteNonQuery(dbCommand);

            return detalleSolicitud;

        }
    }
}
