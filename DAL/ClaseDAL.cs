using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.Common;


//
namespace DAL
{
    public class ClaseDAL
    {
        public static List<Entidades.Prioridad> Obtener(Entidades.Filtro filtro)
        {
            List<Entidades.Prioridad> lista = new List<Entidades.Prioridad>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("sp_clas_clases_productos_get_all");

            db.AddInParameter(dbCommand, "EMPID", DbType.Int32, filtro.EmpId);


            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int NOMBRE = reader.GetOrdinal("nombre");
                //nt DEFAULT = reader.GetOrdinal("DEFAULT");


                while (reader.Read())
                {
                    Entidades.Prioridad OBJ = new Entidades.Prioridad();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Descripcion = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
                    //OBJ.Default = (bool)(!reader.IsDBNull(DEFAULT) ? reader.GetValue(DEFAULT) : false);


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
