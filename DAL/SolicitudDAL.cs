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
    public class SolicitudDAL
    {
        public static List<Entidades.Solicitud> ObtenerCorrelativo(Entidades.Filtro filtro)
        {
            List<Entidades.Solicitud> solicitud = new List<Entidades.Solicitud>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_CORRELATIVO_LEER");

            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId != 0 ? filtro.EmpId : (object)null);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int FOLIO = reader.GetOrdinal("FOLIO");

                while (reader.Read())
                {
                    Entidades.Solicitud OBJ = new Entidades.Solicitud();
                    //BeginFields
                    OBJ.Folio = (int)(!reader.IsDBNull(FOLIO) ? reader.GetValue(FOLIO) : 0);
                    //EndFields

                    solicitud.Add(OBJ);
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



            return solicitud;

        }
        public static Entidades.Solicitud InsertarSolicitud(Entidades.Solicitud solicitud)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_INS");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, solicitud.Id != 0 ? solicitud.Id : (object)null);
            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, solicitud.Emp_Id != 0 ? solicitud.Emp_Id : (object)null);
            db.AddInParameter(dbCommand, "USUARIO_ID", DbType.Int32, solicitud.Usr_Id != 0 ? solicitud.Usr_Id : (object)null);
            db.AddInParameter(dbCommand, "FECHA", DbType.DateTime, solicitud.Fecha_Ingreso != DateTime.MinValue ? solicitud.Fecha_Ingreso : (object)null);
            db.AddInParameter(dbCommand, "FOLIO", DbType.Int32, solicitud.Folio != 0 ? solicitud.Folio : (object)null);
            db.AddInParameter(dbCommand, "PRIO_ID", DbType.Int32, solicitud.Prioridad_Id != 0 ? solicitud.Prioridad_Id : (object)null);
            db.AddInParameter(dbCommand, "SOES_ID", DbType.Int32, solicitud.Estado_Id != 0 ? solicitud.Estado_Id : (object)null);
            db.AddInParameter(dbCommand, "OBSERVACION", DbType.String, solicitud.Observacion_Solicitud != null ? solicitud.Observacion_Solicitud : (object)null);
            db.AddInParameter(dbCommand, "NULA", DbType.Byte, solicitud.Nula == true ? 1 : 0);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, solicitud.Eliminado == true ? 1 : 0);

            solicitud.Id = int.Parse(db.ExecuteScalar(dbCommand).ToString());

            return solicitud;
        }
        public static List<Entidades.Solicitud> ObtenerSolicitud(Entidades.Filtro filtro)
        {
            List<Entidades.Solicitud> solicitud = new List<Entidades.Solicitud>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOLICITUD_LEER");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, filtro.Id != 0 ? filtro.Id : (object)null);
            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId != 0 ? filtro.EmpId : (object)null);
            db.AddInParameter(dbCommand, "FECHA_DESDE", DbType.DateTime, filtro.Desde != DateTime.MinValue ? filtro.Desde : (object)null);
            db.AddInParameter(dbCommand, "FECHA_HASTA", DbType.DateTime, filtro.Hasta != DateTime.MinValue ? filtro.Hasta : (object)null);


            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int EMP_ID = reader.GetOrdinal("EMP_ID");
                int ALIAS = reader.GetOrdinal("ALIAS");
                int USUARIO_ID = reader.GetOrdinal("USUARIO_ID");
                int NOMBRE = reader.GetOrdinal("NOMBRE");
                int FECHA = reader.GetOrdinal("FECHA");
                int FOLIO = reader.GetOrdinal("FOLIO");
                int PRIO_ID = reader.GetOrdinal("PRIO_ID");
                int PRIORIDAD = reader.GetOrdinal("PRIORIDAD");
                int SOES_ID = reader.GetOrdinal("SOES_ID");
                int ESTADO = reader.GetOrdinal("ESTADO");
                int OBSERVACION = reader.GetOrdinal("OBSERVACION");

                while (reader.Read())
                {
                    Entidades.Solicitud OBJ = new Entidades.Solicitud();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Emp_Id = (int)(!reader.IsDBNull(EMP_ID) ? reader.GetValue(EMP_ID) : 0);
                    OBJ.Empresa = (String)(!reader.IsDBNull(ALIAS) ? reader.GetValue(ALIAS) : string.Empty);
                    OBJ.Usr_Id = (int)(!reader.IsDBNull(USUARIO_ID) ? reader.GetValue(USUARIO_ID) : 0);
                    OBJ.UsuarioCreador = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
                    OBJ.Fecha_Ingreso = (DateTime)(!reader.IsDBNull(FECHA) ? reader.GetValue(FECHA) : DateTime.MinValue);
                    OBJ.Folio = (int)(!reader.IsDBNull(FOLIO) ? reader.GetValue(FOLIO) : 0);
                    OBJ.Prioridad_Id = (int)(!reader.IsDBNull(PRIO_ID) ? reader.GetValue(PRIO_ID) : 0);
                    OBJ.Prioridad = (String)(!reader.IsDBNull(PRIORIDAD) ? reader.GetValue(PRIORIDAD) : string.Empty);
                    OBJ.Estado_Id = (int)(!reader.IsDBNull(SOES_ID) ? reader.GetValue(SOES_ID) : 0);
                    OBJ.Estado = (String)(!reader.IsDBNull(ESTADO) ? reader.GetValue(ESTADO) : string.Empty);
                    OBJ.Observacion_Solicitud = (String)(!reader.IsDBNull(OBSERVACION) ? reader.GetValue(OBSERVACION) : string.Empty);
                    //EndFields

                    solicitud.Add(OBJ);
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



            return solicitud;

        }
    }
}
