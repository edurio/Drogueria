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
    public class ProductoDAL
    {
        public static List<Entidades.Producto> Obtener(Entidades.Filtro filtro)
        {
            List<Entidades.Producto> lista = new List<Entidades.Producto>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_USR_PRODUCTOS_GET");

            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId);
            

            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int NOMBRE = reader.GetOrdinal("DESCRIPCION");               
                int CLASE = reader.GetOrdinal("CLASE");


                while (reader.Read())
                {
                    Entidades.Producto OBJ = new Entidades.Producto();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Descripcion = (String)(!reader.IsDBNull(NOMBRE) ? reader.GetValue(NOMBRE) : string.Empty);
                    OBJ.Clase = (String)(!reader.IsDBNull(CLASE) ? reader.GetValue(CLASE) : string.Empty);


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
