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
    public class RolDAL
    {
        public static Entidades.Rol InsertarRolUsuario(Entidades.Rol rol)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_RL_USR_ROL_INS");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, rol.RL_Usr_Rol_Id);
            db.AddInParameter(dbCommand, "USR_ID", DbType.Int32, rol.Usr_Id != 0 ? rol.Usr_Id : (object)null);
            db.AddInParameter(dbCommand, "ROL_ID", DbType.Int32, rol.Id != 0 ? rol.Id : (object)null);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, rol.Eliminado == true ? 1 : 0);
            db.AddInParameter(dbCommand, "CONTADOR", DbType.Int32, rol.Contador);

            db.ExecuteNonQuery(dbCommand);

            return rol;
        }
        public static List<Entidades.Rol> ObtenerRoles()
        {
            List<Entidades.Rol> lista = new List<Entidades.Rol>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_ROL_ROLES_LEER");

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int ROL = reader.GetOrdinal("ROL");

                while (reader.Read())
                {
                    Entidades.Rol OBJ = new Entidades.Rol();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.RolStr = (String)(!reader.IsDBNull(ROL) ? reader.GetValue(ROL) : string.Empty);
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
        public static List<Entidades.Rol> ObtenerRolesUsuario(int idUsuario)
        {
            List<Entidades.Rol> lista = new List<Entidades.Rol>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA_Solicitudes");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_RL_USR_ROL_LEER");

            db.AddInParameter(dbCommand, "USR_ID", DbType.Int32, idUsuario);

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int USR_ID = reader.GetOrdinal("USR_ID");
                int ROL_ID = reader.GetOrdinal("ROL_ID");
                int ROL = reader.GetOrdinal("ROL");

                while (reader.Read())
                {
                    Entidades.Rol OBJ = new Entidades.Rol();
                    //BeginFields
                    OBJ.RL_Usr_Rol_Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Usr_Id = (int)(!reader.IsDBNull(USR_ID) ? reader.GetValue(USR_ID) : 0);
                    OBJ.Id = (int)(!reader.IsDBNull(ROL_ID) ? reader.GetValue(ROL_ID) : 0);
                    OBJ.RolStr = (String)(!reader.IsDBNull(ROL) ? reader.GetValue(ROL) : string.Empty);
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
