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
        public static Entidades.Usuario GuardaUsuario(Entidades.Usuario usuario)
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_USR_USUARIO_SOL_INS");


            db.AddInParameter(dbCommand, "ID", DbType.Int32, usuario.Id);
            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, usuario.EmpId);
            db.AddInParameter(dbCommand, "NOMBRE", DbType.String, usuario.Nombre);
            db.AddInParameter(dbCommand, "USERNAME", DbType.String, usuario.Username);
            db.AddInParameter(dbCommand, "PASSWORD", DbType.String, usuario.Password);
            db.AddInParameter(dbCommand, "CORREO", DbType.String, usuario.Correo);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, usuario.Est_id);
            db.AddInParameter(dbCommand, "ELIMINADO", DbType.Byte, usuario.Eliminado == true ? 1 : 0);

            usuario.Id = int.Parse(db.ExecuteScalar(dbCommand).ToString());

            return usuario;
        }
        public static List<Entidades.Usuario> ObtenerUsuarios(Entidades.Filtro filtro)
        {
            List<Entidades.Usuario> lista = new List<Entidades.Usuario>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_USR_USUARIO_LEER");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, filtro.Id != 0 ? filtro.Id : (object)null);
            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId != 0 ? filtro.EmpId : (object)null);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, filtro.Est_Id != 0 ? filtro.Est_Id : (object)null);


            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int EMP_ID = reader.GetOrdinal("EMP_ID");
                int EMPRESA = reader.GetOrdinal("EMPRESA");
                int EST_ID = reader.GetOrdinal("EST_ID");
                int ESTABLECIMIENTO = reader.GetOrdinal("ESTABLECIMIENTO");
                int NOMBRE = reader.GetOrdinal("NOMBRE");
                int CORREO = reader.GetOrdinal("MAIL");
                int USERNAME = reader.GetOrdinal("USERNAME");
                int PASSWORD = reader.GetOrdinal("PASSWORD");

                while (reader.Read())
                {
                    Entidades.Usuario OBJ = new Entidades.Usuario();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.EmpId = (int)(!reader.IsDBNull(EMP_ID) ? reader.GetValue(EMP_ID) : 0);
                    OBJ.Empresa = (String)(!reader.IsDBNull(EMPRESA) ? reader.GetValue(EMPRESA) : string.Empty);
                    OBJ.Est_id = (int)(!reader.IsDBNull(EST_ID) ? reader.GetValue(EST_ID) : 0);
                    OBJ.Establecimiento = (String)(!reader.IsDBNull(ESTABLECIMIENTO) ? reader.GetValue(ESTABLECIMIENTO) : string.Empty);
                    OBJ.Nombre = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
                    OBJ.Correo = (String)(!reader.IsDBNull(CORREO) ? reader.GetValue(CORREO) : string.Empty);
                    OBJ.Username = (String)(!reader.IsDBNull(USERNAME) ? reader.GetValue(USERNAME) : string.Empty);
                    OBJ.Password = (String)(!reader.IsDBNull(PASSWORD) ? reader.GetValue(PASSWORD) : string.Empty);
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

            if (filtro.TraeRoles == true)
            {
                foreach (var d in lista)
                {
                    d.ListaRoles = DAL.RolDAL.ObtenerRolesUsuario(d.Id);
                }
            }


            return lista;

        }
        public static void EliminarUsuario(int id)
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_USR_USUARIO_DEL");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, id);


            db.ExecuteNonQuery(dbCommand);

        }

    }
}
