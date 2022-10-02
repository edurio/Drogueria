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
    public class UsuarioDAL
    {
        public static List<Entidades.Usuario> ValidarUsuario(Entidades.Filtro filtro)
        {
            List<Entidades.Usuario> lista = new List<Entidades.Usuario>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_USR_USUARIO_GET");

            db.AddInParameter(dbCommand, "CORREO", DbType.String, filtro.Nombre);
            db.AddInParameter(dbCommand, "CLAVE", DbType.String, filtro.Password);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int EMP_ID = reader.GetOrdinal("EMP_ID");
                int NOMBRE = reader.GetOrdinal("NOMBRE");
                //int CORREO = reader.GetOrdinal("CORREO");
                //int TELEFONO = reader.GetOrdinal("TELEFONO");
                int CLAVE = reader.GetOrdinal("PASSWORD");
                int EST_ID = reader.GetOrdinal("EST_ID");
                

                while (reader.Read())
                {
                    Entidades.Usuario OBJ = new Entidades.Usuario();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.EmpId = (int)(!reader.IsDBNull(EMP_ID) ? reader.GetValue(EMP_ID) : 0);
                    OBJ.Nombre = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
                    //  OBJ.Correo = (String)(!reader.IsDBNull(CORREO) ? reader.GetValue(CORREO) : string.Empty);
                    OBJ.Est_id = (int)(!reader.IsDBNull(EST_ID) ? reader.GetValue(EST_ID) : 0);


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
