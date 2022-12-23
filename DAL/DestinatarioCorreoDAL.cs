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
    public class DestinatarioCorreoDAL
    {
        public static List<Entidades.DestinatarioCorreo> ObtenerDestinatarios()
        {
            List<Entidades.DestinatarioCorreo> lista = new List<Entidades.DestinatarioCorreo>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_DEST_DESTINATARIO_GET");

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int CORREO = reader.GetOrdinal("CORREO");
                int NOMBRE = reader.GetOrdinal("NOMBRE");


                while (reader.Read())
                {
                    Entidades.DestinatarioCorreo OBJ = new Entidades.DestinatarioCorreo();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Correo = (String)(!reader.IsDBNull(CORREO) ? reader.GetValue(CORREO) : string.Empty);
                    OBJ.Nombre = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
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
