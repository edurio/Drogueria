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
    public class RLProductosDAL
    {
        public static Entidades.RLProductos InsertarRelacionProducto(Entidades.RLProductos rlProductos)
        {
            Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_RL_PROD_INTERNO_PROD_EXTERNO_INS");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, rlProductos.Id);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, rlProductos.Est_Id != 0 ? rlProductos.Est_Id : (object)null);
            db.AddInParameter(dbCommand, "PROD_ID_INTERNO", DbType.Int32, rlProductos.Prod_Id_Drogueria != 0 ? rlProductos.Prod_Id_Drogueria : (object)null);
            db.AddInParameter(dbCommand, "DESCRIPCION_INTERNA", DbType.String, rlProductos.Descripcion_Drogueria != null ? rlProductos.Descripcion_Drogueria : (object)null);
            db.AddInParameter(dbCommand, "PROD_ID_EXTERNO", DbType.Int32, rlProductos.Prod_Id_Externo != 0 ? rlProductos.Prod_Id_Externo : (object)null);
            db.AddInParameter(dbCommand, "DESCRIPCION_EXTERNA", DbType.String, rlProductos.Descripcion_Externa != null ? rlProductos.Descripcion_Externa : (object)null);

            rlProductos.Id = int.Parse(db.ExecuteScalar(dbCommand).ToString());

            return rlProductos;
        }
        public static List<Entidades.RLProductos> ObtenerProductosRelacionados(Entidades.Filtro filtro)
        {
            List<Entidades.RLProductos> lista = new List<Entidades.RLProductos>();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("baseDatosDROGUERIA");
            DbCommand dbCommand = db.GetStoredProcCommand("SP_RL_PROD_INTERNO_PROD_EXTERNO_LEER");

            db.AddInParameter(dbCommand, "ID", DbType.Int32, filtro.Id != 0 ? filtro.Id : (object)null);
            db.AddInParameter(dbCommand, "EST_ID", DbType.Int32, filtro.Est_Id != 0 ? filtro.Est_Id : (object)null);


            IDataReader reader = (IDataReader)db.ExecuteReader(dbCommand);

            try
            {
                int ID = reader.GetOrdinal("ID");
                int EST_ID = reader.GetOrdinal("EST_ID");
                int PROD_ID_INTERNO = reader.GetOrdinal("PROD_ID_INTERNO");
                int DESCRIPCION_INTERNA = reader.GetOrdinal("DESCRIPCION_INTERNA");
                int PROD_ID_EXTERNO = reader.GetOrdinal("PROD_ID_EXTERNO");
                int DESCRIPCION_EXTERNA = reader.GetOrdinal("DESCRIPCION_EXTERNA");

                while (reader.Read())
                {
                    Entidades.RLProductos OBJ = new Entidades.RLProductos();
                    //BeginFields
                    OBJ.Id = (int)(!reader.IsDBNull(ID) ? reader.GetValue(ID) : 0);
                    OBJ.Est_Id = (int)(!reader.IsDBNull(EST_ID) ? reader.GetValue(EST_ID) : 0);
                    OBJ.Prod_Id_Drogueria = (int)(!reader.IsDBNull(PROD_ID_INTERNO) ? reader.GetValue(PROD_ID_INTERNO) : 0);
                    OBJ.Descripcion_Drogueria = (String)(!reader.IsDBNull(DESCRIPCION_INTERNA) ? reader.GetValue(DESCRIPCION_INTERNA) : string.Empty);
                    OBJ.Prod_Id_Externo = (int)(!reader.IsDBNull(PROD_ID_EXTERNO) ? reader.GetValue(PROD_ID_EXTERNO) : 0);
                    OBJ.Descripcion_Externa = (String)(!reader.IsDBNull(DESCRIPCION_EXTERNA) ? reader.GetValue(DESCRIPCION_EXTERNA) : string.Empty);
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
