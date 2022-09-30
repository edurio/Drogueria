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
    public class UnidadDAL
    {
        public static Entidades.Unidad InsertarUnidad(Entidades.Unidad unidad)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_UNID_UNIDADES_INS");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, unidad.Id);
            db.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, unidad.Descripcion != null ? unidad.Descripcion : (object)null);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, unidad.Eliminado == true ? 1 : 0);

            unidad.Id = int.Parse(db.ExecuteScalar(dbCommand).ToString());

            return unidad;
        }
        public static List<Entidades.Unidad> ObtenerUnidades()
        {
            List<Entidades.Unidad> listaUnidades = new List<Entidades.Unidad>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_UNID_UNIDADES_LEER");

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int DESCRIPCION = reader.GetOrdinal("DESCRIPCION");

                while (reader.Read())
                {
                    Entidades.Unidad OBJ = new Entidades.Unidad();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Descripcion = (String)(!reader.IsDBNull(DESCRIPCION) ? reader.GetValue(DESCRIPCION) : string.Empty);
                    //EndFields

                    listaUnidades.Add(OBJ);
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



            return listaUnidades;

        }
    }
}
