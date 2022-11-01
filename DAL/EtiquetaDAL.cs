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
    public class EtiquetaDAL
    {
        public static List<Entidades.Etiqueta> Obtener(Entidades.Filtro filtro)
        {
            List<Entidades.Etiqueta> lista = new List<Entidades.Etiqueta>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_GET_ETIQUETA");

            db.AddInParameter(dbCommand, "EMP_ID", DbType.Int32, filtro.EmpId);
            db.AddInParameter(dbCommand, "AÑO", DbType.Int32, filtro.Año);
            db.AddInParameter(dbCommand, "NUMERO", DbType.Int32, filtro.Numero != 0 ? filtro.Numero : (object)null);


            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int PROD_ID = reader.GetOrdinal("PROD_ID");
                int ARTICULO = reader.GetOrdinal("ARTICULO");
                int LOTE = reader.GetOrdinal("LOTE");
                int CANTIDAD = reader.GetOrdinal("CANTIDAD");
                int FECHA_VENCIMIENTO = reader.GetOrdinal("FECHA_VENCIMIENTO");

                while (reader.Read())
                {
                    Entidades.Etiqueta OBJ = new Entidades.Etiqueta();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.ProdId = (int)(!reader.IsDBNull(PROD_ID) ? reader.GetValue(PROD_ID) : 0);
                    OBJ.Articulo = (String)(!reader.IsDBNull(ARTICULO) ? reader.GetValue(ARTICULO) : string.Empty);
                    OBJ.Lote = (String)(!reader.IsDBNull(LOTE) ? reader.GetValue(LOTE) : string.Empty);
                    OBJ.FechaVencimiento = (DateTime)(!reader.IsDBNull(FECHA_VENCIMIENTO) ? reader.GetValue(FECHA_VENCIMIENTO) : DateTime.MinValue);
                    OBJ.Cantidad = (decimal)(!reader.IsDBNull(CANTIDAD) ? reader.GetValue(CANTIDAD) : 0);
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
