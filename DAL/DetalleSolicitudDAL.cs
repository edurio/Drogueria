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
            db.AddInParameter(dbCommand, "NUEVO", DbType.Byte, detalleSolicitud.ProductoNuevo == true ? 1 : 0);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, detalleSolicitud.Eliminado == true ? 1 : 0);

            db.ExecuteNonQuery(dbCommand);

            return detalleSolicitud;

        }
        public static List<Entidades.DetalleSolicitud> ObtenerDetalleSolicitud(Entidades.Filtro filtro)
        {
            List<Entidades.DetalleSolicitud> detalleSolicitud = new List<Entidades.DetalleSolicitud>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_DETALLE_LEER");

            db.AddInParameter(dbCommand, "SOLICITUD_ID", DbType.Int32, filtro.Solicitud_Id != 0 ? filtro.Solicitud_Id : (object)null);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int SOLICITUD_ID = reader.GetOrdinal("SOLICITUD_ID");
                int PROD_ID = reader.GetOrdinal("PROD_ID");
                int PRODUCTO = reader.GetOrdinal("PRODUCTO");
                int CANTIDAD = reader.GetOrdinal("CANTIDAD");
                int OBSERVACION = reader.GetOrdinal("OBSERVACION");
                int NUEVO = reader.GetOrdinal("NUEVO");

                while (reader.Read())
                {
                    Entidades.DetalleSolicitud OBJ = new Entidades.DetalleSolicitud();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Solicitud_Id = (int)(!reader.IsDBNull(SOLICITUD_ID) ? reader.GetValue(SOLICITUD_ID) : 0);
                    OBJ.Producto_Id = (int)(!reader.IsDBNull(PROD_ID) ? reader.GetValue(PROD_ID) : 0);
                    OBJ.ProductoStr = (String)(!reader.IsDBNull(PRODUCTO) ? reader.GetValue(PRODUCTO) : string.Empty);
                    OBJ.Cantidad = (int)(!reader.IsDBNull(CANTIDAD) ? reader.GetValue(CANTIDAD) : 0);
                    OBJ.Observacion = (String)(!reader.IsDBNull(OBSERVACION) ? reader.GetValue(OBSERVACION) : string.Empty);
                    OBJ.ProductoNuevo = (bool)(!reader.IsDBNull(NUEVO) ? reader.GetValue(NUEVO) : false);
                    //EndFields

                    detalleSolicitud.Add(OBJ);
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



            return detalleSolicitud;

        }
        public static void EliminarProducto(int id)
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_DETALLE_DEL");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, id);


            db.ExecuteNonQuery(dbCommand);

        }
    }
}
