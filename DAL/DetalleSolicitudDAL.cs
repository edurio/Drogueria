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
            db.AddInParameter(dbCommand, "CONSUMO", DbType.Int32, detalleSolicitud.Consumo != 0 ? detalleSolicitud.Consumo : (object)null);
            db.AddInParameter(dbCommand, "FACTOR", DbType.Decimal, detalleSolicitud.Factor != 0 ? detalleSolicitud.Factor : (object)null);
            db.AddInParameter(dbCommand, "CANTIDAD", DbType.Int32, detalleSolicitud.Cantidad != 0 ? detalleSolicitud.Cantidad : (object)null);
            db.AddInParameter(dbCommand, "UNIDAD", DbType.String, detalleSolicitud.Unidad != "" ? detalleSolicitud.Unidad : (object)null);
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

            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId != 0 ? filtro.EmpId : (object)null);
            db.AddInParameter(dbCommand, "ESTADO_ID", DbType.Int32, filtro.Estado_Id != 0 ? filtro.Estado_Id : (object)null);
            db.AddInParameter(dbCommand, "SOLICITUD_ID", DbType.Int32, filtro.Solicitud_Id != 0 ? filtro.Solicitud_Id : (object)null);
            db.AddInParameter(dbCommand, "FECHA_DESDE", DbType.DateTime, filtro.Desde != DateTime.MinValue ? filtro.Desde : (object)null);
            db.AddInParameter(dbCommand, "FECHA_HASTA", DbType.DateTime, filtro.Hasta != DateTime.MinValue ? filtro.Hasta : (object)null);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int SOLICITUD_ID = reader.GetOrdinal("SOLICITUD_ID");
                int PROD_ID = reader.GetOrdinal("PROD_ID");
                int PRODUCTO = reader.GetOrdinal("PRODUCTO");
                int CONSUMO = reader.GetOrdinal("CONSUMO");
                int FACTOR = reader.GetOrdinal("FACTOR");
                int CANTIDAD = reader.GetOrdinal("CANTIDAD");
                int OBSERVACION = reader.GetOrdinal("OBSERVACION");
                int NUEVO = reader.GetOrdinal("NUEVO");
                int FOLIO = reader.GetOrdinal("FOLIO");
                int FECHA = reader.GetOrdinal("FECHA");
                int PRIORIDAD = reader.GetOrdinal("PRIORIDAD");
                int UNIDAD = reader.GetOrdinal("UNIDAD");

                while (reader.Read())
                {
                    Entidades.DetalleSolicitud OBJ = new Entidades.DetalleSolicitud();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Solicitud_Id = (int)(!reader.IsDBNull(SOLICITUD_ID) ? reader.GetValue(SOLICITUD_ID) : 0);
                    OBJ.Producto_Id = (int)(!reader.IsDBNull(PROD_ID) ? reader.GetValue(PROD_ID) : 0);
                    OBJ.ProductoStr = (String)(!reader.IsDBNull(PRODUCTO) ? reader.GetValue(PRODUCTO) : string.Empty);
                    OBJ.Consumo = (int)(!reader.IsDBNull(CONSUMO) ? reader.GetValue(CONSUMO) : 0);
                    OBJ.Factor = (decimal)(!reader.IsDBNull(FACTOR) ? reader.GetValue(FACTOR) : Decimal.Parse("0"));
                    OBJ.Cantidad = (int)(!reader.IsDBNull(CANTIDAD) ? reader.GetValue(CANTIDAD) : 0);
                    OBJ.Observacion = (String)(!reader.IsDBNull(OBSERVACION) ? reader.GetValue(OBSERVACION) : string.Empty);
                    OBJ.ProductoNuevo = (bool)(!reader.IsDBNull(NUEVO) ? reader.GetValue(NUEVO) : false);
                    OBJ.FolioSolicitud = (int)(!reader.IsDBNull(FOLIO) ? reader.GetValue(FOLIO) : 0);
                    OBJ.Fecha_Ingreso = (DateTime)(!reader.IsDBNull(FECHA) ? reader.GetValue(FECHA) : DateTime.MinValue);
                    OBJ.Prioridad = (String)(!reader.IsDBNull(PRIORIDAD) ? reader.GetValue(PRIORIDAD) : string.Empty);
                    OBJ.Unidad = (String)(!reader.IsDBNull(UNIDAD) ? reader.GetValue(UNIDAD) : string.Empty);
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
