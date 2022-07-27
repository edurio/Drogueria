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
    public class EstadoDAL
    {
        public static List<Entidades.Estado> ObtenerEstado()
        {
            List<Entidades.Estado> lista = new List<Entidades.Estado>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_SOES_SOLICITUD_WEB_ESTADO");

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int ESTADO = reader.GetOrdinal("ESTADO");

                while (reader.Read())
                {
                    Entidades.Estado OBJ = new Entidades.Estado();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Estado_Solicitud = (String)(!reader.IsDBNull(ESTADO) ? reader.GetValue(ESTADO) : string.Empty);
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
